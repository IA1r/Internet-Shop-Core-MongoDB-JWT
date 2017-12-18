using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Core.Dto;
using Core.Model;
using Core.Interface.Repository;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.IO;
using Core.GoogleAPI;
using Core.Interface;
using MongoDB.Driver;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;

namespace Data.Repository
{
	/// <summary>
	/// Implements repository for products.
	/// </summary>
	/// <seealso cref="Core.Interface.Repository.IProductRepository" />
	public class ProductRepository : RepositoryBase<Product>, IProductRepository
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ProductRepository"/> class.
		/// </summary>
		/// <param name="dataAccess">The data access.</param>
		public ProductRepository(IDataAccess dataAccess) : base(dataAccess)
		{
		}

		/// <summary>
		/// Gets the all of products from database.
		/// </summary>
		/// <returns>List of products</returns>
		public async Task<List<Product>> GetProductsAsync()
		{
			List<BsonDocument> document = await this.Documents
					.Aggregate()
					.Project(Builders<Product>.Projection
						.Include("_id")
						.Include("Type")
						.Include("Tags")
						.Include("Characteristics.Name")
						.Include("Characteristics.Price")
						.Include("Characteristics.Image"))
					.ToListAsync();

			List<Product> products = new List<Product>();

			foreach (BsonDocument item in document)
			{
				products.Add(BsonSerializer.Deserialize<Product>(item));
			}

			return products;
		}

		/// <summary>
		/// Gets the products by type.
		/// </summary>
		/// <param name="type">The product type.</param>
		/// <returns>List of products</returns>
		public async Task<List<Product>> GetProductsAsync(string type)
		{
			List<BsonDocument> document = await this.Documents
					.Aggregate()
					.Match(p => p.Type == type)
					.Project(Builders<Product>.Projection
						.Include("_id")
						.Include("Type")
						.Include("Tags")
						.Include("Characteristics.Name")
						.Include("Characteristics.Price")
						.Include("Characteristics.Image"))
					.ToListAsync();

			List<Product> products = new List<Product>();

			foreach (BsonDocument item in document)
			{
				products.Add(BsonSerializer.Deserialize<Product>(item));
			}

			return products;
		}

		/// <summary>
		/// Gets the specified product.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns>Product</returns>
		public async Task<Product> GetProductAsync(Guid id)
		{
			Product product = await this.Documents
				.Find(p => p._Id == id)
				.SingleOrDefaultAsync();

			foreach (var item in product.Tags.Select((value, i) => new { i, value }))
			{
				product.Tag += item.value;
				if (item.i != product.Tags.Length - 1)
					product.Tag += "|";
			}

			return product;
		}

		/// <summary>
		/// Gets the all of product types.
		/// </summary>
		/// <returns>List of product types</returns>
		public async Task<List<string>> GetProductTypesAsync()
		{
			List<string> types = await this.Documents
						.Distinct(p => p.Type, p => p._Id != null)
						.ToListAsync();

			return types;
		}

		/// <summary>
		/// Initializes the characteristics fields for client side.
		/// </summary>
		/// <param name="type">The product type.</param>
		/// <returns>Empty product characteristics for specified type</returns>
		public async Task<Product> InitDictionaryFieldsAsync(string type)
		{
			BsonDocument document = await this.Documents
				.Aggregate()
				.Match(p => p.Type == type)
				.Project(Builders<Product>.Projection
					.Include("Characteristics"))
				.FirstOrDefaultAsync();

			Product product = BsonSerializer.Deserialize<Product>(document);
			product._Id = Guid.Empty;

			foreach (var item in product.Characteristics.ToList())
			{
				product.Characteristics[item.Key] = "";
			}

			return product;
		}

		/// <summary>
		/// Adds the product to database.
		/// </summary>
		/// <param name="product">The product.</param>
		/// <returns>The product id</returns>
		public async Task<Guid> AddProductToDBAsync(Product product)
		{
			product.Tags = product.Tag.Split('|');

			await this.Documents.InsertOneAsync(product);

			return product._Id;
		}

		/// <summary>
		/// Updates the product.
		/// </summary>
		/// <param name="product">The product dto.</param>
		public async Task UpdateProductAsync(Product product)
		{
			product.Tags = product.Tag.Split('|');

			await this.Documents.ReplaceOneAsync(p => p._Id == product._Id, product);
		}

		/// <summary>
		/// Updates the product image.
		/// </summary>
		/// <param name="productID">The product identifier.</param>
		/// <param name="file">The image file.</param>
		/// <returns>The image id</returns>
		public async Task<string> UpdateProductImageAsync(Guid productID, IFormFile file)
		{
			string imageID = null;

			using (var fileStream = file.OpenReadStream())
			using (var ms = new MemoryStream())
			{
				fileStream.CopyTo(ms);
				imageID = DriveAPI.UploadImage(ms, file.FileName);
			}

			BsonDocument document = await this.Documents
				.Aggregate()
				.Match(p => p._Id == productID)
				.Project(Builders<Product>.Projection
					.Include("Characteristics.Image"))
				.FirstOrDefaultAsync();

			Product product = BsonSerializer.Deserialize<Product>(document);

			DriveAPI.DeleteFile(product.Characteristics["Image"]);
			product.Characteristics["Image"] = imageID;

			UpdateDefinition<Product> update = Builders<Product>.Update.Set("Characteristics.Image", product.Characteristics["Image"]);
			await this.Documents.UpdateOneAsync(p => p._Id == productID, update);

			return imageID;
		}

		/// <summary>
		/// Searches the products by id(for admin project) or keyword.
		/// </summary>
		/// <param name="id">The product id.</param>
		/// <param name="keyword">The keyword.</param>
		/// <returns>List of products</returns>
		public async Task<List<Product>> SearchProductsAsync(Guid id, string keyword)
		{
			FilterDefinition<Product> filter = null;
			if (id != Guid.Empty)
				filter = Builders<Product>.Filter.Where(p => p._Id == id);

			if (!string.IsNullOrWhiteSpace(keyword))
				filter = Builders<Product>.Filter.Regex("Characteristics.Name", new BsonRegularExpression(keyword, "i"));

			List<Product> products = await this.Documents
				.Find(filter)
				.ToListAsync();

			return products;
		}
	}
}

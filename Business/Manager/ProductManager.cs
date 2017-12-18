using Core.Dto;
using Core.Interface.Manager;
using Core.Interface.Repository;
using Core.Model;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Business.Manager
{
	/// <summary>
	/// Implements functionality to manage products
	/// </summary>
	/// <seealso cref="Core.Interface.Manager.IProductManager" />
	public class ProductManager : IProductManager
	{
		/// <summary>
		/// The product repository
		/// </summary>
		private IProductRepository productRepository;

		/// <summary>
		/// Initializes a new instance of the <see cref="ProductManager"/> class.
		/// </summary>
		/// <param name="productRepository">The product repository.</param>
		public ProductManager(IProductRepository productRepository)
		{
			this.productRepository = productRepository;
		}

		/// <summary>
		/// Gets the all of products from database.
		/// </summary>
		/// <returns>List of products</returns>
		public async Task<List<Product>> GetProductsAsync()
		{
			return await this.productRepository.GetProductsAsync();
		}

		/// <summary>
		/// Gets the products by type.
		/// </summary>
		/// <param name="type">Product type</param>
		/// <returns> List of products</returns>
		/// <exception cref="ArgumentException">Invalid type</exception>
		public async Task<List<Product>> GetProductsAsync(string type)
		{
			if (string.IsNullOrWhiteSpace(type))
				throw new ArgumentException("Invalid type");

			return await this.productRepository.GetProductsAsync(type);
		}

		/// <summary>
		/// Gets the specified product.
		/// </summary>
		/// <param name="id">The product identifier.</param>
		/// <returns> Product </returns>
		/// <exception cref="ArgumentException">Invalid product id</exception>
		public async Task<Product> GetProductAsync(Guid id)
		{
			if(id == Guid.Empty)
				throw new ArgumentException("Invalid product id");

			return await this.productRepository.GetProductAsync(id);
		}

		/// <summary>
		/// Gets the all of product types.
		/// </summary>
		/// <returns>List of product types</returns>
		public async Task<List<string>> GetProductTypesAsync()
		{
			return await this.productRepository.GetProductTypesAsync();
		}

		/// <summary>
		/// Initializes the dictionary fields for client side.
		/// </summary>
		/// <param name="type"></param>
		/// <returns> Empty product for specified type </returns>
		/// <exception cref="ArgumentException">Invalid type</exception>
		public async Task<Product> InitDictionaryFieldsAsync(string type)
		{
			if (string.IsNullOrWhiteSpace(type))
				throw new ArgumentException("Invalid type");

			return await this.productRepository.InitDictionaryFieldsAsync(type);
		}

		/// <summary>
		/// Adds the product to database.
		/// </summary>
		/// <param name="product">The product dto.</param>
		/// <returns>The product </returns>
		public async Task<Guid> AddProductToDBAsync(Product product)
		{
			return await this.productRepository.AddProductToDBAsync(product);
		}

		/// <summary>
		/// Updates the product.
		/// </summary>
		/// <param name="product">The product dto.</param>
		public async Task UpdateProductAsync(Product product)
		{
			await this.productRepository.UpdateProductAsync(product);
		}

		/// <summary>
		/// Updates the product image.
		/// </summary>
		/// <param name="productID">The product identifier.</param>
		/// <param name="file">The image file.</param>
		/// <returns>The image identifier</returns>
		/// <exception cref="ArgumentException">Some troublehappened</exception>
		public async Task<string> UpdateProductImageAsync(Guid productID, IFormFile file)
		{
			if (file.Length <= 0 || productID == Guid.Empty)
				throw new ArgumentException("Some troublehappened");

			return await this.productRepository.UpdateProductImageAsync(productID, file);
		}

		/// <summary>
		/// Searches the products by keyword.
		/// </summary>
		/// <param name="id">The product id.</param>
		/// <param name="keyword">The keyword.</param>
		/// <returns>List of products</returns>
		public async Task<List<Product>> SearchProductsAsync(Guid id, string keyword)
		{
			if (id == null && string.IsNullOrWhiteSpace(keyword))
				return null;

			return await this.productRepository.SearchProductsAsync(id, keyword);
		}
	}
}

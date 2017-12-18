using Core.Dto;
using Core.Model;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface.Repository
{
	/// <summary>
	/// Represents methods for ProductRepository. 
	/// </summary>
	public interface IProductRepository: IRepositoryBase<Product>
	{
		/// <summary>
		/// Gets the all of products from database.
		/// </summary>
		/// <returns>List of products</returns>
		Task<List<Product>> GetProductsAsync();

		/// <summary>
		/// Gets the products by type.
		/// </summary>
		/// <param name="type">The product type.</param>
		/// <returns>List of products</returns>
		Task<List<Product>> GetProductsAsync(string type);

		/// <summary>
		/// Gets the specified product.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns>Product</returns>
		Task<Product> GetProductAsync(Guid id);

		/// <summary>
		/// Gets the all of product types.
		/// </summary>
		/// <returns>List of product types</returns>
		Task<List<string>> GetProductTypesAsync();

		/// <summary>
		/// Initializes the characteristics fields for client side.
		/// </summary>
		/// <param name="type">The product type.</param>
		/// <returns>Empty product characteristics for specified type</returns>
		Task<Product> InitDictionaryFieldsAsync(string type);

		/// <summary>
		/// Adds the product to database.
		/// </summary>
		/// <param name="product">The product.</param>
		/// <returns>The product id</returns>
		Task<Guid> AddProductToDBAsync(Product product);

		/// <summary>
		/// Updates the product.
		/// </summary>
		/// <param name="product">The product dto.</param>
		Task UpdateProductAsync(Product product);

		/// <summary>
		/// Updates the product image.
		/// </summary>
		/// <param name="productID">The product identifier.</param>
		/// <param name="file">The image file.</param>
		/// <returns>The image id</returns>
		Task<string> UpdateProductImageAsync(Guid productID, IFormFile file);

		/// <summary>
		/// Searches the products by id(for admin project) or keyword.
		/// </summary>
		/// <param name="id">The product id.</param>
		/// <param name="keyword">The keyword.</param>
		/// <returns>List of products</returns>
		Task<List<Product>> SearchProductsAsync(Guid id, string keyword);
	}
}

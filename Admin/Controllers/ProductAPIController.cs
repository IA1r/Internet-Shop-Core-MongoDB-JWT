using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Core.Interface.Manager;
using Core.Dto;
using Admin.RequestModel;
using System.IO;
using Core.GoogleAPI;
using Admin.ResopnseModel;
using Core.Model;
using Microsoft.AspNetCore.Authorization;

namespace Admin.Controllers
{
	/// <summary>
	/// Implemets API controller to manage produtcs
	/// </summary>
	/// <seealso cref="Microsoft.AspNetCore.Mvc.Controller" />
	[Produces("application/json")]
	[Route("api/ProductAPI/[action]")]
	public class ProductAPIController : Controller
	{
		/// <summary>
		/// The product manager
		/// </summary>
		private IProductManager productManager;

		/// <summary>
		/// The response status
		/// </summary>
		private ResponseStatusModel responseStatus;

		/// <summary>
		/// Initializes a new instance of the <see cref="ProductAPIController"/> class.
		/// </summary>
		/// <param name="productManager">The product manager.</param>
		public ProductAPIController(IProductManager productManager)
		{
			this.productManager = productManager;
		}

		/// <summary>
		/// Gets the all of products from database
		/// </summary>
		[HttpGet]
		public async Task<IActionResult> GetProducts()
		{
			IEnumerable<Product> produtcs = await this.productManager.GetProductsAsync();
			this.responseStatus = new ResponseStatusModel { Success = true };
			return Ok(new { ResponseStatus = this.responseStatus, Products = produtcs });
		}

		/// <summary>
		/// Gets the specified product.
		/// </summary>
		/// <param name="id">The identifier.</param>
		[Authorize]
		[HttpGet("{id}")]
		public async Task<IActionResult> GetProduct(Guid id)
		{
			Product product = await this.productManager.GetProductAsync(id);
			if (product != null)
			{
				this.responseStatus = new ResponseStatusModel { Success = true };
				return Ok(new { ResponseStatus = this.responseStatus, Product = product });
			}
			else
			{
				this.responseStatus = new ResponseStatusModel { Code = 404, Message = $"Produc with id - {id} not Found", Success = false };
				return NotFound(new { ResponseStatus = this.responseStatus });
			}
		}

		/// <summary>
		/// Gets the product types.
		/// </summary>
		[Authorize]
		[HttpGet]
		public async Task<IActionResult> GetProductTypes()
		{
			List<string> types = await this.productManager.GetProductTypesAsync();
			this.responseStatus = new ResponseStatusModel { Success = true };
			return Ok(new { ResponseStatus = this.responseStatus, Types = types });

		}

		/// <summary>
		/// Initializes the dictionary fields.
		/// </summary>
		/// <param name="id">The identifier.</param>
		[Authorize]
		[HttpGet("{type}")]
		public async Task<IActionResult> InitDictionaryFields(string type)
		{
			try
			{
				Product emptyProduct = await this.productManager.InitDictionaryFieldsAsync(type);
				this.responseStatus = new ResponseStatusModel { Success = true };
				return Ok(new { ResponseStatus = this.responseStatus, Product = emptyProduct });
			}
			catch (ArgumentException ex)
			{
				this.responseStatus = new ResponseStatusModel { Success = false, Message = ex.Message, Code = 400 };
				return BadRequest(new { ResponseStatus = this.responseStatus });
			}
		}

		/// <summary>
		/// Adds the product to database.
		/// </summary>
		/// <param name="request">The request.</param>
		[Authorize]
		[HttpPost]
		public async Task<IActionResult> AddProductToDB([FromBody]ProductRequestModel request)
		{
			try
			{
				Guid productID = await this.productManager.AddProductToDBAsync(new Product
				{
					Type = request.Type,
					Tag = request.Tag,
					Characteristics = request.Characteristics
				});

				this.responseStatus = new ResponseStatusModel { Success = true };
				return Ok(new { ResponseStatus = this.responseStatus, ProducID = productID });
			}
			catch (ArgumentException ex)
			{
				this.responseStatus = new ResponseStatusModel { Success = false, Message = ex.Message, Code = 400 };
				return BadRequest(new { ResponseStatus = this.responseStatus });
			}
		}

		/// <summary>
		/// Updates the product.
		/// </summary>
		/// <param name="request">The request.</param>
		[Authorize]
		[HttpPost]
		public async Task<IActionResult> UpdateProduct([FromBody]ProductRequestModel request)
		{
			try
			{
				await this.productManager.UpdateProductAsync(new Product
				{
					_Id = request._Id,
					Tag = request.Tag,
					Type = request.Type,
					Characteristics = request.Characteristics
				});

				this.responseStatus = new ResponseStatusModel { Success = true, Message = "Data successfully updated" };
				return Ok(new { ResponseStatus = this.responseStatus });
			}
			catch (ArgumentException ex)
			{
				this.responseStatus = new ResponseStatusModel { Success = false, Message = ex.Message, Code = 400 };
				return BadRequest(new { ResponseStatus = this.responseStatus });
			}
		}

		/// <summary>
		/// Uploads product image.
		/// </summary>
		/// <param name="id">The product identifier.</param>
		/// <param name="file">The file.</param>
		[Authorize]
		[HttpPost("{id}")]
		public async Task<IActionResult> ImageUpload(Guid id, IFormFile file)
		{
			try
			{
				string imageID = await this.productManager.UpdateProductImageAsync(id, file);
				this.responseStatus = new ResponseStatusModel { Success = true };
				return Ok(new { ResponseStatus = this.responseStatus, ImageID = imageID });
			}
			catch (ArgumentException ex)
			{
				this.responseStatus = new ResponseStatusModel { Success = false, Message = ex.Message, Code = 400 };
				return BadRequest(new { ResponseStatus = this.responseStatus });
			}
		}

		/// <summary>
		/// Searches the product.
		/// </summary>
		/// <param name="keyword">The keyword.</param>
		[Authorize]
		[HttpGet("{id}/{keyword}")]
		public async Task<IActionResult> SearchProduct(Guid id, string keyword)
		{
			IEnumerable<Product> products = await this.productManager.SearchProductsAsync(id, keyword);
			if (products != null && products.Count() > 0)
			{
				this.responseStatus = new ResponseStatusModel { Success = true };
				return Ok(new { ResponseStatus = this.responseStatus, Products = products });
			}

			this.responseStatus = new ResponseStatusModel { Success = false, Message = "No result." };
			return Ok(new { ResponseStatus = this.responseStatus });
		}
	}
}

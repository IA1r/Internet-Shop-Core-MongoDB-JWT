using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Core.Interface.Manager;
using Core.Dto;
using Shop.RequestModel;
using Core.Model;
using Microsoft.AspNetCore.Authorization;
using Shop.ResopnseModel;
using Core.Interface;
using Core.Helpers;
using Core.Const;

namespace Shop.Controllers
{
	/// <summary>
	/// Implemets API controller to manage products
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
		/// The cart manager
		/// </summary>
		private ICartManager cartManager;

		/// <summary>
		/// The response status
		/// </summary>
		private ResponseStatusModel responseStatus;

		/// <summary>
		/// Initializes a new instance of the <see cref="ProductAPIController"/> class.
		/// </summary>
		/// <param name="productManager">The product manager.</param>
		/// <param name="cartManager">The cart manager.</param>
		public ProductAPIController(IProductManager productManager, ICartManager cartManager)
		{
			this.productManager = productManager;
			this.cartManager = cartManager;
		}

		/// <summary>
		/// Gets the products.
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		public async Task<IActionResult> GetProducts()
		{
			List<Product> produtcs = await this.productManager.GetProductsAsync();
			this.responseStatus = new ResponseStatusModel { Success = true };

			return Ok(new { ResponseStatus = this.responseStatus, Products = produtcs });
		}

		///// <summary>
		///// Gets the products by specified type.
		///// </summary>
		///// <param name="id">The type identifier.</param>
		[HttpGet("{type}")]
		public async Task<IActionResult> GetProducts(string type)
		{
			List<Product> produtcs = await this.productManager.GetProductsAsync(type);
			this.responseStatus = new ResponseStatusModel { Success = true };
			return Ok(new { ResponseStatus = this.responseStatus, Products = produtcs });
		}

		/// <summary>
		/// Gets the specified product.
		/// </summary>
		/// <param name="id">The product identifier.</param>
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

		///// <summary>
		///// Adds the product to shopping cart.
		///// </summary>
		///// <param name="id">The product identifier.</param>
		[Authorize]
		[HttpPost("{id}")]
		public async Task<IActionResult> AddProduct(Guid id)
		{
			try
			{
				string token = Request.Headers.FirstOrDefault(h => h.Key == "Authorization").Value;
				Guid userID = new Guid(JWTHelper.GetClaimData(token.Remove(0, token.LastIndexOf(' ') + 1), ClaimsTypeConst.ID));

				await this.cartManager.AddProductAsync(id, userID);

				this.responseStatus = new ResponseStatusModel { Success = true, Message = "Product successfully added to the Shopping Cart" };
				return Ok(new { ResponseStatus = responseStatus });
			}
			catch (ArgumentException ex)
			{
				this.responseStatus = new ResponseStatusModel { Code = 404, Message = ex.Message, Success = false };
				return NotFound(new { ResponseStatus = this.responseStatus });
			}
		}

		///// <summary>
		///// Gets the shopping cart.
		///// </summary>
		[Authorize]
		[HttpGet]
		public async Task<IActionResult> GetShoppingCart()
		{
			try
			{
				string token = Request.Headers.FirstOrDefault(h => h.Key == "Authorization").Value;
				Guid userID = new Guid(JWTHelper.GetClaimData(token.Remove(0, token.LastIndexOf(' ') + 1), ClaimsTypeConst.ID));
				ShoppingCart cart = await this.cartManager.GetShoppingCartAsync(userID);

				if (cart == null)
				{
					this.responseStatus = new ResponseStatusModel { Success = true, Message = "Shopping Cart is empty.", Code = 204 };
					return NoContent();
				}

				this.responseStatus = new ResponseStatusModel { Success = true };
				return Ok(new { ResponseStatus = this.responseStatus, Cart = cart });
			}
			catch (ArgumentException ex)
			{
				this.responseStatus = new ResponseStatusModel { Success = true, Message = ex.Message, Code = 204 };
				return NoContent();
			}
		}

		///// <summary>
		///// Deletes the product from, shopping cart.
		///// </summary>
		///// <param name="id">The product identifier.</param>
		[Authorize]
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteItem(Guid id)
		{
			try
			{
				string token = Request.Headers.FirstOrDefault(h => h.Key == "Authorization").Value;
				Guid userID = new Guid(JWTHelper.GetClaimData(token.Remove(0, token.LastIndexOf(' ') + 1), ClaimsTypeConst.ID));

				await this.cartManager.DeleteItemAsync(userID, id);
				this.responseStatus = new ResponseStatusModel { Success = true };
				return Ok(new { ResponseStatus = this.responseStatus });
			}
			catch (ArgumentException ex)
			{
				this.responseStatus = new ResponseStatusModel { Success = false, Message = ex.Message, Code = 404 };
				return NotFound(new { ResponseStatus = this.responseStatus });
			}
		}

		///// <summary>
		///// Checkouts from shopping cart.
		///// </summary>
		///// <param name="request">The request.</param>
		[Authorize]
		[HttpPost]
		public async Task<IActionResult> Checkout([FromBody]CheckoutRequestModel request)
		{
			try
			{
				await this.cartManager.CheckoutAsync(new User
				{
					_Id = request._Id,
					Name = request.Name,
					Phone = request.Phone,
					DeliveryAddress = request.DeliveryAddress,	
				});

				this.responseStatus = new ResponseStatusModel { Success = true };
				return Ok(new { ResponseStatus = this.responseStatus });
			}
			catch (ArgumentException ex)
			{
				return NotFound(new { Success = false, Error = ex.Message });
			}
		}

		///// <summary>
		///// Searches the product by keyword.
		///// </summary>
		///// <param name="keyword">The keyword.</param>
		[HttpGet("{keyword}")]
		public async Task<IActionResult> SearchProduct(string keyword)
		{
			IEnumerable<Product> products = await this.productManager.SearchProductsAsync(Guid.Empty, keyword);
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

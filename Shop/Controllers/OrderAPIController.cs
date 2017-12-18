using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Core.Interface.Manager;
using Shop.ResopnseModel;
using Core.Helpers;
using Core.Const;
using Core.Model;
using Microsoft.AspNetCore.Authorization;

namespace Shop.Controllers
{
	/// <summary>
	/// Implemets API controller to manage orders
	/// </summary>
	/// <seealso cref="Microsoft.AspNetCore.Mvc.Controller" />
	[Produces("application/json")]
	[Route("api/OrderAPI/[action]")]
	public class OrderAPIController : Controller
	{
		/// <summary>
		/// The order manager
		/// </summary>
		private IOrderManager orderManager;

		/// <summary>
		/// The response status
		/// </summary>
		private ResponseStatusModel responseStatus;

		/// <summary>
		/// Initializes a new instance of the <see cref="OrderAPIController"/> class.
		/// </summary>
		/// <param name="orderManager">The order manager.</param>
		public OrderAPIController(IOrderManager orderManager)
		{
			this.orderManager = orderManager;
		}

		/// <summary>
		/// Gets the specified order.
		/// </summary>
		/// <param name="id">The identifier.</param>
		[Authorize]
		[HttpGet("{id}")]
		public async Task<IActionResult> GetOrder(Guid id)
		{
			try
			{
				Order order = await this.orderManager.GetOrderAsync(id);

				this.responseStatus = new ResponseStatusModel { Success = true };
				return Ok(new { ResponseStatus = this.responseStatus, Order = order });
			}
			catch (ArgumentException ex)
			{
				this.responseStatus = new ResponseStatusModel { Success = true, Message = ex.Message, Code = 404 };
				return NotFound(new { ResponseStatus = this.responseStatus });
			}
		}

		/// <summary>
		/// Gets the order list.
		/// </summary>
		[Authorize]
		[HttpGet]
		public async Task<IActionResult> GetOrderList()
		{
			string token = Request.Headers.FirstOrDefault(h => h.Key == "Authorization").Value;
			string userID = JWTHelper.GetClaimData(token.Remove(0, token.LastIndexOf(' ') + 1), ClaimsTypeConst.ID);

			IEnumerable<Order> orders = await this.orderManager.GetOrderListAsync(userID);

			if (orders == null || orders.Count() == 0)
				return NoContent();

			this.responseStatus = new ResponseStatusModel { Success = true };
			return Ok(new { ResponseStatus = this.responseStatus, OrderList = orders });
		}
	}
}

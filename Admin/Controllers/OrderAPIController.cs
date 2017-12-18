using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Core.Interface.Manager;
using Core.Dto;
using Admin.ResopnseModel;
using Core.Model;
using Microsoft.AspNetCore.Authorization;

namespace Admin.Controllers
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
		/// Gets the order list of all users.
		/// </summary>
		[Authorize]
		[HttpGet]
		public async Task<IActionResult> GetOrderList()
		{
			IEnumerable<Order> orders = await this.orderManager.GetOrderListAsync();

			if (orders == null || orders.Count() == 0)
				return NoContent();

			this.responseStatus = new ResponseStatusModel { Success = true };
			return Ok(new { ResponseStatus = this.responseStatus, OrderList = orders });
		}

		/// <summary>
		/// Gets the order.
		/// </summary>
		/// <param name="orderID">The order identifier.</param>
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
		/// Confirms the specified order.
		/// </summary>
		/// <param name="id">The order identifier.</param>
		[Authorize]
		[HttpPost("{id}")]
		public async Task<IActionResult> ConfirmOrder(Guid id)
		{
			try
			{
				await this.orderManager.ConfirmOrderAsync(id);
				this.responseStatus = new ResponseStatusModel { Success = true };
				return Ok(new { ResponseStatus = this.responseStatus });
			}
			catch (ArgumentException ex)
			{
				this.responseStatus = new ResponseStatusModel { Success = true, Message = ex.Message, Code = 404 };
				return NotFound(new { ResponseStatus = this.responseStatus });
			}
		}

		/// <summary>
		/// Deletes the specified order.
		/// </summary>
		/// <param name="id">The order identifier.</param>
		[Authorize]
		[HttpPost("{id}")]
		public async Task<IActionResult> DeleteOrder(Guid id)
		{
			try
			{
				await this.orderManager.DeleteOrderAsync(id);
				this.responseStatus = new ResponseStatusModel { Success = true };
				return Ok(new { ResponseStatus = this.responseStatus });
			}
			catch (ArgumentException ex)
			{
				this.responseStatus = new ResponseStatusModel { Success = true, Message = ex.Message, Code = 404 };
				return NotFound(new { ResponseStatus = this.responseStatus });
			}
		}
	}
}

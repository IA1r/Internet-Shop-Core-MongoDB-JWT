using Core.Dto;
using Core.Interface.Manager;
using Core.Interface.Repository;
using Core.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Business.Manager
{
	/// <summary>
	/// Implements functionality to manage order.
	/// </summary>
	/// <seealso cref="Core.Interface.Manager.IOrderManager" />
	public class OrderManager : IOrderManager
	{
		/// <summary>
		/// The order repository
		/// </summary>
		private IOrderRepository orderRepository;

		/// <summary>
		/// Initializes a new instance of the <see cref="OrderManager"/> class.
		/// </summary>
		/// <param name="orderRepository">The order repository.</param>
		public OrderManager(IOrderRepository orderRepository)
		{
			this.orderRepository = orderRepository;
		}

		/// <summary>
		/// Gets the order.
		/// </summary>
		/// <param name="orderID">The order identifier.</param>
		/// <returns>Order object</returns>
		/// <exception cref="ArgumentException">Invalid order ID</exception>
		public async Task<Order> GetOrderAsync(Guid orderID)
		{
			if (orderID == Guid.Empty)
				throw new ArgumentException("Invalid order ID");

			return await this.orderRepository.GetOrderAsync(orderID);
		}

		/// <summary>
		/// Gets the order list for specified user.
		/// </summary>
		/// <param name="userID">The user identifier.</param>
		/// <returns>Order list</returns>
		public async Task<List<Order>> GetOrderListAsync(string userID)
		{
			if (string.IsNullOrWhiteSpace(userID))
				return null;

			return await this.orderRepository.GetOrderListAsync(userID);
		}

		/// <summary>
		/// Gets the order list of all users.
		/// </summary>
		/// <returns>Order list</returns>
		public async Task<List<Order>> GetOrderListAsync()
		{
			return await this.orderRepository.GetOrderListAsync();
		}

		/// <summary>
		/// Confirms the specified order.
		/// </summary>
		/// <param name="orderID">The order identifier.</param>
		/// <exception cref="ArgumentException">Invalid order id</exception>
		public async Task ConfirmOrderAsync(Guid orderID)
		{
			if (orderID == Guid.Empty)
				throw new ArgumentException("Invalid order id");

			await this.orderRepository.ConfirmOrderAsync(orderID);
		}

		/// <summary>
		/// Deletes the specified order.
		/// </summary>
		/// <param name="orderID">The order identifier.</param>
		public async Task DeleteOrderAsync(Guid orderID)
		{
			if (orderID == Guid.Empty)
				throw new ArgumentException("Invalid order ID");

			await this.orderRepository.DeleteOrderAsync(orderID);
		}
	}
}

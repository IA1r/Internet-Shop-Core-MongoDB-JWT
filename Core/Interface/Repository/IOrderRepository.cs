using Core.Dto;
using Core.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface.Repository
{
	/// <summary>
	/// Represents methods for OrderRepository. 
	/// </summary>
	public interface IOrderRepository
	{

		/// <summary>
		/// Creates the order.
		/// </summary>
		/// <param name="order">The order.</param>
		Task CreateOrder(Order order);

		/// <summary>
		/// Gets the order.
		/// </summary>
		/// <param name="orderID">The order identifier.</param>
		/// <returns> Order object </returns>
		Task<Order> GetOrderAsync(Guid orderID);

		/// <summary>
		/// Gets the order list for specified user.
		/// </summary>
		/// <param name="userID">The user identifier.</param>
		/// <returns>Order list</returns>
		Task<List<Order>> GetOrderListAsync(string userID);

		/// <summary>
		/// Gets the order list of all users.
		/// </summary>
		/// <returns>Order list</returns>
		Task<List<Order>> GetOrderListAsync();

		/// <summary>
		/// Confirms the specified order.
		/// </summary>
		/// <param name="orderID">The order identifier.</param>
		Task ConfirmOrderAsync(Guid orderID);

		/// <summary>
		/// Deletes the specified order.
		/// </summary>
		/// <param name="orderID">The order identifier.</param>
		Task DeleteOrderAsync(Guid orderID);
	}
}

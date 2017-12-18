using Core.Dto;
using Core.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Core.Interface.Repository;
using Core.Interface;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;

namespace Data.Repository
{
	/// <summary>
	/// Implements repository for orders.
	/// </summary>
	/// <seealso cref="Core.Interface.Repository.IOrderRepository" />
	public class OrderRepository : RepositoryBase<Order>, IOrderRepository
	{

		/// <summary>
		/// Initializes a new instance of the <see cref="OrderRepository"/> class.
		/// </summary>
		/// <param name="dataAccess">The data access.</param>
		public OrderRepository(IDataAccess dataAccess) : base(dataAccess)
		{
		}


		/// <summary>
		/// Creates the order.
		/// </summary>
		/// <param name="order">The order.</param>
		public async Task CreateOrder(Order order)
		{
			await this.Documents.InsertOneAsync(order);
		}

		/// <summary>
		/// Gets the order.
		/// </summary>
		/// <param name="orderID">The order identifier.</param>
		/// <returns> Order object </returns>
		public async Task<Order> GetOrderAsync(Guid orderID)
		{
			BsonDocument document = await this.Documents
				.Aggregate()
				.Match(o => o._Id == orderID)
				.Project(Builders<Order>.Projection
				.Include("User.Name")
				.Include("User.Phone")
				.Include("User.DeliveryAddress")
				.Include("Date")
				.Include("IsApprove")
				.Include("TotalPrice")
				.Include("Products"))
				.FirstOrDefaultAsync();

			if(document == null)
				throw new ArgumentException("Order not found");

			Order order = BsonSerializer.Deserialize<Order>(document);

			return order;
		}

		/// <summary>
		/// Gets the order list for specified user.
		/// </summary>
		/// <param name="userID">The user identifier.</param>
		/// <returns>Order list</returns>
		public async Task<List<Order>> GetOrderListAsync(string userID)
		{
			List<Order> orders = await this.Documents
				.Find(o => o.User._Id == new Guid(userID))
				.ToListAsync();

			return orders;
		}

		/// <summary>
		/// Gets the order list of all users.
		/// </summary>
		/// <returns>Order list</returns>
		public async Task<List<Order>> GetOrderListAsync()
		{
			List<Order> orders = await this.Documents
				.Find(o => true)
				.ToListAsync();

			return orders;
		}

		/// <summary>
		/// Confirms the specified order.
		/// </summary>
		/// <param name="orderID">The order identifier.</param>
		/// <exception cref="ArgumentException">Order not found</exception>
		public async Task ConfirmOrderAsync(Guid orderID)
		{
			Order order = await this.Documents
				.Find(o => o._Id == orderID)
				.SingleOrDefaultAsync();

			if (order == null)
				throw new ArgumentException("Order not found");

			order.IsApprove = true;
			await this.Documents.ReplaceOneAsync(o => o._Id == orderID, order);
		}

		/// <summary>
		/// Deletes the specified order.
		/// </summary>
		/// <param name="orderID">The order identifier.</param>
		/// <exception cref="ArgumentException">Order not found</exception>
		public async Task DeleteOrderAsync(Guid orderID)
		{
			var result = await this.Documents.DeleteOneAsync(o => o._Id == orderID);

			if(!result.IsAcknowledged)
				throw new ArgumentException("Order not found");
		}
	}
}

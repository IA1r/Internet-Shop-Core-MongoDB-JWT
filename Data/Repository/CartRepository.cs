using Core.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Core.Dto;
using Core.Interface.Repository;
using System.Threading.Tasks;
using System.Globalization;
using Microsoft.EntityFrameworkCore;
using Core.Interface;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;

namespace Data.Repository
{

	/// <summary>
	/// Implements repository for shopping cart.
	/// </summary>
	/// <seealso cref="Core.Interface.Repository.ICartRepository" />
	public class CartRepository : RepositoryBase<ShoppingCart>, ICartRepository
	{
		/// <summary>
		/// The user repository
		/// </summary>
		private IUserRepository userRepository;

		/// <summary>
		/// The product repository
		/// </summary>
		private IProductRepository productRepository;

		/// <summary>
		/// The order repository
		/// </summary>
		private IOrderRepository orderRepository;

		/// <summary>
		/// Initializes a new instance of the <see cref="CartRepository"/> class.
		/// </summary>
		/// <param name="unitOfWork">The unit of work.</param>
		public CartRepository(
					IDataAccess dataAccess,
					IUserRepository userRepository,
					IProductRepository productRepository,
					IOrderRepository orderRepository) : base(dataAccess)
		{
			this.userRepository = userRepository;
			this.productRepository = productRepository;
			this.orderRepository = orderRepository;
		}


		/// <summary>
		/// Adds the product to shoping cart.
		/// </summary>
		/// <param name="productID">The product identifier.</param>
		/// <param name="userID">The user/cart identifier.</param>
		public async Task AddProductAsync(Guid productID, Guid userID)
		{
			Product product = await this.productRepository.GetProductAsync(productID);

			if (await this.Documents.Find(sc => sc._Id == userID).SingleOrDefaultAsync() == null)
			{
				User user = await this.userRepository.FindByIdAsync(userID);
				this.CreateDocument(new ShoppingCart { _Id = userID, UserName = user.Login, Products = new[] { product } });
			}
			else
			{
				var updateProducts = Builders<ShoppingCart>.Update	
							.Push<Product>(sc => sc.Products, product);

				await this.Documents.FindOneAndUpdateAsync(sc => sc._Id == userID, updateProducts);
			}
		}


		/// <summary>
		/// Gets the shopping cart.
		/// </summary>
		/// <param name="id">The shoppingcart identifier.</param>
		/// <returns>Shopping Cart.</returns>
		public async Task<ShoppingCart> GetShoppingCartAsync(Guid id)
		{
			ShoppingCart shoppingCart = await this.Documents
				.Find(sc => sc._Id == id)
				.SingleOrDefaultAsync();

			if (shoppingCart == null)
				return shoppingCart;

			double totalPrice = 0.0;
			foreach (Product product in shoppingCart.Products)
			{
				totalPrice += Convert.ToDouble(product.Characteristics["Price"], CultureInfo.InvariantCulture);
			}
			shoppingCart.TotalPrice = totalPrice;

			return shoppingCart;
		}

		/// <summary>
		/// Deletes the item from shopping cart.
		/// </summary>
		/// <param name="productID">The product identifier.</param>
		/// <param name="cartID">The cart identifier.</param>
		public async Task DeleteItemAsync(Guid cartID, Guid productID)
		{
			var updateProducts = Builders<ShoppingCart>.Update
							.PullFilter<Product>(sc => sc.Products, p => p._Id == productID);

			await this.Documents.FindOneAndUpdateAsync(sc => sc._Id == cartID, updateProducts);
		}


		/// <summary>
		/// Checkouts from shopping cart.
		/// </summary>
		/// <param name="user">The user data.</param>
		public async Task CheckoutAsync(User user)
		{
			BsonDocument document = await this.Documents
					.Aggregate()
					.Match(d => d._Id == user._Id)
					.Project(Builders<ShoppingCart>.Projection
					.Include("Products"))
					.SingleOrDefaultAsync();

			ShoppingCart cart = BsonSerializer.Deserialize<ShoppingCart>(document);

			double totalPrice = 0.0;
			foreach (Product product in cart.Products)
			{
				totalPrice += Convert.ToDouble(product.Characteristics["Price"], CultureInfo.InvariantCulture);
			}

			Order order = new Order
			{
				User = user,
				Products = cart.Products,
				Date = DateTime.Now,
				TotalPrice = totalPrice
			};

			await this.orderRepository.CreateOrder(order);

			var updateProducts = Builders<ShoppingCart>.Update
							.PullFilter<Product>(sc => sc.Products, p => p._Id != null);

			await this.Documents.FindOneAndUpdateAsync(sc => sc._Id == user._Id, updateProducts);
		}
	}
}

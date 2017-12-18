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
	/// Implements functionality to manage shopping cart
	/// </summary>
	/// <seealso cref="Core.Interface.Manager.ICartManager" />
	public class CartManager : ICartManager
	{
		/// <summary>
		/// The shopping cart repository
		/// </summary>
		private ICartRepository cartRepository;
		/// <summary>
		/// Initializes a new instance of the <see cref="CartManager"/> class.
		/// </summary>
		/// <param name="cartRepository">The cart repository.</param>
		public CartManager(ICartRepository cartRepository)
		{
			this.cartRepository = cartRepository;
		}

		/// <summary>
		/// Gets the shopping cart products.
		/// </summary>
		/// <param name="id">The user identifier.</param>
		/// <returns>Shopping Cart</returns>
		/// <exception cref="ArgumentException">Invalid ID".</exception>
		public async Task<ShoppingCart> GetShoppingCartAsync(Guid id)
		{
			if (id == Guid.Empty)
				throw new ArgumentException("Invalid ID");

			return await this.cartRepository.GetShoppingCartAsync(id);
		}

		/// <summary>
		/// Adds the product to shopping cart.
		/// </summary>
		/// <param name="productID">The product identifier.</param>
		/// <param name="userID">The user/cart identifier.</param>
		/// <exception cref="ArgumentException">Invalid ID</exception>
		public async Task AddProductAsync(Guid productID, Guid userID)
		{
			if (productID == Guid.Empty || userID == Guid.Empty)
				throw new ArgumentException("Invalid ID");

			await this.cartRepository.AddProductAsync(productID, userID);
		}

		/// <summary>
		/// Deletes the item from shopping cart.
		/// </summary>
		/// <param name="productID">The product identifier.</param>
		/// <param name="cartID">The cart identifier.</param>
		/// <exception cref="ArgumentException">Invalid ID</exception>
		public async Task DeleteItemAsync(Guid cartID, Guid productID)
		{
			if (cartID == Guid.Empty || productID == Guid.Empty)
				throw new ArgumentException("Invalid ID");

			await this.cartRepository.DeleteItemAsync(cartID, productID);
		}

		/// <summary>
		/// Checkouts from shopping cart.
		/// </summary>
		/// <param name="user">The User data.</param>
		public async Task CheckoutAsync(User user)
		{
			await this.cartRepository.CheckoutAsync(user);
		}
	}
}

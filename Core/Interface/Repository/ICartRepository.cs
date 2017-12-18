using Core.Dto;
using Core.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface.Repository
{
	/// <summary>
	/// Represents methods for CartRepository. 
	/// </summary>
	public interface ICartRepository
	{
		/// <summary>
		/// Gets the shopping cart products.
		/// </summary>
		/// <param name="userID">The user identifier.</param>
		/// <returns>Shopping Cart Dto</returns>
		Task<ShoppingCart> GetShoppingCartAsync(Guid id);

		/// <summary>
		/// Adds the product to shopping cart.
		/// </summary>
		/// <param name="productID">The product identifier.</param>
		/// <param name="userID">The user/cart identifier.</param>
		Task AddProductAsync(Guid productID, Guid userID);

		/// <summary>
		/// Deletes the item from shopping cart.
		/// </summary>
		/// <param name="cartContentID">The cart item identifier.</param>
		Task DeleteItemAsync(Guid cartID, Guid productID);

		/// <summary>
		/// Checkouts from shopping cart.
		/// </summary>
		/// <param name="order">The order.</param>
		Task CheckoutAsync(User user);
	}
}

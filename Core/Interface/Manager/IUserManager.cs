using Core.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface.Manager
{
	/// <summary>
	/// Represents methods for UserRepository. 
	/// </summary>
	public interface IUserManager
	{
		/// <summary>
		/// Finds the by login asynchronous.
		/// </summary>
		/// <param name="login">The login.</param>
		/// <returns>User</returns>
		Task<User> FindByLoginAsync(string login);

		/// <summary>
		/// Finds the by email asynchronous.
		/// </summary>
		/// <param name="email">The email.</param>
		/// <returns>User</returns>
		Task<User> FindByEmailAsync(string email);

		/// <summary>
		/// Creates the user.
		/// </summary>
		/// <param name="user">The user.</param>
		/// <returns>User</returns>
		User CreateUser(User user);

		/// <summary>
		/// Gets the user data for checkout.
		/// </summary>
		/// <param name="userID">The user identifier.</param>
		/// <returns>User</returns>
		Task<User> GetUserDataForCheckoutAsync(string userID);
	}
}

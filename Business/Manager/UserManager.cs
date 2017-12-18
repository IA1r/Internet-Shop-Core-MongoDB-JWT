using Core.Interface.Manager;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Core.Interface.Repository;
using Data.Repository;
using Core.Interface;
using Core.Model;
using System.Threading.Tasks;

namespace Business.Manager
{
	/// <summary>
	/// Implements functionality to manage users
	/// </summary>
	/// <seealso cref="Core.Interface.Manager.IUserManager" />
	public class UserManager : IUserManager
	{
		/// <summary>
		/// The user repository
		/// </summary>
		private IUserRepository userRepository;

		/// <summary>
		/// Initializes a new instance of the <see cref="UserManager"/> class.
		/// </summary>
		/// <param name="userRepository">The user repository.</param>
		public UserManager(IUserRepository userRepository)
		{
			this.userRepository = userRepository;
		}

		/// <summary>
		/// Finds the by email asynchronous.
		/// </summary>
		/// <param name="email">The email.</param>
		/// <returns>User</returns>
		public async Task<User> FindByEmailAsync(string email)
		{
			return await this.userRepository.FindByEmailAsync(email);
		}

		/// <summary>
		/// Finds the by login asynchronous.
		/// </summary>
		/// <param name="login">The login.</param>
		/// <returns> User </returns>
		public async Task<User> FindByLoginAsync(string login)
		{
			return await this.userRepository.FindByLoginAsync(login);
		}

		/// <summary>
		/// Creates the user.
		/// </summary>
		/// <param name="user">The user.</param>
		/// <returns> User </returns>
		public User CreateUser(User user)
		{
			return this.userRepository.CreateDocument(user);
		}

		/// <summary>
		/// Gets the user data for checkout.
		/// </summary>
		/// <param name="userID">The user identifier.</param>
		/// <returns> User </returns>
		public Task<User> GetUserDataForCheckoutAsync(string userID)
		{
			return this.userRepository.GetUserDataForCheckoutAsync(userID);
		}
	}
}

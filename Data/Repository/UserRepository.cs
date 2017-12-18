using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Core.Model;
using Core.Interface.Repository;
using Core.Interface;
using MongoDB.Driver;

using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;

namespace Data.Repository
{
	/// <summary>
	/// Implements repository for users.
	/// </summary>
	/// <seealso cref="Data.Repository.RepositoryBase{Core.Model.User}" />
	/// <seealso cref="Core.Interface.Repository.IUserRepository" />
	public class UserRepository : RepositoryBase<User>, IUserRepository
	{

		/// <summary>
		/// Initializes a new instance of the <see cref="UserRepository"/> class.
		/// </summary>
		/// <param name="dataAccess">The data access.</param>
		public UserRepository(IDataAccess dataAccess) : base(dataAccess)
		{
		}


		/// <summary>
		/// Finds the by identifier asynchronous.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns>User</returns>
		public async Task<User> FindByIdAsync(Guid id)
		{
			User user = await this.Documents
				.Find(u => u._Id == id)
				.SingleOrDefaultAsync();

			return user;
		}

		/// <summary>
		/// Finds the by login asynchronous.
		/// </summary>
		/// <param name="login">The login.</param>
		/// <returns>User</returns>
		public async Task<User> FindByLoginAsync(string login)
		{
			User user = await this.Documents
				.Find(u => u.Login == login)
				.SingleOrDefaultAsync();

			return user;
		}

		/// <summary>
		/// Finds the by email asynchronous.
		/// </summary>
		/// <param name="email">The email.</param>
		/// <returns>User</returns>
		public async Task<User> FindByEmailAsync(string email)
		{
			User user = await this.Documents
				.Find(u => u.Email == email)
				.SingleOrDefaultAsync();

			return user;
		}

		/// <summary>
		/// Gets the user data for checkout.
		/// </summary>
		/// <param name="userID">The user identifier.</param>
		/// <returns>User data</returns>
		public async Task<User> GetUserDataForCheckoutAsync(string userID)
		{
			BsonDocument document = await this.Documents
					.Aggregate()
					.Match(d => d._Id == new Guid(userID))
					.Project(Builders<User>.Projection
					.Include("Name")
					.Include("Phone")
					.Include("DeliveryAddress"))
					.SingleOrDefaultAsync();

			User user = BsonSerializer.Deserialize<User>(document);

			return user;
		}
	}
}

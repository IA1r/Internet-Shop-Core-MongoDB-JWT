using Core.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface.Repository
{
	public interface IUserRepository : IRepositoryBase<User>
	{
		Task<User> FindByLoginAsync(string login);
		Task<User> FindByEmailAsync(string email);
		Task<User> FindByIdAsync(Guid id);
		Task<User> GetUserDataForCheckoutAsync(string userID);
	}
}

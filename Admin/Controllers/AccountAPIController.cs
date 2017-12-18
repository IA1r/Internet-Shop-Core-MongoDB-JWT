using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Core.Model;
using Admin.RequestModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Admin.ResopnseModel;
using Core.Interface.Manager;
using Core.Helpers;

namespace Admin.Controllers
{
	/// <summary>
	/// Implemets API controller to manage users
	/// </summary>
	/// <seealso cref="Microsoft.AspNetCore.Mvc.Controller" />
	[Produces("application/json")]
	[Route("api/AccountAPI/[action]")]
	public class AccountAPIController : Controller
	{
		/// <summary>
		/// The user manager
		/// </summary>
		private readonly IUserManager userManager;

		/// <summary>
		/// The response status
		/// </summary>
		private ResponseStatusModel responseStatus;

		/// <summary>
		/// Initializes a new instance of the <see cref="AccountAPIController"/> class.
		/// </summary>
		/// <param name="userManager">The user manager.</param>
		public AccountAPIController(IUserManager userManager)
		{
			this.userManager = userManager;
		}

		/// <summary>
		/// Registrations the specified user.
		/// </summary>
		/// <param name="model">The model.</param>
		[HttpPost]
		public async Task<IActionResult> Registration([FromBody]RegistrationRequestModel model)
		{
			if (await this.userManager.FindByLoginAsync(model.Login) != null)
			{
				this.responseStatus = new ResponseStatusModel { Success = false, Message = "This login already use.", Code = 409 };
				return StatusCode(409, new { ResponseStatus = this.responseStatus });
			}

			if (await this.userManager.FindByEmailAsync(model.Email) != null)
			{
				this.responseStatus = new ResponseStatusModel { Success = false, Message = "This email already use.", Code = 409 };
				return StatusCode(409, new { ResponseStatus = this.responseStatus });
			}

			User user = this.userManager
				.CreateUser(new User
				{
					Login = model.Login,
					Email = model.Email,
					Password = model.Password,
					Country = model.Country,
					Year = model.Year,
					Roles = new string[] { "admin" }
				});

			if (user != null)
			{
				this.responseStatus = new ResponseStatusModel { Success = true };
				return Ok(new { ResponseStatus = this.responseStatus });
			}

			this.responseStatus = new ResponseStatusModel { Success = false, Code = 400 };
			return BadRequest(new { ResponseStatus = this.responseStatus });
		}

		/// <summary>
		/// Authorization the specified user.
		/// </summary>
		/// <param name="model">The model.</param>
		/// <returns></returns>
		[HttpPost]
		public async Task<IActionResult> SignIn([FromBody]SignInRequestModel model)
		{
			User user = await this.userManager.FindByLoginAsync(model.Login);
			if (user != null)
			{
				if(user.Roles.Any(r => r != "admin"))
				{
					this.responseStatus = new ResponseStatusModel { Success = false, Message = "This user is not an administrator", Code = 400 };
					return BadRequest(new { ResponseStatus = this.responseStatus });
				}

				string token = JWTHelper.GetToken(user);

				this.responseStatus = new ResponseStatusModel { Success = true };
				return Ok(new { ResponseStatus = this.responseStatus, Token = token });
			}
			else
			{
				this.responseStatus = new ResponseStatusModel { Success = false, Message = "Invalid login or password", Code = 400 };
				return BadRequest(new { ResponseStatus = this.responseStatus });
			}
		}
	}
}

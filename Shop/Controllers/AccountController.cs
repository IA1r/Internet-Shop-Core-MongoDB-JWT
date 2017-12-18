using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shop.RequestModel;
using Core.Model;
using Microsoft.AspNetCore.Authorization;
using Shop.ResopnseModel;
using Core.Interface.Manager;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Newtonsoft.Json;
using System.Security.Principal;
using Core.Const;
using Core.Helpers;

namespace Shop.Controllers
{
	/// <summary>
	/// Implemets API controller to manage users
	/// </summary>
	/// <seealso cref="Microsoft.AspNetCore.Mvc.Controller" />
	[Produces("application/json")]
	[Route("api/[controller]/[action]")]
	public class AccountController : Controller
	{
		/// <summary>
		/// The user manager
		/// </summary>
		private readonly IUserManager userManager;

		/// <summary>
		/// The response status
		/// </summary>
		private ResponseStatusModel responseStatus;

		///// <summary>
		///// Initializes a new instance of the <see cref="AccountController"/> class.
		///// </summary>
		///// <param name="userManager">The user manager.</param>
		public AccountController(IUserManager userManager)
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
					Roles = new string[] { "user" }
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
		[HttpPost]
		public async Task<IActionResult> SignIn([FromBody]SignInRequestModel model)
		{
			User user = await this.userManager.FindByLoginAsync(model.Login);
			if (user != null)
			{
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

		[Authorize]
		[HttpGet]
		public async Task<IActionResult> GetUserDataForCheckout()
		{
			string token = Request.Headers.FirstOrDefault(h => h.Key == "Authorization").Value;
			string userID = JWTHelper.GetClaimData(token.Remove(0, token.LastIndexOf(' ') + 1), ClaimsTypeConst.ID);

			User user = await this.userManager.GetUserDataForCheckoutAsync(userID);
			if(user != null)
			{
				this.responseStatus = new ResponseStatusModel { Success = true };
				return Ok(new { ResponseStatus = this.responseStatus, User = user });
			}
			else
			{
				this.responseStatus = new ResponseStatusModel { Success = false, Message = "User data not found", Code = 400 };
				return NotFound(new { ResponseStatus = this.responseStatus });
			}
		}
	}
}

using Core.Const;
using Core.Model;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Core.Helpers
{
	public static class JWTHelper
	{
		private static JwtSecurityTokenHandler handler;
		public static string GetToken(User user)
		{
			if (handler == null)
				handler = new JwtSecurityTokenHandler();

			var securityKey = new SymmetricSecurityKey(JWTOptionsConst.GenerateKey());
			var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

			var identity = new ClaimsIdentity(new[] {
												new Claim(ClaimsTypeConst.ID, user._Id.ToString()),
												new Claim(ClaimsTypeConst.LOGIN, user.Login),
												new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Roles[0] )});

			var token = handler.CreateJwtSecurityToken(subject: identity,
													   signingCredentials: signingCredentials,
													   audience: JWTOptionsConst.AUDIENCE,
													   issuer: JWTOptionsConst.ISSUER,
													   expires: DateTime.UtcNow.AddMinutes(JWTOptionsConst.EXPIRES));

			return handler.WriteToken(token);
		}

		public static string GetClaimData(string token, string type)
		{
			if (handler == null)
				handler = new JwtSecurityTokenHandler();

			string data = handler.ReadJwtToken(token).Claims.First(claim => claim.Type == type).Value;
			return data;
		}
	}
}

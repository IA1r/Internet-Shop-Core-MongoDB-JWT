using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Const
{
	public static class JWTOptionsConst
	{
		public const string ISSUER = "InternetShop";
		public const string AUDIENCE = "http://localhost:0000/";
		public const string KEY = "super_secret_key";
		public const int EXPIRES = 15;


		public static byte[] GenerateKey()
		{
			return Encoding.UTF8.GetBytes(KEY);
		}
	}
}

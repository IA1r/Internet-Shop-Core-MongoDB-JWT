using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Configuration
{
	/// <summary>
	/// Representds MongoDB connection settings.
	/// </summary>
	public class MongoSettings
	{
		/// <summary>
		/// The database name.
		/// </summary>
		public string DatabaseName;

		/// <summary>
		/// The connection string.
		/// </summary>
		public string ConnectionString;
	}
}

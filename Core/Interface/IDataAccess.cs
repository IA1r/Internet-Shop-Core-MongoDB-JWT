using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Interface
{
	/// <summary>
	/// Represents fields to access Mongo database.
	/// </summary>
	public interface IDataAccess
	{
		/// <summary>
		/// Gets the Mongo database.
		/// </summary>
		IMongoDatabase Database { get; }
	}
}

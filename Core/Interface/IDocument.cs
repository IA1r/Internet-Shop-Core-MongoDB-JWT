using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Interface
{
	/// <summary>
	/// Represents common fields for MongoDB documents.
	/// </summary>
	public interface IDocument
	{
		/// <summary>
		/// Gets or sets the document identifier.
		/// </summary>
		Guid _Id { get; set; }
	}
}

using Core.Interface;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Model
{
	public class Order : IDocument
	{
		/// <summary>
		/// Gets or sets the order identifier.
		/// </summary>
		[BsonId]
		public Guid _Id { get; set; }

		/// <summary>
		/// Gets or sets the user data.
		/// </summary>
		[BsonElement("User")]
		public User User { get; set; }

		/// <summary>
		/// Gets or sets the list of products.
		/// </summary>
		[BsonElement("Products")]
		public Product[] Products { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this instance is approve.
		/// </summary>
		/// <value>
		///   <c>true</c> if this instance is approve; otherwise, <c>false</c>.
		/// </value>
		[BsonElement("IsApprove")]
		public bool IsApprove { get; set; }

		/// <summary>
		/// Gets or sets the date.
		/// </summary>
		[BsonElement("Date")]
		public DateTime Date { get; set; }

		[BsonElement("TotalPrice")]
		public double TotalPrice { get; set; }

	}
}

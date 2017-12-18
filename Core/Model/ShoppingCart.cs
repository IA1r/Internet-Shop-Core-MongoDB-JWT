using Core.Interface;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Core.Model
{
	public class ShoppingCart : IDocument
	{
		[BsonId]
		public Guid _Id { get; set; }

		[BsonElement("UserName")]
		public string UserName { get; set; }

		[BsonElement("Products")]
		public Product[] Products { get; set; }

		[BsonIgnore]
		public double TotalPrice { get; set; }
	}
}

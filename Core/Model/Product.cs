using Core.Interface;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Model
{
	public class Product: IDocument
	{

		/// <summary>
		/// Gets or sets the product identifier.
		/// </summary>
		[BsonId]
		public Guid _Id { get; set; }

		[BsonElement("Type")]
		public string Type { get; set; }

		[BsonElement("Tags")]
		public string[] Tags { get; set; }

		[BsonIgnore]
		public string Tag { get; set; }

		[BsonElement("Characteristics")]
		[BsonDictionaryOptions(DictionaryRepresentation.Document)]
		public Dictionary<string,string> Characteristics { get; set; }

	}
}

using Core.Interface;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;


namespace Core.Model
{
	public class User : IDocument
	{

		/// <summary>
		/// Gets or sets the user identifier.
		/// </summary>
		[BsonId]
		public Guid _Id { get; set; }

		/// <summary>
		/// Gets or sets the login.
		/// </summary>
		[BsonElement("Login")]
		public string Login { get; set; }

		/// <summary>
		/// Gets or sets the user name.
		/// </summary>
		[BsonElement("Name")]
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the user Phone.
		/// </summary>
		[BsonElement("Phone")]
		public string Phone { get; set; }

		/// <summary>
		/// Gets or sets the email.
		/// </summary>
		[BsonElement("Email")]
		public string Email { get; set; }

		/// <summary>
		/// Gets or sets the password.
		/// </summary>
		[BsonElement("Password")]
		public string Password { get; set; }

		/// <summary>
		/// Gets or sets the country.
		/// </summary>
		[BsonElement("Country")]
		public string Country { get; set; }

		/// <summary>
		/// Gets or sets the delivery address.
		/// </summary>
		[BsonElement("DeliveryAddress")]
		public string DeliveryAddress { get; set; }

		/// <summary>
		/// Gets or sets the year.
		/// </summary>
		[BsonElement("Year")]
		public string Year { get; set; }

		/// <summary>
		/// Gets or sets the user roles.
		/// </summary>
		[BsonElement("Roles")]
		public string[] Roles { get; set; }
	}
}

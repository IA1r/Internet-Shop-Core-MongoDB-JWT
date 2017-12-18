using Core.Interface;
using Core.Interface.Repository;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Repository
{
	/// <summary>
	/// Implements Base repository.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <seealso cref="Core.Interface.Repository.IRepositoryBase{T}" />
	public class RepositoryBase<T> : IRepositoryBase<T> where T : IDocument
	{
		/// <summary>
		/// The data access
		/// </summary>
		private readonly IDataAccess dataAccess;

		/// <summary>
		/// The collection name
		/// </summary>
		private readonly string collectionName;

		/// <summary>
		/// Gets the documents.
		/// </summary>
		/// <value>
		/// The documents.
		/// </value>
		protected IMongoCollection<T> Documents
		{
			get
			{
				return this.dataAccess.Database.GetCollection<T>(this.collectionName);
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="RepositoryBase{T}"/> class.
		/// </summary>
		/// <param name="dataAccess">The data access.</param>
		public RepositoryBase(IDataAccess dataAccess)
		{
			this.dataAccess = dataAccess;

			// pluralize document type name - lets just add "s" for this test application
			this.collectionName = typeof(T).Name + "s";
		}


		/// <summary>
		/// Creates the document.
		/// </summary>
		/// <param name="item">The item.</param>
		/// <returns> T-item </returns>
		public T CreateDocument(T item)
		{
			this.Documents.InsertOne(item);

			return item;
		}
	}
}

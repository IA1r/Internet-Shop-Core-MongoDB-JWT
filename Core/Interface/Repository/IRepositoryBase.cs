using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Interface.Repository
{
	public interface IRepositoryBase<T> where T : IDocument
	{

		/// <summary>
		/// Creates the document.
		/// </summary>
		/// <param name="item">The item.</param>
		/// <returns> T-item </returns>
		T CreateDocument(T item);
	}
}

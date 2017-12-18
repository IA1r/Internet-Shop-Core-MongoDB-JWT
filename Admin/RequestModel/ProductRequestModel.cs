using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Admin.RequestModel
{
	/// <summary>
	/// Product Request Model
	/// </summary>
	public class ProductRequestModel
    {
		/// <summary>
		/// Gets or sets the product identifier.
		/// </summary>
		public Guid _Id { get; set; }
		public string Type { get; set; }
		public string Tag { get; set; }
		public Dictionary<string, string> Characteristics { get; set; }
	}
}

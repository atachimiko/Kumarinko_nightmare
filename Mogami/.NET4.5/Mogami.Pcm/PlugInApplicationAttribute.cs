using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mogami.Pcm
{
	/// <summary>
	/// This attribute is used to mark a class as a PlugIn based Application.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class)]
	public class PlugInApplicationAttribute : Attribute
	{
		/// <summary>
		/// Name of the PlugIn.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="name">Name of the PlugIn</param>
		public PlugInApplicationAttribute(string name)
		{
			Name = name;
		}
	}
}

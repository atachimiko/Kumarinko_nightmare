using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mogami.Pcm
{
	/// <summary>
	/// This attribute is used to mark a class as a PlugIn.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class)]
	public class PlugInAttribute : Attribute
	{
		#region コンストラクタ

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="name">Name of the PlugIn</param>
		public PlugInAttribute(string name, string version)
		{
			Name = name;
			Version = version;
		}

		#endregion コンストラクタ

		#region プロパティ

		/// <summary>
		/// Name of the PlugIn.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public string Version { get; set; }

		#endregion プロパティ
	}
}

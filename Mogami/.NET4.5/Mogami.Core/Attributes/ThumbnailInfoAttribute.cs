using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mogami.Core.Attributes
{
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
	public class ThumbnailInfoAttribute : System.Attribute
	{
		public String ThumbnailDirectoryName;
		public ThumbnailInfoAttribute(String str)
		{
			this.ThumbnailDirectoryName = str;
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Akalib.Wpf.Mvvm.Attribute
{
	[AttributeUsage(System.AttributeTargets.Property, Inherited = true)]
	public class ContentPropertyAttribute : System.Attribute
	{
		public BindingMode BindingMode { get; set; }

		public ContentPropertyAttribute()
		{
		}
	}
}

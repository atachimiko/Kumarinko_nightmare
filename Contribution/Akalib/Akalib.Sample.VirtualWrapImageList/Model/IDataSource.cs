using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akalib.Sample.VirtualWrapImageList.Model
{
	/// <summary>
	/// データソース
	/// </summary>
	/// <typeparam name="T"></typeparam>
	interface IDataSource<T>
		where T : ICollection
	{
		T Items { get; }
	}

}

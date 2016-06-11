using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akalib
{
	public interface IBuildAssemblyParameter
	{


		#region プロパティ

		Dictionary<string, string> Params { get; }

		#endregion プロパティ

	}
}

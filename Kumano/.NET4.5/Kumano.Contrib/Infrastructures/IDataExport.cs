using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kumano.Contrib.Infrastructures
{
	public interface IDataExport<T>
	{
		T Export();
	}
}

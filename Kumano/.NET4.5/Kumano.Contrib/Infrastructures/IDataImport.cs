using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kumano.Contrib.Infrastructures
{
	public interface IDataImport<T>
	{
		void Import(IDataExport<T> exporter);
	}
}

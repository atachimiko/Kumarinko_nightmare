using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akalib
{
	public interface IEventSource
	{
		event EventHandler Event;
	}
}

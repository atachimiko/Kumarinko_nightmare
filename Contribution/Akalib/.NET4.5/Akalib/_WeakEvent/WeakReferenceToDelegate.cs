using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akalib
{
	/// <summary>
	/// A class demonstrating a CommandManager.InvalidateRequery-like event.
	/// </summary>
	public class WeakReferenceToDelegate
	{
		List<WeakReference> handlers = new List<WeakReference>();

		public event EventHandler Event
		{
			add
			{
				// event is easily made thread-safe is desired
				lock (handlers)
				{
					// optionally clean up when adding an event handler to prevent leak
					// when event is never fired:
					handlers.RemoveAll(wr => !wr.IsAlive);

					handlers.Add(new WeakReference(value));
				}
			}
			remove
			{
				lock (handlers)
				{
					handlers.RemoveAll(wr =>
					{
						EventHandler target = (EventHandler)wr.Target;
						return target == null || target == value;
					});
				}
			}
		}

		void FireEvent(EventArgs e)
		{
			EventHandler callHandlers = null;
			lock (handlers)
			{
				for (int i = handlers.Count - 1; i >= 0; i--)
				{
					EventHandler target = (EventHandler)handlers[i].Target;
					if (target == null)
						handlers.RemoveAt(i);
					else
						callHandlers += target;
				}
			}
			// Call event handlers using a separate event handler list after removing the dead entries
			// from the old list to ensure that registering+deregistering events from within an event
			// handler works as expected.
			if (callHandlers != null)
				callHandlers(this, e);
		}
	}
}

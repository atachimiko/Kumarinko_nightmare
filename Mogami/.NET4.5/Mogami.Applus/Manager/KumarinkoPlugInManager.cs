using Mogami.Pcm;
using Mogami.Pcm.Kumarinko;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mogami.Applus.Manager
{
	[PlugInApplication("Kumarinko")]
	public class KumarinkoPlugInManager : PlugInBasedApplication<IKumarinkoOperationPlugIn>, IKumarinkoManager
	{
	}
}

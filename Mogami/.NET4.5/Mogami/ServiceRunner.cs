using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Mogami
{
	internal class ServiceRunner
	{

		#region フィールド

		private static ILog LOG = LogManager.GetLogger(typeof(ServiceRunner));

		#endregion フィールド


		#region メソッド

		[STAThread]
		private static void Main(string[] args)
		{
			System.Threading.Thread.CurrentThread.Name = "Mogami application thread";

			// Windowsサービスとして起動する
			ServiceBase[] ServicesToRun;
			ServicesToRun = new ServiceBase[]
				{
				new MogamiWindowsService()
				};
			ServiceBase.Run(ServicesToRun);
		}

		#endregion メソッド

	}
}

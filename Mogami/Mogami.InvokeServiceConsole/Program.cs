using Mogami.Core;
using Mogami.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Mogami.InvokeServiceConsole
{
	class Program
	{

		#region フィールド

		private static Application application;

		#endregion フィールド


		#region メソッド

		static void Main(string[] args)
		{
			Console.WriteLine("サーバーシステム・プログラム:");
			while (1 == 1)
			{
				Console.WriteLine("起動モードを選択してください");
				Console.WriteLine("   0: APIサーバ起動");
				Console.Write("> ");

				string d = Console.ReadLine();
				if (d == "0")
				{
					break;
				}
			}

			// Serviceサーバーを起動して、クライアントにWSDLなどを公開するためのコンソールプログラムです。
			System.Threading.Thread.CurrentThread.Name = "Halcyon API";
			application = new Application();

			var buildAssemblyType = new BuildAssemblyParameter();
			buildAssemblyType.Params["ApplicationDirectoryPath"] = @"\Mogami_InvokeServiceConsole";

			ApplicationContext.SetupApplicationContextImpl(ApplicationContextImpl.CreateInstance(application, buildAssemblyType));
			((ApplicationContextImpl)ApplicationContextImpl.GetInstance()).InitializeApplication();

			RunApiService();
		}

		private static void RunApiService()
		{
			using (ServiceHost host = new ServiceHost(typeof(MogamiApiService)))
			{
				// ① エンドポイントの手動構成設定 (C/B/A を指定)
				//   バインディングの構成設定を行いたい場合には、Binding インスタンスのプロパティを設定する
				var binder = new NetNamedPipeBinding();
				binder.MaxBufferSize = 1073741824;
				binder.MaxBufferPoolSize = 1073741824;
				binder.MaxReceivedMessageSize = 1073741824;
				binder.ReaderQuotas.MaxArrayLength = 1073741824;

				host.AddServiceEndpoint(typeof(IMogamiApiService), binder,
					"net.pipe://localhost/Kumarinko.Server/Halcyon");
				// ② ビヘイビアの手動構成設定
				//   すでにいくつかのビヘイビアは既定で追加されているため、取り払ってから再設定する
				host.Description.Behaviors.Remove(typeof(ServiceDebugBehavior));
				host.Description.Behaviors.Remove(typeof(ServiceMetadataBehavior));
				host.Description.Behaviors.Add(new ServiceDebugBehavior { IncludeExceptionDetailInFaults = true });
				host.Description.Behaviors.Add(new ServiceMetadataBehavior
				{
					HttpGetEnabled = true,
					HttpGetUrl = new Uri("http://localhost:8000/Kumarinko.Server/Mogami/mex")
				});

				// ホストのオープン
				host.Open();
				Console.WriteLine("WCF サービスを起動しました。");
				Console.ReadLine();
				host.Close();
			}
		}

		#endregion メソッド
	}
}

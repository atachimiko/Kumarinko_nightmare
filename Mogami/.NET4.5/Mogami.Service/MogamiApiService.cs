using AutoMapper;
using log4net;
using Mogami.Service.Construction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Mogami.Service
{
	[ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Single, InstanceContextMode = InstanceContextMode.Single, IncludeExceptionDetailInFaults = true)]
	public class MogamiApiService : IMogamiApiService
	{


		#region フィールド

		static ILog LOG = LogManager.GetLogger(typeof(MogamiApiService));

		static IMapper Mapper;
		static MapperConfiguration MapperConfig;

		#endregion フィールド


		#region コンストラクタ

		public MogamiApiService()
		{
			MapperConfig = new MapperConfiguration(cfg =>
			{
			});

			Mapper = MapperConfig.CreateMapper();
		}

		#endregion コンストラクタ

		#region メソッド

		/// <summary>
		/// 
		/// </summary>
		/// <param name="versionType"></param>
		/// <returns></returns>
		public string GetVersion(VERSION_SELECTOR versionType)
		{
			switch (versionType)
			{
				case VERSION_SELECTOR.API_VERSION:
					return "1.0.0";
				case VERSION_SELECTOR.DATABASE_VERSION:
					return "1.0.0";
				case VERSION_SELECTOR.SERVICE_VERSION:
					return "1.0.0";
				default:
					return "";
			}
		}

		#endregion メソッド
	}
}

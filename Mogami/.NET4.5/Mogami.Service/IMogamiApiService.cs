using Mogami.Service.Construction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Mogami.Service
{
	[ServiceContract(SessionMode = SessionMode.Required)]
	public interface IMogamiApiService
	{
		#region メソッド

		/// <summary>
		/// [S0001]
		/// サービスシステムのバージョン番号を取得します。
		/// </summary>
		/// <param name="versionType">取得したいバージョン番号の種別</param>
		/// <returns></returns>
		[OperationContract]
		string GetVersion(VERSION_SELECTOR versionType);

		#endregion メソッド
	}
}

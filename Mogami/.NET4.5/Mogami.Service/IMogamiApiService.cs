using Mogami.Service.Construction;
using Mogami.Service.Request;
using Mogami.Service.Response;
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
		/// 
		/// </summary>
		/// <param name="reqparam"></param>
		/// <returns></returns>
		[OperationContract]
		RESPONSE_ADDCATEGORY AddCategory(REQUEST_ADDCATEGORY reqparam);

		/// <summary>
		/// Artifactデータモデルの検索
		/// </summary>
		/// <param name="reqparam"></param>
		/// <returns></returns>
		[OperationContract]
		RESPONSE_FINDARTIFACT FindArtifact(REQUEST_FINDARTIFACT reqparam);

		/// <summary>
		/// サービスシステムのバージョン番号を取得します。
		/// </summary>
		/// <param name="versionType">取得したいバージョン番号の種別</param>
		/// <returns></returns>
		[OperationContract]
		RESPONSE_GETSERVERVERSION GetServerVersion(VERSION_SELECTOR versionType);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="reqparam"></param>
		/// <returns></returns>
		[OperationContract]
		RESPONSE_LOADARTIFACT LoadArtifact(REQUEST_LOADARTIFACT reqparam);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="reqparam"></param>
		/// <returns></returns>
		[OperationContract]
		RESPONSE_LOADCATGEORY LoadCategory(REQUEST_LOADCATEGORY reqparam);

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		[OperationContract]
		RESPONSE_LOADDEVICESETTING LoadDeviceSetting();

		/// <summary>
		/// 
		/// </summary>
		/// <param name="reqparam"></param>
		/// <returns></returns>
		[OperationContract]
		RESPONSE_LOADTAG LoadTag(REQUEST_LOADTAG reqparam);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="reqparam"></param>
		/// <returns></returns>
		[OperationContract]
		RESPONSE_LOADTHUMBNAIL LoadThumbnail(REQUEST_LOADTHUMBNAIL reqparam);

		[OperationContract(IsInitiating = true, IsTerminating = false)]
		void Login();

		[OperationContract(IsInitiating = false, IsTerminating = true)]
		void Logout();

		/// <summary>
		/// 
		/// </summary>
		/// <param name="reqparam"></param>
		/// <returns></returns>
		[OperationContract]
		RESPONSE_SAVEDEVICESETTING SaveDeviceSetting(REQUEST_SAVEDEVICRSETTING reqparam);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="reqparam"></param>
		/// <returns></returns>
		[OperationContract]
		RESPONSE_UPDATECATEGORY UpdateCategory(REQUEST_UPDATECATEGORY reqparam);

		#endregion メソッド

	}
}

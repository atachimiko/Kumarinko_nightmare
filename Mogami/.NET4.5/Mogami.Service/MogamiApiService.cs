using AutoMapper;
using log4net;
using Mogami.Model;
using Mogami.Service.Construction;
using Mogami.Service.Response;
using Mogami.Service.Serialized;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Mogami.Service.Request;
using Mogami.Gateway;
using Mogami.Model.Repository;

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
				cfg.CreateMap<Artifact, DataArtifact>();
			});

			Mapper = MapperConfig.CreateMapper();
		}

		#endregion コンストラクタ


		#region メソッド

		/// <summary>
		/// 
		/// </summary>
		/// <param name="reqparam"></param>
		/// <returns></returns>
		public RESPONSE_ADDCATEGORY AddCategory(REQUEST_ADDCATEGORY reqparam)
		{
			var rsp = new RESPONSE_ADDCATEGORY();
			return rsp;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="reqparam"></param>
		/// <returns></returns>
		public RESPONSE_FINDARTIFACT FindArtifact(REQUEST_FINDARTIFACT reqparam)
		{
			var rsp = new RESPONSE_FINDARTIFACT();
			
			using(var dbc = new AppDbContext())
			{
				var repo = new ArtifactRepository(dbc);
				foreach(var prop in repo.GetAll())
				{
					var mapped = Mapper.Map<DataArtifact>(prop);
					rsp.Artifacts.Add(mapped);
				}
			}
			
			rsp.Success = true;
			return rsp;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="versionType"></param>
		/// <returns></returns>
		public RESPONSE_GETSERVERVERSION GetServerVersion(VERSION_SELECTOR versionType)
		{
			var result = new RESPONSE_GETSERVERVERSION();
			switch (versionType)
			{
				case VERSION_SELECTOR.API_VERSION:
					result.VersionText = "1.0.0";
					result.Success = true;
					break;
				case VERSION_SELECTOR.DATABASE_VERSION:
					result.VersionText = "1.0.0";
					result.Success = true;
					break;
				case VERSION_SELECTOR.SERVICE_VERSION:
					result.VersionText = "1.0.0";
					result.Success = true;
					break;
				default:
					result.Success = false;
					break;
			}

			return result;
		}

		public RESPONSE_LOADCATGEORY LoadCategory(REQUEST_LOADCATEGORY reqparam)
		{
			var rsp = new RESPONSE_LOADCATGEORY();

			using(var dbc = new AppDbContext())
			{
				var repo = new CategoryRepository(dbc);
				var appliction = repo.Load(3L);
				foreach(var child in appliction.ChildCategories)
				{
					rsp.Categories.Add(new DataCategory
					{
						Id = child.Id,
						Name = child.Name,
						CategoryTypeCode = child.CategoryTypeCode,
						IsHasChild = false
					});
				}
			}

			rsp.Success = true;
			return rsp;
		}

		public RESPONSE_LOADTHUMBNAIL LoadThumbnail(REQUEST_LOADTHUMBNAIL reqparam)
		{
			var rsp = new RESPONSE_LOADTHUMBNAIL();

			using (var tdbc = new ThumbDbContext())
			{
				var repo = new ThumbnailRepository(tdbc);

				var thumb = repo.FindFromKey(reqparam.ThumbnailKey).FirstOrDefault();
				if (thumb != null)
				{
					LOG.InfoFormat("サムネイルの送信={0}", thumb.BitmapBytes.Count());
					rsp.ThumbnailBytes = thumb.BitmapBytes;
				}
			}
			rsp.Success = true;
			return rsp;
		}

		public void Login()
		{
			
		}

		public void Logout()
		{

		}

		public RESPONSE_UPDATECATEGORY UpdateCategory(REQUEST_UPDATECATEGORY reqparam)
		{
			var rsp = new RESPONSE_UPDATECATEGORY();
			return rsp;
		}

		#endregion メソッド

	}
}

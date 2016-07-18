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
using System.Windows.Media.Imaging;
using System.IO;

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
				cfg.CreateMap<Category, DataCategory>();
				cfg.CreateMap<Tag, DataTag>();
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
			IList<Artifact> artifact = null;
			using(var dbc = new AppDbContext())
			{
				var repo = new ArtifactRepository(dbc);

				switch (reqparam.TargetType)
				{
					case FINDTARGET_SELECTOR.CATEGORY:
						var r = repo.FindByCategory(new Category { Id = reqparam.TargetId });
						artifact = new List<Artifact>(r.ToArray().OrderBy(p => p.Title));
						rsp.Success = true;
						break;
					case FINDTARGET_SELECTOR.TAG:
						var rtags = repo.FindByTag(new Tag { Id = reqparam.TargetId });
						artifact = new List<Artifact>(rtags.ToArray().OrderBy(p => p.Title));
						rsp.Success = true;
						break;
					default:
						rsp.Success = false;
						rsp.Message = "不明な取得条件です";
						break;
						
				}
			}


			if (artifact != null)
			{
				foreach (var prop in artifact)
				{
					var mapped = Mapper.Map<DataArtifact>(prop);
					rsp.Artifacts.Add(mapped);
				}
			}

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

		public RESPONSE_LOADARTIFACT LoadArtifact(REQUEST_LOADARTIFACT reqparam)
		{
			var rsp = new RESPONSE_LOADARTIFACT();
			using (var dbc = new AppDbContext())
			{
				var repo = new ArtifactRepository(dbc);
				var artifact = repo.Load(reqparam.TargetArtifactId);

				var mapped = Mapper.Map<DataArtifact>(artifact);
				rsp.Artifact = mapped;

				var filePath = Path.Combine(artifact.FileMappingInfo.Workspace.PhysicalPath, artifact.FileMappingInfo.MappingFilePath);
				rsp.FilePath = filePath;
			}

			return rsp;
		}

		public RESPONSE_LOADCATGEORY LoadCategory(REQUEST_LOADCATEGORY reqparam)
		{
			var rsp = new RESPONSE_LOADCATGEORY();

			using(var dbc = new AppDbContext())
			{
				var repo = new CategoryRepository(dbc);
				long loadCategoryId = 3;
				if (reqparam.TargetCategortId != 0) loadCategoryId = reqparam.TargetCategortId;

				var appliction = repo.Load(loadCategoryId);
				foreach(var child in appliction.ChildCategories)
				{
					rsp.Categories.Add(new DataCategory
					{
						Id = child.Id,
						Name = child.Name,
						CategoryTypeCode = child.CategoryTypeCode,
						IsHasChild = child.ChildCategories.Count > 0 ? true : false
					});
				}
			}

			rsp.Success = true;
			return rsp;
		}

		public RESPONSE_LOADDEVICESETTING LoadDeviceSetting()
		{
			var rsp = new RESPONSE_LOADDEVICESETTING();

			using (var dbc = new AppDbContext())
			{
				

			}

			return rsp;
		}

		public RESPONSE_LOADTAG LoadTag(REQUEST_LOADTAG reqparam)
		{
			var rsp = new RESPONSE_LOADTAG();

			using (var dbc = new AppDbContext())
			{
				var repo = new TagRepository(dbc);

				long loadTagId = reqparam.TargetTagId;

				var application = repo.Load(loadTagId);
				foreach(var child in application.ChildTag)
				{
					rsp.Tags.Add(new DataTag
					{
						Id=child.Id,
						Name=child.Name,
						IsHasChild = child.ChildTag.Count > 0 ? true : false
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

		public RESPONSE_SAVEDEVICESETTING SaveDeviceSetting(REQUEST_SAVEDEVICRSETTING reqparam)
		{
			var rsp = new RESPONSE_SAVEDEVICESETTING();

			return rsp;
		}

		public RESPONSE_UPDATECATEGORY UpdateCategory(REQUEST_UPDATECATEGORY reqparam)
		{
			var rsp = new RESPONSE_UPDATECATEGORY();
			return rsp;
		}

		#endregion メソッド

	}
}

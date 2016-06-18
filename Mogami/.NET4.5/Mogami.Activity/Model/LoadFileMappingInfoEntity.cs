using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using Mogami.Model;
using Mogami.Model.Repository;
using Mogami.Core.Infrastructure;
using log4net;
using Mogami.Contrib.Akalib;
using System.ComponentModel;

namespace Mogami.Activity.Model
{

	public sealed class LoadFileMappingInfoEntity : CodeActivity<FileMappingInfo>
	{


		#region フィールド

		private static ILog LOG = LogManager.GetLogger(typeof(LoadFileMappingInfoEntity));

		#endregion フィールド

		#region プロパティ

		/// <summary>
		/// KeyValueの値が、どのような値か設定します。
		/// </summary>
		/// <remarks><pre>
		/// ・Id
		/// ・AclHash
		/// ・MappingFilePath
		/// </pre></remarks>
		[RequiredArgument]
		public InArgument<string> KeyType { get; set; }
		[RequiredArgument]
		public InArgument<string> KeyValue { get; set; }

		/// <summary>
		/// ParameterResultKeyを指定した場合のパラメータスタック
		/// </summary>
		[Category("Employee Control")]
		[OverloadGroup("ParameterStack")]
		public InArgument<ParameterStack> Parameter { get; set; }

		/// <summary>
		/// 戻り値を、ParameterStackに格納する場合の格納先のキーです
		/// </summary>
		[Category("Employee Control")]
		[OverloadGroup("ParameterStack")]
		public InArgument<string> ParameterResultKey { get; set; }

		#endregion プロパティ

		#region メソッド

		// アクティビティが値を返す場合は、CodeActivity<TResult> から派生して、
		// Execute メソッドから値を返します。
		protected override FileMappingInfo Execute(CodeActivityContext context)
		{
			IWorkflowContext workflowContext = context.GetExtension<IWorkflowContext>();
			var repo = new FileMappingInfoRepository(workflowContext.DbContext);

			string keyType = this.KeyType.Get(context);
			string keyValue = this.KeyValue.Get(context);

			FileMappingInfo returnEntity = null;
			switch (keyType)
			{
				case "Id":
					returnEntity = repo.Load(long.Parse(keyValue));
					if (returnEntity == null)
						LOG.WarnFormat("FileMappingInfoの読み込みに失敗しました。 Id={0}", long.Parse(keyValue));
					break;
				case "AclHash":
					returnEntity = repo.LoadByAclHash(keyValue);
					if (returnEntity == null)
						LOG.WarnFormat("FileMappingInfoの読み込みに失敗しました。 AclHash={0}", keyValue);
					break;
				case "MappingFilePath":
					returnEntity = repo.LoadByPath(keyValue);
					if (returnEntity == null)
						LOG.WarnFormat("FileMappingInfoの読み込みに失敗しました。 MappingFilePath={0}", keyValue);
					break;
				default:
					throw new ApplicationException("キータイプの指定が不正です");
			}

			var resultKey = this.ParameterResultKey.Get(context);
			if (resultKey != null)
			{
				ParameterStack pstack = context.GetValue<ParameterStack>(this.Parameter);
				pstack.SetValue(resultKey, returnEntity);
			}

			return returnEntity;
		}

		#endregion メソッド

	}
}

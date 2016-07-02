using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using log4net;
using Mogami.Contrib.Akalib;
using Mogami.Core.Infrastructure;
using Mogami.Model;
using Mogami.Core.Definication;
using Mogami.Model.Repository;

namespace Mogami.Activity.Feature.Category
{

	public sealed class MakeCategory : CodeActivity
	{


		#region フィールド

		static ILog LOG = LogManager.GetLogger(typeof(MakeCategory));

		bool _IsCreate = false;

		#endregion フィールド


		#region プロパティ

		public OutArgument<bool> AlreadyCategoryFlag { get; set; }

		[RequiredArgument]
		public InArgument<ParameterStack> Parameter { get; set; }

		#endregion プロパティ


		#region メソッド

		protected override void Execute(CodeActivityContext context)
		{
			IWorkflowContext workflowContext = context.GetExtension<IWorkflowContext>();
			ParameterStack pstack = context.GetValue<ParameterStack>(this.Parameter);

			string categoryPath = null;

			var target = pstack.GetValue<FileMappingInfo>(ActivityParameterStack.TARGET);
			if (target != null)
			{
				categoryPath = target.MappingFilePath;
			}
			else
			{
				categoryPath = pstack.GetValue<string>(ActivityParameterStack.MAKECATEGORY_CURRENT_CATEGORY);
			}

			if (string.IsNullOrEmpty(categoryPath)) throw new ApplicationException("有効なカテゴリパス文字列ではありません");

			var catrepo = new CategoryRepository(workflowContext.DbContext);
			var appcat = catrepo.Load(3L);
			if (appcat == null) throw new ApplicationException();
			_IsCreate = false;

			Mogami.Model.Category targetCategory = appcat;
			var tokens = target.MappingFilePath.Split(new string[] { @"\" }, StringSplitOptions.None);
			var sttokens = new Stack<string>(tokens);
			var title = sttokens.Pop();
			var qutokens = new Queue<string>(sttokens.Reverse<string>());
			while (qutokens.Count > 0)
			{
				var oneText = qutokens.Dequeue();
				targetCategory = CreateOrSelectCategory(targetCategory, oneText, catrepo);
			}

			workflowContext.DbContext.SaveChanges();

			pstack.SetValue(ActivityParameterStack.CATEGORY, targetCategory);

			this.AlreadyCategoryFlag.Set(context, !_IsCreate);
		}

		private Mogami.Model.Category CreateOrSelectCategory(Mogami.Model.Category parentCategory, string categoryName, CategoryRepository repo)
		{
			Mogami.Model.Category category = null;
			foreach (var child in parentCategory.ChildCategories)
			{
				if (child.Name == categoryName)
				{
					category = child;
					break;
				}
			}

			// Create
			if (category == null)
			{
				category = new Mogami.Model.Category()
				{
					Name = categoryName,
					CategoryTypeCode = Core.Constructions.CategoryType.APPLICATION,
					ParentCategory = parentCategory
				};
				repo.Add(category);
				_IsCreate = true;
			}

			return category;
		}

		#endregion メソッド
	}
}

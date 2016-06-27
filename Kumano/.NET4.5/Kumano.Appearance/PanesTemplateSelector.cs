using Kumano.Data.ViewModel;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Xceed.Wpf.AvalonDock.Layout;

namespace Kumano.View
{
	internal class PanesTemplateSelector : DataTemplateSelector
	{

		#region フィールド

		static ILog LOG = LogManager.GetLogger(typeof(PanesTemplateSelector));
		static Dictionary<Type, PropertyInfo> PaneViewModelDataTemplate = new Dictionary<Type, PropertyInfo>();

		#endregion フィールド


		#region コンストラクタ

		static PanesTemplateSelector()
		{
			foreach (var propinfo in typeof(PanesTemplateSelector).GetProperties())
			{
				var attrs = propinfo.GetCustomAttributes(typeof(DataTemplateSelectMarkAttribute), false);
				if (attrs.Length == 0) continue;

				var tgtAttr = attrs[0] as DataTemplateSelectMarkAttribute;
				if (tgtAttr == null) return;

				PaneViewModelDataTemplate.Add(tgtAttr.PaneViewModel, propinfo);
			}
		}

		#endregion コンストラクタ


		#region プロパティ

		[DataTemplateSelectMark(typeof(CategoryTreeExplorerPaneViewModel))]
		public DataTemplate CategoryTreeExplorerPaneTemplate { get; set; }

		//[DataTemplateSelectMark(typeof(PropertyPaneViewModel))]
		public DataTemplate PropertyPaneTemplate { get; set; }

		#endregion プロパティ


		#region メソッド

		public override System.Windows.DataTemplate SelectTemplate(object item, System.Windows.DependencyObject container)
		{
			if (item != null)
			{
				var itemAsLayoutContent = item as LayoutContent;
				if (!PaneViewModelDataTemplate.ContainsKey(item.GetType()))
				{
					LOG.Warn(item.GetType() + "を表示できるDataTemplateがありません。");
					return base.SelectTemplate(item, container);
				}

				var propinfo = PaneViewModelDataTemplate[item.GetType()];
				if (propinfo != null)
				{
					return propinfo.GetValue(this, null) as DataTemplate;
				}
			}

			return base.SelectTemplate(item, container);
		}

		#endregion メソッド


		#region Classes

		[AttributeUsage(AttributeTargets.Property)]
		internal class DataTemplateSelectMarkAttribute : Attribute
		{

			#region コンストラクタ

			public DataTemplateSelectMarkAttribute(Type paneViewModel)
			{
				this.PaneViewModel = paneViewModel;
			}

			#endregion コンストラクタ


			#region プロパティ

			public Type PaneViewModel { get; set; }

			#endregion プロパティ
		}

		#endregion Classes
	}
}

using Akalib.Wpf.Control.Tree;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interactivity;

namespace Kumano.View.Behavior
{
	/// <summary>
	/// TreeListクラスの標準ではバインドできないプロパティを表します
	/// </summary>
	public enum TreeListUnbindableCanReadProperty
	{
		SelectedNodes,
		SelectedNode
	}

	public class TreeListSetStateToSourceAction : TriggerAction<TreeList>
	{

		#region フィールド

		public static readonly DependencyProperty PropertyProperty =
					DependencyProperty.Register("Property", typeof(TreeListUnbindableCanReadProperty), typeof(TreeListSetStateToSourceAction), new PropertyMetadata());

		public static readonly DependencyProperty SourceProperty =
					DependencyProperty.Register("Source", typeof(object), typeof(TreeListSetStateToSourceAction), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

		#endregion フィールド

		#region プロパティ

		/// <summary>
		/// 値を設定したいプロパティを取得または設定します。
		/// </summary>
		/// <remarks>
		/// TagListのSelectionChangeイベントをトリガーとし、下記サンプルのように呼び出すことができます。
		/// </remarks>
		/// <example>
		///  <i:Interaction.Triggers>
		///     <i:EventTrigger EventName="SelectionChanged">
		///         <l:LivetCallMethodAction MethodName="Outputlog" MethodTarget="{Binding}" />
		///         <localBehavior:TreeListSetStateToSourceAction Source="{Binding SelectedTreeListNode}" Property="SelectedNode" />
		///         <localBehavior:TreeListSetStateToSourceAction Source="{Binding SelectedTreeListNodes}" Property="SelectedNodes" />
		///     </i:EventTrigger>
		/// </i:Interaction.Triggers>
		/// </example>
		public TreeListUnbindableCanReadProperty Property
		{
			get { return (TreeListUnbindableCanReadProperty)GetValue(PropertyProperty); }
			set { SetValue(PropertyProperty, value); }
		}

		/// <summary>
		/// Propertyプロパティで指定されたプロパティから値が設定されるソースを取得または設定します。
		/// </summary>
		[Bindable(BindableSupport.Default, BindingDirection.TwoWay)]
		public object Source
		{
			get { return (object)GetValue(SourceProperty); }
			set { SetValue(SourceProperty, value); }
		}

		#endregion プロパティ


		#region メソッド

		protected override void Invoke(object parameter)
		{
			switch (Property)
			{
				case TreeListUnbindableCanReadProperty.SelectedNode:
					if ((TreeNode)Source != AssociatedObject.SelectedNode)
					{
						Source = AssociatedObject.SelectedNode;
					}
					break;
				case TreeListUnbindableCanReadProperty.SelectedNodes:
					if ((System.Collections.ICollection)Source != AssociatedObject.SelectedNodes)
					{
						Source = AssociatedObject.SelectedNodes;
					}
					break;
			}
		}

		#endregion メソッド
	}
}

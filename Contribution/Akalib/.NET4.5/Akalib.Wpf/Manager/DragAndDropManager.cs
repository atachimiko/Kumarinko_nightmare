using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Documents;
using System.Diagnostics.Contracts;
using System.Collections;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Akalib.Wpf.Manager
{
	public class DragAdorner : Adorner
	{


		#region フィールド

		protected UIElement _Child;

		protected UIElement _Owner;

		protected double XCenter;

		protected double YCenter;

		
		private double _LeftOffset;

		
		private double _TopOffset;

		#endregion フィールド

		#region コンストラクタ

		/// <summary>
		/// 
		/// </summary>
		/// <param name="owner"></param>
		/// <param name="adornElement"></param>
		public DragAdorner(UIElement owner, UIElement adornElement)
			: base(adornElement)
		{
			_Owner = owner;

			Brush brush = new VisualBrush(adornElement);

			var rectangle = new Rectangle
			{
				Width = adornElement.RenderSize.Width,
				Height = adornElement.RenderSize.Height,
				Fill = brush,
				Opacity = 0.7
			};

			Point mousePoint = Mouse.PrimaryDevice.GetPosition(owner);
			XCenter = mousePoint.X;
			YCenter = mousePoint.Y;

			_Child = rectangle;
		}

		#endregion コンストラクタ

		//=====================================================================

		//=====================================================================


		#region プロパティ

		public double LeftOffset
		{
			get
			{
				return _LeftOffset;
			}
			set
			{
				_LeftOffset = value - XCenter;
				UpdatePosition();
			}
		}

		public double TopOffset
		{
			get
			{
				return _TopOffset;
			}
			set
			{
				_TopOffset = value - YCenter;
				UpdatePosition();
			}
		}

		protected override int VisualChildrenCount
		{
			get
			{
				return 1;
			}
		}

		#endregion プロパティ


		#region メソッド

		public override GeneralTransform GetDesiredTransform(GeneralTransform transform)
		{
			var result = new GeneralTransformGroup();
			result.Children.Add(base.GetDesiredTransform(transform));
			result.Children.Add(new TranslateTransform(_LeftOffset, _TopOffset));

			return result;
		}

		/// <summary>
		/// Adornerの表示座標
		/// </summary>
		/// <param name="left"></param>
		/// <param name="top"></param>
		public void SetPosition(double left, double top)
		{
			TopOffset = top;
			LeftOffset = left;

			UpdatePosition();
		}

		//=====================================================================

		//=====================================================================
		//=====================================================================

		//=====================================================================

		protected override Size ArrangeOverride(Size finalSize)
		{
			_Child.Arrange(new Rect(finalSize));
			return finalSize;
		}

		//=====================================================================
		protected override Visual GetVisualChild(int index)
		{
			return this._Child;
		}

		//=====================================================================
		protected override Size MeasureOverride(Size finalSize)
		{
			_Child.Measure(finalSize);
			return _Child.DesiredSize;
		}

		private void UpdatePosition()
		{
			var adornerLayer = (AdornerLayer)Parent;
			if (adornerLayer != null)
				adornerLayer.Update(AdornedElement);
		}

		#endregion メソッド

	}

	/// <summary>
	/// 
	/// </summary>
	public class DragAndDropManager
	{
		//=====================================================================

		//=====================================================================

		#region フィールド

		/// <summary>
		/// 
		/// </summary>
		/// <remarks>
		/// 
		/// </remarks>
		public static readonly DependencyProperty EndDragAndDropCallbackProperty =
			DependencyProperty.RegisterAttached("EndDragAndDropCallback",
			typeof(Func<DragEventArgs, bool>),
			typeof(DragAndDropManager)
			);

		public static readonly DependencyProperty IsAllowDragProperty =
			DependencyProperty.RegisterAttached("IsAllowDrag",
			typeof(bool),
			typeof(DragAndDropManager),
			new UIPropertyMetadata(false, IsAllowDragChanged));

		public static readonly DependencyProperty IsAllowDropProperty =
			DependencyProperty.RegisterAttached("IsAllowDrop",
			typeof(bool),
			typeof(DragAndDropManager),
			new UIPropertyMetadata(false, IsAllowDropChanged));

		public static readonly DependencyProperty TargetDragDropCallbackProperty =
			DependencyProperty.RegisterAttached("TargetDragDropCallback",
			typeof(Func<DragEventArgs, bool>),
			typeof(DragAndDropManager)
			);

		/// <summary>
		/// ドラッグ先のDragOverイベントで呼び出されるコールバック関数
		/// </summary>
		/// <remarks>
		/// プロパティに有効なコールバック関数が設定された場合、DragOverイベント内で呼び出されます。
		/// コールバック関数がTrueを返す場合は、ドロップ可能とし、
		/// Falseを返す場合は、ドロップ不可能とします。
		/// </remarks>
		public static readonly DependencyProperty TargetDragOverCallbackProperty =
			DependencyProperty.RegisterAttached("TargetDragOverCallback",
			typeof(Func<DragEventArgs, bool>),
			typeof(DragAndDropManager)
			);

		private static DragAndDropManager instance;
		//=====================================================================
		TreeItemScroll _TreeItemScroll = null;

		//=====================================================================
		/// <summary>
		/// D&Dしているデータの形式
		/// </summary>
		/// <remarks>
		/// デフォルトでは独自の形式名(DragDropItemsControl
		/// </remarks>
		DataFormat dataFormat = DataFormats.GetDataFormat("DragDropItemsControl");

		//=====================================================================
		/// <summary>
		/// ドラッグ中の状態を表示するためのAdorner
		/// </summary>
		DragAdorner dragAdorner;

		/// <summary>
		/// ドラッグ中のデータ
		/// </summary>
		object dragData;

		/// <summary>
		/// ドラッグを開始したPanel
		/// </summary>
		FrameworkElement dragSourcePanel;

		/// <summary>
		/// ドラッグ先のUIElement
		/// </summary>
		UIElement dragTargetElement;

		/// <summary>
		/// 挿入位置マーカを描画したAdorner
		/// </summary>
		InsertionAdorner insertionAdorner;

		/// <summary>
		/// 挿入マーカの表示位置(ListBox内のアイテムのインデックス値)
		/// </summary>
		int insertionIndex;

		/// <summary>
		/// 挿入マーカの表示位置(0番目に表示する場合はTrue)
		/// </summary>
		bool isInFirstHalf;

		bool isTreeNodeSeparation = true;

		/// <summary>
		/// ドラッグ先がItemsControlである場合の
		/// </summary>
		FrameworkElement itemContainerInnerItemsControl;

		/// <summary>
		/// 
		/// </summary>
		Point LastMousePos = new Point(0, 0);

		/// <summary>
		/// 
		/// </summary>
		Point LastScrollOffsetPos;

		/// <summary>
		/// ルートウィンドウ
		/// </summary>
		//Window rootWindow;
		FrameworkElement rootWindow;

		//=====================================================================
		/// <summary>
		/// ドラッグを開始した地点を示すマウス座標
		/// </summary>
		Point startPoint;

		#endregion フィールド

		#region コンストラクタ

		//=====================================================================
		/// <summary>
		/// singletonクラスのため、コンストラクタを使用してインスタンスを作成することはできません。
		/// </summary>
		private DragAndDropManager()
		{
		}

		#endregion コンストラクタ


		#region プロパティ

		public static DragAndDropManager Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new DragAndDropManager();
				}
				return instance;
			}
		}

		#endregion プロパティ

		//=====================================================================
		//=====================================================================

		//=====================================================================

		//=====================================================================


		#region メソッド

		public static Func<DragEventArgs, bool> GetEndDragAndDropCallback(DependencyObject obj)
		{
			return (Func<DragEventArgs, bool>)obj.GetValue(EndDragAndDropCallbackProperty);
		}

		public static bool GetIsAllowDrag(DependencyObject obj)
		{
			return (bool)obj.GetValue(IsAllowDragProperty);
		}

		public static bool GetIsAllowDrop(DependencyObject obj)
		{
			return (bool)obj.GetValue(IsAllowDropProperty);
		}

		public static Func<DragEventArgs, bool> GetTargetDragDropCallback(DependencyObject obj)
		{
			return (Func<DragEventArgs, bool>)obj.GetValue(TargetDragDropCallbackProperty);
		}

		//=====================================================================
		public static Func<DragEventArgs, bool> GetTargetDragOverCallback(DependencyObject obj)
		{
			return (Func<DragEventArgs, bool>)obj.GetValue(TargetDragOverCallbackProperty);
		}
		public static bool IsInFirstHalf(FrameworkElement container, Point clickedPoint, bool hasVerticalOrientation)
		{
			if (hasVerticalOrientation)
			{
				return clickedPoint.Y < container.ActualHeight / 2;
			}
			return clickedPoint.X < container.ActualWidth / 2;
		}

		public static bool IsInnerNoder(FrameworkElement container, Point clickedPoint, bool hasVerticalOrientation)
		{
			if (hasVerticalOrientation)
			{
				double d = container.ActualHeight / 3;
				if (d < clickedPoint.Y && d * 2.0 > clickedPoint.Y)
					return true;
				return false;
			}

			double f = container.ActualWidth / 3;
			if (f < clickedPoint.X && f * 2.0 > clickedPoint.X)
				return true;
			return false;
		}

		public static void SetEndDragAndDropCallback(DependencyObject obj, Func<DragEventArgs, bool> value)
		{
			obj.SetValue(EndDragAndDropCallbackProperty, value);
		}

		public static void SetIsAllowDrag(DependencyObject obj, bool value)
		{
			obj.SetValue(IsAllowDragProperty, value);
		}

		public static void SetIsAllowDrop(DependencyObject obj, bool value)
		{
			obj.SetValue(IsAllowDropProperty, value);
		}

		public static void SetTargetDragDropCallback(DependencyObject obj, Func<DragEventArgs, bool> value)
		{
			obj.SetValue(TargetDragDropCallbackProperty, value);
		}

		public static void SetTargetDragOverCallback(DependencyObject obj, Func<DragEventArgs, bool> value)
		{
			obj.SetValue(TargetDragDropCallbackProperty, value);
		}
		private static void IsAllowDragChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			UIElement dragSourcePanel = sender as UIElement;
			Contract.Requires(dragSourcePanel != null, "ドラッグソースを設定できるのはPanel型のみです");

			if (Equals(e.NewValue, true))
			{
				dragSourcePanel.PreviewMouseLeftButtonDown += Instance.OnDragSource_PreviewMouseLeftButtonDown;
				dragSourcePanel.PreviewMouseLeftButtonUp += Instance.OnDragSource_PreviewMouseLeftButtonUp;
				dragSourcePanel.PreviewMouseMove += Instance.OnDragSource_PreviewMouseMove;
			}
			else
			{
				dragSourcePanel.PreviewMouseLeftButtonDown -= Instance.OnDragSource_PreviewMouseLeftButtonDown;
				dragSourcePanel.PreviewMouseLeftButtonUp -= Instance.OnDragSource_PreviewMouseLeftButtonUp;
				dragSourcePanel.PreviewMouseMove -= Instance.OnDragSource_PreviewMouseMove;
			}
		}
		private static void IsAllowDropChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			UIElement dragTargetPanel = sender as UIElement;
			Contract.Requires(dragTargetPanel != null, "ドラッグターゲットを設定できるのはPanel型のみです");

			if (Equals(e.NewValue, true))
			{
				dragTargetPanel.AllowDrop = true;

				//dragTargetPanel.PreviewDragEnter += Instance.OnDragTarget_PreviewDragEnter;
				//dragTargetPanel.PreviewDragOver += Instance.OnDragTarget_PreviewDragOver;
				//dragTargetPanel.PreviewDragLeave += Instance.OnDragTarget_PreviewDragLeave;
				//dragTargetPanel.PreviewDrop += Instance.OnDragTarget_PreviewDrop;

				dragTargetPanel.DragEnter += Instance.OnDragTarget_PreviewDragEnter;
				dragTargetPanel.DragOver += Instance.OnDragTarget_PreviewDragOver;
				dragTargetPanel.DragLeave += Instance.OnDragTarget_PreviewDragLeave;
				dragTargetPanel.Drop += Instance.OnDragTarget_PreviewDrop;
			}
			else
			{
				dragTargetPanel.AllowDrop = false;

				//dragTargetPanel.PreviewDragEnter -= Instance.OnDragTarget_PreviewDragEnter;
				//dragTargetPanel.PreviewDragOver -= Instance.OnDragTarget_PreviewDragOver;
				//dragTargetPanel.PreviewDragLeave -= Instance.OnDragTarget_PreviewDragLeave;
				//dragTargetPanel.PreviewDrop -= Instance.OnDragTarget_PreviewDrop;

				dragTargetPanel.DragEnter -= Instance.OnDragTarget_PreviewDragEnter;
				dragTargetPanel.DragOver -= Instance.OnDragTarget_PreviewDragOver;
				dragTargetPanel.DragLeave -= Instance.OnDragTarget_PreviewDragLeave;
				dragTargetPanel.Drop -= Instance.OnDragTarget_PreviewDrop;
			}
		}
		//=====================================================================

		//=====================================================================

		/// <summary>
		/// 挿入位置マーカが表示されたAdornerを作成する
		/// </summary>
		private void CreateInsertionAdorner()
		{
			if (this.itemContainerInnerItemsControl != null)
			{
				var adornerLayer = AdornerLayer.
					GetAdornerLayer(this.dragSourcePanel);

				insertionAdorner = new InsertionAdorner(this.isTreeNodeSeparation,
					this.isInFirstHalf,
					this.itemContainerInnerItemsControl,
					adornerLayer);
			}
		}

		/// <summary>
		/// ドロップ先によってD&Dの動作挙動を決定するためのルーチン
		/// </summary>
		/// <remarks>
		/// ドロップ先のイベント(DragEnterやDragOver)で、D&Dに関する処理を行う前にこの関数を呼び出してください。
		/// </remarks>
		/// <param name="e">DragEnterイベントやDragOverイベントの引数をそのまま与えてください</param>
		private void DecideDropTarget(DragEventArgs e)
		{
			if (dragTargetElement is ItemsControl) // ドラッグ先がListBoxの場合
			{
				var dtgItemsControl = this.dragTargetElement as ItemsControl;

				int itemNum = dtgItemsControl.Items.Count;
				if (itemNum > 0)
				{
					int itemPos = 0;

					// マウス座標の下にあるコントロールを取得する
					if (dtgItemsControl is TreeView)
					{
						// TreeNodeの時にTreeViewItemを再帰的に遡って、ドロップ先のTreeViewItemとその位置を取得する
						this.itemContainerInnerItemsControl = GetCetontainerFromElement(
							(TreeView)dtgItemsControl,
							(DependencyObject)e.OriginalSource,
							out itemPos);
					}
					else
					{
						this.itemContainerInnerItemsControl = dtgItemsControl.ContainerFromElement(
							(DependencyObject)e.OriginalSource) as FrameworkElement;
					}


					if (itemContainerInnerItemsControl != null)
					{
						Point positionRelativeToItemContainer = e.GetPosition(this.itemContainerInnerItemsControl);
						this.insertionIndex = dtgItemsControl.ItemContainerGenerator.IndexFromContainer(this.itemContainerInnerItemsControl);
						this.isInFirstHalf = IsInFirstHalf(itemContainerInnerItemsControl, positionRelativeToItemContainer, true);
						if (dtgItemsControl is TreeView)
						{
							var @flag = IsInnerNoder(itemContainerInnerItemsControl, positionRelativeToItemContainer, true);
							this.isTreeNodeSeparation = !@flag; // 符号を反転させる必要があります
							e.Data.SetData("DropInNode", @flag); // DropBeforePositionDataデータが示すノード内にD&Dするかどうか？
						}
						else
						{
							this.isTreeNodeSeparation = true;
						}

						if (this.itemContainerInnerItemsControl is TreeViewItem)
						{
							var tvi = this.itemContainerInnerItemsControl as TreeViewItem;

							if (itemPos != -1)
								this.insertionIndex = itemPos; // 変数値の上書き（子ノードの場合のみ)

							// ドラッグ中のマウスカーソルの下にあるTreeViewItemのデータを
							// ターゲットに渡す
							dynamic data = tvi.DataContext;
							e.Data.SetData("DropBeforePositionData", data);

							//Console.WriteLine("\t DataContext = {0}", ((dynamic)tvi.DataContext).NodeName);
						}

						if (!this.isInFirstHalf)
						{
							this.insertionIndex++;
						}
					}
					else
					{
						// ----------------------------
						// 最後の項目を選択する
						// ただし、ドロップ先がTreeViewの場合は動作が不自然なのでこの処理は行わない。
						if (!(dtgItemsControl is TreeView))
						{
							itemContainerInnerItemsControl = dtgItemsControl.ItemContainerGenerator.ContainerFromIndex(itemNum - 1) as FrameworkElement;
							this.isInFirstHalf = false;
							this.insertionIndex = itemNum;
						}
					}
				}
				else
				{
					this.itemContainerInnerItemsControl = null;
					this.insertionIndex = 0;
				}

				// 挿入位置をターゲットに渡す
				// ターゲットがTreeViewの場合は注意が必要です。
				// DropBeforePositionDataと同じ階層に属しており、下記の位置がドロップ先の新しい位置になります。
				e.Data.SetData(this.insertionIndex);

				return; // ドラッグ先がListBox時の処理 終了。
			}

			// Default
			this.itemContainerInnerItemsControl = null;
			this.insertionIndex = -1;
		}

		//=====================================================================
		private TreeViewItem GetCetontainerFromElement(TreeView treeView, DependencyObject buttomControl, out int itemPos)
		{
			// Dump
			ItemsControl lastBefore = null;
			ItemsControl last = null;
			int i = 1;
			foreach (var @d in GetGCetontainerFromElementList(treeView, buttomControl))
			{
				dynamic data = @d.DataContext;
				//Console.WriteLine("[{1}]  DataContext = {0}",data.NodeName,i);

				last = lastBefore;
				lastBefore = @d;

				i++;
			}

			if (lastBefore != null && last != null)
			{
				//Console.WriteLine("[Lastbefore]  DataContext = {0}", ((dynamic)lastBefore.DataContext).NodeName);
				//Console.WriteLine("[Last]  DataContext = {0}", ((dynamic)last.DataContext).NodeName);

				var @pos = last.ItemContainerGenerator.IndexFromContainer(lastBefore);
				//Console.WriteLine("\t場所 = {0}", @pos);
				itemPos = @pos;
			}
			else
			{
				/*
				if (lastBefore == null)
					Console.WriteLine("\tlastBeforeがありません");
				if(last == null)
					Console.WriteLine("\tlastがありません");
				*/
				itemPos = -1;
			}

			// --
			return GetGCetontainerFromElementList(treeView, buttomControl).LastOrDefault() as TreeViewItem;
		}

		//=====================================================================
		/// <summary>
		/// 再帰的にItemsControlのコンテナをさかのぼって行くための再帰呼び出し関数
		/// </summary>
		/// <param name="topItemsControl"></param>
		/// <param name="buttomControl"></param>
		/// <returns></returns>
		private IEnumerable<ItemsControl> GetGCetontainerFromElementList(ItemsControl topItemsControl, DependencyObject buttomControl)
		{
			ItemsControl tmp = topItemsControl;
			while (tmp != null)
			{
				tmp = ItemsControl.ContainerFromElement(tmp, buttomControl) as ItemsControl;
				if (tmp != null)
				{
					yield return tmp;
				}
			}
		}

		/// <summary>
		/// ドラッグソース内でマウスの左クリックを受けた場合の実行関数
		/// </summary>
		/// <remarks>
		/// D&Dの開始地点となります。
		/// </remarks>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnDragSource_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			// ドラッグ元はPanelのみ。
			//if (!(sender is Panel)) throw new NotSupportedException();
			if (!(sender is FrameworkElement)) throw new NotSupportedException();

			// ドラッグするデータは、XAMLからの設定方法がないので、
			// UIElementが持つプロパティ「DataContext」をそのままドラッグデータとして設定しています。

			//this.dragSourcePanel = sender as Panel;
			this.dragSourcePanel = sender as FrameworkElement;

			// ドラッグデータは、ドラッグソースのDataContextをそのまま扱う。
			if (this.dragSourcePanel.DataContext is ICloneable)
				this.dragData = ((ICloneable)this.dragSourcePanel.DataContext).Clone();
			else
				this.dragData = this.dragSourcePanel.DataContext; // ドラッグデータは、ドラッグソースのDataContextをそのまま扱う

			this.rootWindow = Window.GetWindow(dragSourcePanel);
			this.startPoint = e.GetPosition(rootWindow);

			if (this._TreeItemScroll != null)
			{
				this.LastScrollOffsetPos = new Point(0, 0);
				this._TreeItemScroll.Stop();
				this._TreeItemScroll = null;
			}
		}

		/// <summary>
		/// ドラッグソース内でマウスの左クリックが解除された場合の実行関数
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnDragSource_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			// D&D処理を中止
			dragData = null;

			if (this._TreeItemScroll != null)
			{
				this.LastScrollOffsetPos = new Point(0, 0);
				this._TreeItemScroll.Stop();
				this._TreeItemScroll = null;
			}
		}


		/// <summary>
		/// ドラッグソース内でのマウス移動の実行関数
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnDragSource_PreviewMouseMove(object sender, MouseEventArgs e)
		{
			if (dragData != null)
			{
				Panel dragTargetPanel = sender as Panel;

				// D&Dのデータ
				DataObject dddata = new DataObject(this.dataFormat.Name, this.dragData);

				bool oldAllowDrop = this.rootWindow.AllowDrop;
				rootWindow.AllowDrop = true;
				rootWindow.DragEnter += OnRootWindow_DragEnter;
				rootWindow.DragOver += OnRootWindow_DragOver;
				rootWindow.DragLeave += OnRootWindow_DragLeave;
				rootWindow.PreviewMouseLeftButtonUp += OnRootWindow_PreviewMouseLeftButtonUp;

				// 下記で、Drop処理が行われるまで、
				// 処理がブロックされる。
				DragDropEffects effects = DragDrop.DoDragDrop((DependencyObject)sender, dddata, DragDropEffects.Move);



				//----------------
				// D&D終了の処理
				RemoveDragAdorner();

				rootWindow.AllowDrop = oldAllowDrop;
				rootWindow.DragEnter -= OnRootWindow_DragEnter;
				rootWindow.DragOver -= OnRootWindow_DragOver;
				rootWindow.DragLeave -= OnRootWindow_DragLeave;
				rootWindow.PreviewMouseLeftButtonUp -= OnRootWindow_PreviewMouseLeftButtonUp;

				this.dragData = null;

				// [ドラッグアンドドロップの終了を通知]
				if (dragTargetElement != null)
				{
					var @func = GetEndDragAndDropCallback(dragTargetElement);
					if (@func != null)
						@func(null);
				}
			}
		}


		/// <summary>
		/// ドロップ先に指定したUIElementにマウスカーソルが入った場合の実行関数
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnDragTarget_PreviewDragEnter(object sender, DragEventArgs e)
		{
			this.dragTargetElement = sender as UIElement;


			object draggedItem = e.Data.GetData(this.dataFormat.Name);
			if (draggedItem != null)
			{
				if (dragTargetElement is TreeView)
				{
					if (_TreeItemScroll == null)
					{
						_TreeItemScroll = new TreeItemScroll(this.rootWindow, (TreeView)dragTargetElement, (offsetPos) =>
						{
							LastScrollOffsetPos = offsetPos;
							var ScrollingPos = new Point(LastMousePos.X, LastMousePos.Y - offsetPos.Y);
							ShowDragAdorner(ScrollingPos);
						});

						_TreeItemScroll.Start();
					}
				}


				DecideDropTarget(e);

				// ドロップ先がItemsControl(ListBoxなど)のコントロールの場合、
				// 挿入先を示すマーカをAdornerとして表示する
				if (sender is ItemsControl)
				{
					CreateInsertionAdorner();
				}
			}
			e.Handled = true;
		}

		/// <summary>
		/// ドロップ先に指定したUIElementからマウスカーソルが離れる場合の実行関数
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnDragTarget_PreviewDragLeave(object sender, DragEventArgs e)
		{
			object draggedItem = e.Data.GetData(this.dataFormat.Name);

			if (draggedItem != null)
			{
				var ui = sender as FrameworkElement;

				// ドロップ先がItemsControl(ListBoxなど)のコントロールの場合、
				// 挿入先を示すマーカを表示したAdornerを削除する
				if (sender is ItemsControl)
				{
					RemoveInsertionAdorner();
				}
			}
			e.Handled = true;
		}

		/// <summary>
		/// ドロップ先に指定したUIElementにマウスカーソルがある場合の実行関数
		/// </summary>
		/// <remarks>
		/// D&Dの内容を描画したAdornerを出力する。
		/// </remarks>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnDragTarget_PreviewDragOver(object sender, DragEventArgs e)
		{
			object draggedItem = e.Data.GetData(this.dataFormat.Name);
			if (draggedItem != null)
			{
				var panel = sender as FrameworkElement;

				Point p = e.GetPosition(rootWindow);
				//Console.WriteLine("Point = {0}", p);

				if (panel is TreeView)
				{
					if (_TreeItemScroll != null)
					{
						_TreeItemScroll.Execute(p);
					}
				}

				// コールバック関数が設定されている場合、コールバック関数の呼び出しを行う
				var f = GetTargetDragOverCallback(dragTargetElement);
				if (f != null)
				{
					bool result = f.Invoke(e);
					if (!result)
					{
						RemoveDragAdorner();
						e.Effects = DragDropEffects.None; // マウスカーソルの形状をドロップ不可能に。
						e.Handled = true;
						return;
					}
				}

				LastMousePos = p;

				// Adornerの出力
				ShowDragAdorner(new Point
				{
					X = p.X - LastScrollOffsetPos.X,
					Y = p.Y - LastScrollOffsetPos.Y
				});

				// ドロップ先がItemsControl(ListBoxなど)のコントロールの場合、
				// 挿入先を示すマーカをAdornerとして表示する
				if (sender is ItemsControl)
				{
					if (this.itemContainerInnerItemsControl == null)
					{
						RemoveDragAdorner();
						e.Effects = DragDropEffects.None; // マウスカーソルの形状をドロップ不可能に。
					}
					else
					{

						// ターゲットがTreeViewの場合、D&D中もマウスの場所によって挙動を変化させなければなりません。
						// ターゲット内のドロップ先が、TreeView内の階層構造を持つTreeViewItemになるためです。
						// ここでは、そのTreeViewItemの下階層へアイテムをドロップさせるかどうかを判定しています。
						// IsInnerNoder()で、現在のマウスの座標がTreeViewItemの縦方向中央付近にある場合、下階層へアイテムをドロップするためのフラグをセットします。
						if (this.dragTargetElement is TreeView)
						{
							Point positionRelativeToItemContainer = e.GetPosition(this.itemContainerInnerItemsControl);
							var @flag = IsInnerNoder(itemContainerInnerItemsControl, positionRelativeToItemContainer, true);
							this.isTreeNodeSeparation = !@flag; // 符号を反転させる必要があります
							e.Data.SetData("DropInNode", @flag); // DropBeforePositionDataデータが示すノード内にD&Dするかどうか？
						}

						UpdateInsertionAdorner();
					}
				}
			}
			else
			{
				RemoveDragAdorner();
				e.Effects = DragDropEffects.None; // マウスカーソルの形状をドロップ不可能に。
			}
			e.Handled = true;
		}

		/// <summary>
		/// ドロップ先に指定したUIElementでドロップした場合の実行関数
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnDragTarget_PreviewDrop(object sender, DragEventArgs e)
		{
			object draggedItem = e.Data.GetData(this.dataFormat.Name);
			if (draggedItem != null)
			{
				DecideDropTarget(e);

				// ドロップ先がItemsControl(ListBoxなど)のコントロールの場合、
				// 挿入先を示すマーカを表示したAdornerを削除する
				if (sender is ItemsControl)
				{
					RemoveInsertionAdorner();
				}


				// ドロップしたときに行う判定
				bool isContinuedAllow = false;
				var f = GetTargetDragDropCallback(dragTargetElement);
				if (f != null)
				{
					// TargetDragDropCallbackの戻り値によって、
					// ドロップ処理を継続するか中止するか。
					isContinuedAllow = f.Invoke(e);
				}
				else
				{
					// TargetDragDropCallbackが未設定の場合は、常に下記の処理
					isContinuedAllow = true;
				}

				if (isContinuedAllow)
				{
					// Drop処理を行う
					DataObject dobj = e.Data as DataObject;
					var o = dobj.GetData("DragDropItemsControl");

					if (sender is ItemsControl) // ドロップ先がItemsControlの場合
					{
						var dtgItemsControl = this.dragTargetElement as ItemsControl;
						IEnumerable itemsSource = dtgItemsControl.ItemsSource;
						if (itemsSource is IList)
						{
							((IList)itemsSource).Insert(this.insertionIndex, o);
						}
					}
				}

				e.Handled = true; // イベントを処理したことを通知
			}
		}

		/// <summary>
		/// ドラッグ中にウィンドウのルートにマウスカーソルが入った場合の実行関数
		/// </summary>
		/// <remarks>
		/// ルートウィンドウはD&Dではドロップしないので、
		/// 常にカーソルの形状は「None(×マーク)」とし、
		/// イベントルーティングもストップする。
		/// </remarks>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnRootWindow_DragEnter(object sender, DragEventArgs e)
		{
			e.Effects = DragDropEffects.None; // マウスカーソルの形状をドロップ不可能に。
			e.Handled = true;
		}

		/// <summary>
		/// ドラッグ中にウィンドウのルートにマウスカーソルが離れようとしている場合の実行関数
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnRootWindow_DragLeave(object sender, DragEventArgs e)
		{
			e.Handled = true;
		}

		/// <summary>
		/// ドラッグ中にウィンドウのルートにマウスカーソルが存在している場合の実行関数
		/// </summary>
		/// <remarks>
		/// ルートウィンドウでは常にD&Dのドロップは行わない。
		/// そのため、マウスカーソルの形状は「None(×マーク)」とする。
		/// ただし、ドラッグ中のマーカ(Andorner)の描画は行う必要がある。
		/// </remarks>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnRootWindow_DragOver(object sender, DragEventArgs e)
		{
			var rootWindow = sender as FrameworkElement;

			double x = e.GetPosition(rootWindow).X;
			double y = e.GetPosition(rootWindow).Y;

			// Andornerの描画
			ShowDragAdorner(new Point
			{
				X = e.GetPosition(rootWindow).X - LastScrollOffsetPos.X,
				Y = e.GetPosition(rootWindow).Y - LastScrollOffsetPos.Y
			});


			e.Effects = DragDropEffects.None; // マウスカーソルの形状をドロップ不可能に。
			e.Handled = true;                 // イベントを処理したことを通知
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnRootWindow_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			if (this._TreeItemScroll != null)
			{
				this.LastScrollOffsetPos = new Point(0, 0);
				this._TreeItemScroll.Stop();
				this._TreeItemScroll = null;
			}
		}
		/// <summary>
		/// ドラッグ中のAdornerを削除
		/// </summary>
		private void RemoveDragAdorner()
		{
			if (dragAdorner != null)
			{
				AdornerLayer layer = AdornerLayer.GetAdornerLayer(dragSourcePanel);
				if (layer != null)
					layer.Remove(dragAdorner);

				this.dragAdorner = null;
			}
		}

		/// <summary>
		/// 挿入位置マーカが表示されたAdornerを削除する
		/// </summary>
		private void RemoveInsertionAdorner()
		{
			if (insertionAdorner != null)
			{
				insertionAdorner.Detach();
				insertionAdorner = null;
			}
		}

		/// <summary>
		/// ドラッグ中のAdornerを表示
		/// </summary>
		/// <param name="currentPosition"></param>
		private void ShowDragAdorner(Point currentPosition)
		{
			if (dragAdorner == null) // Adornerが作成されてない場合は、作成する
			{
				AdornerLayer layer = AdornerLayer.GetAdornerLayer(dragSourcePanel);
				this.dragAdorner = new DragAdorner(rootWindow, dragSourcePanel);

				if (layer != null)
					layer.Add(this.dragAdorner);
			}

			dragAdorner.SetPosition(currentPosition.X, currentPosition.Y);
		}
		/// <summary>
		/// 挿入位置マーカの表示位置を更新する
		/// </summary>
		private void UpdateInsertionAdorner()
		{
			if (insertionAdorner != null)
			{
				//Console.WriteLine("UpdateInsertionAdorner " + this.isTreeNodeSeparation);
				insertionAdorner.IsSeparatorHorizontal = this.isTreeNodeSeparation;
				insertionAdorner.IsInFirstHalf = this.isInFirstHalf;
				insertionAdorner.InvalidateVisual();
			}
		}

		#endregion メソッド

	}

	/// <summary>
	/// 挿入マーカをItemsControlに表示するためのAndorner
	/// </summary>
	public class InsertionAdorner : Adorner
	{
		//=====================================================================

		#region フィールド

		/// <summary>
		/// 挿入マーカの前景色
		/// </summary>
		private static Brush brush = Brushes.Gray;

		//=====================================================================
		/// <summary>
		/// 線を引くためのペン
		/// </summary>
		private static Pen linePen;

		/// <summary>
		/// 三角形を描画するためのパス
		/// </summary>
		private static PathGeometry trianglePath;
		//=====================================================================

		/// <summary>
		/// 描画先のAdorner用のレイヤー
		/// </summary>
		/// <remarks>
		/// このレイヤーに対してアイテム挿入マーカの描画を行います。
		/// </remarks>
		private AdornerLayer adornerLayer;

		#endregion フィールド

		#region コンストラクタ

		//=====================================================================
		static InsertionAdorner()
		{
			linePen = new Pen { Brush = brush, Thickness = 2 };
			linePen.Freeze();

			// 三角形状のパスを作成
			LineSegment firstLine = new LineSegment(new Point(0, -5), false);
			firstLine.Freeze();
			LineSegment secondLine = new LineSegment(new Point(0, 5), false);
			secondLine.Freeze();

			PathFigure figure = new PathFigure { StartPoint = new Point(5, 0) };
			figure.Segments.Add(firstLine);
			figure.Segments.Add(secondLine);
			figure.Freeze();

			trianglePath = new PathGeometry();
			trianglePath.Figures.Add(figure);
			trianglePath.Freeze();
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="isSeparatorHorizontal">水平方向へ挿入マーカを描画する場合はTrue</param>
		/// <param name="isInFirstHalf"></param>
		/// <param name="adornerElement"></param>
		/// <param name="adornerLayer">描画先のレイヤ</param>
		public InsertionAdorner(bool isSeparatorHorizontal,
			bool isInFirstHalf,
			UIElement adornerElement,
			AdornerLayer adornerLayer)
			: base(adornerElement)
		{
			this.IsSeparatorHorizontal = isSeparatorHorizontal;
			this.IsInFirstHalf = isInFirstHalf;
			this.adornerLayer = adornerLayer;
			this.IsHitTestVisible = false;

			this.adornerLayer.Add(this);
		}

		#endregion コンストラクタ

		//=====================================================================

		//=====================================================================

		#region プロパティ

		//=====================================================================
		/// <summary>
		/// 
		/// </summary>
		public bool IsInFirstHalf { get; set; }

		//=====================================================================
		/// <summary>
		/// 挿入マーカの描画方向
		/// </summary>
		/// <remarks>
		/// このフラグがTrueの場合、水平方向の描画を行う。
		/// </remarks>
		public bool IsSeparatorHorizontal { get; set; }

		#endregion プロパティ

		#region メソッド

		/// <summary>
		/// 描画した内容をレイヤから削除する
		/// </summary>
		public void Detach()
		{
			this.adornerLayer.Remove(this);
		}
		//=====================================================================

		//=====================================================================
		//=====================================================================

		//=====================================================================

		/// <summary>
		/// 挿入マーカを描画
		/// </summary>
		/// <param name="drawingContext"></param>
		protected override void OnRender(DrawingContext drawingContext)
		{
			Point startPoint; // 描画の開始位置を示す座標
			Point endPoint; // 描画の終了位置を示す座標
			CalculateStartAndEndPoint(out startPoint, out endPoint);


			// 直線の描画
			drawingContext.DrawLine(linePen, startPoint, endPoint);

			// 三角形の描画
			if (this.IsSeparatorHorizontal)
			{
				DrawTriangle(drawingContext, startPoint, 0);
				DrawTriangle(drawingContext, endPoint, 180);
			}
			else
			{
				DrawTriangle(drawingContext, startPoint, 90);
				DrawTriangle(drawingContext, endPoint, -90);
			}
		}

		/// <summary>
		/// 挿入マーカの描画座標をUIElementから算出する
		/// </summary>
		/// <param name="startPoint"></param>
		/// <param name="endPoint"></param>
		private void CalculateStartAndEndPoint(out Point startPoint, out Point endPoint)
		{
			// outのインスタンスを作成
			startPoint = new Point();
			endPoint = new Point();

			// 描画座標を、startPoint及びendPointに設定する
			double width = this.AdornedElement.RenderSize.Width;
			double height = this.AdornedElement.RenderSize.Height;

			if (this.IsSeparatorHorizontal) // 水平方向への描画
			{
				endPoint.X = width;
				if (!this.IsInFirstHalf)
				{
					startPoint.Y = height;
					endPoint.Y = height;
				}
			}
			else // 垂直方向への描画
			{
				endPoint.Y = height;
				if (!this.IsInFirstHalf)
				{
					startPoint.X = 0;// width;
					endPoint.X = 0;// width;
				}
			}
		}

		/// <summary>
		/// 三角形を指定の座標へ描画する
		/// </summary>
		/// <param name="drawingContext">描画先のコンテキスト</param>
		/// <param name="origin">描画座標</param>
		/// <param name="angle">描画角度</param>
		private void DrawTriangle(DrawingContext drawingContext, Point origin, double angle)
		{
			drawingContext.PushTransform(new TranslateTransform(origin.X, origin.Y));
			drawingContext.PushTransform(new RotateTransform(angle));

			// 三角形の描画は、ラインと同じ色で行う
			drawingContext.DrawGeometry(linePen.Brush, null, trianglePath);

			drawingContext.Pop();
			drawingContext.Pop();
		}

		#endregion メソッド

		//=====================================================================

		//=====================================================================
	}

	internal class TreeItemScroll
	{


		#region フィールド

		private const int HEIGHT = 15;      // 領域の高さ
		private Rect FBottomRect;
		// 同、下端部の流域
		private ScrollLine FScrollLine;

		// スクロールの要否あるいは方向
		private ScrollViewer FScrollViewer;

		// TreeView が内蔵する ScrollViewer コントロール
		private System.Windows.Threading.DispatcherTimer FTimer;

		private Rect FTopRect;              // TreeView 上端部にマウスが入るとスクロールを開始する領域
 // スクロールするためのタイマー
		private Action<Point> ScrollCallback = null; // スクロールした後に呼び出すコールバック(引数は移動量)
		private Point ScrollStartPos;

		#endregion フィールド

		#region コンストラクタ

		//---------------------------------------------------------------------------------------------
		public TreeItemScroll(FrameworkElement rootWindow, TreeView treeView, Action<Point> callback)
		{
			Point rootScreenPos = rootWindow.PointToScreen(new Point(0.0, 0.0));
			Point targetLeftTop = treeView.PointToScreen(new Point(0.0, 0.0));

			//Console.WriteLine("TreeViewControl X:{0} Y:{1}"
			//	, targetLeftTop.X - rootScreenPos.X
			//	, targetLeftTop.Y - rootScreenPos.Y);

			FTopRect = new Rect(targetLeftTop.X - rootScreenPos.X, targetLeftTop.Y - rootScreenPos.Y
				, treeView.ActualWidth, HEIGHT);
			FBottomRect = new Rect(targetLeftTop.X - rootScreenPos.X, targetLeftTop.Y - rootScreenPos.Y + treeView.ActualHeight - HEIGHT
				, treeView.ActualWidth, HEIGHT);
			Console.WriteLine("FTopRect = {0}", FTopRect);

			FTimer = new System.Windows.Threading.DispatcherTimer();
			FTimer.Interval = new TimeSpan(0, 0, 0, 0, 200); // スクロールは 200 ミリ秒ごと
			FTimer.Tick += new EventHandler(timer_Tick);

			FScrollViewer = this.GetFirstVisualChild<ScrollViewer>(treeView);

			ScrollStartPos = new Point(0, FScrollViewer.ContentVerticalOffset);

			ScrollCallback = callback;
		}

		#endregion コンストラクタ

		#region Enums

		//*********************************************************************************************
		// スクロールする方向
		internal enum ScrollLine
		{
			None,
			LineUp,
			LineDown
		}

		#endregion Enums

		#region メソッド

		//---------------------------------------------------------------------------------------------
		// マウスの現在位置に基づいて必要ならスクロールする（TreeViewItem の DragOver イベントから呼び出す）
		// point : マウスの現在位置
		public void Execute(Point point)
		{
			if (FTopRect.Contains(point))
			{   // TreeView の上端部にあるとき
				FScrollLine = ScrollLine.LineUp;
			}
			else if (FBottomRect.Contains(point)) // TreeView の下端部にあるとき
				FScrollLine = ScrollLine.LineDown;
			else
				FScrollLine = ScrollLine.None;
		}

		//---------------------------------------------------------------------------------------------
		// タイマーを起動する
		public void Start()
		{
			FTimer.Start();
		}

		//---------------------------------------------------------------------------------------------
		// タイマーを停止する
		public void Stop()
		{
			FTimer.Stop();
		}

		private void ExecuteCallback()
		{
			if (ScrollCallback != null)
			{
				var offset = new Point(0, ScrollStartPos.Y - FScrollViewer.ContentVerticalOffset);
				ScrollCallback(offset);
			}
		}

		//---------------------------------------------------------------------------------------------
		// TreeView が内蔵する ScrollViewer コントロールを取得する
		private T GetFirstVisualChild<T>(Visual obj) where T : Visual
		{
			if (obj != null)
			{
				for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); ++i)
				{
					Visual child = (Visual)VisualTreeHelper.GetChild(obj, i);

					if (child != null && child is T)
					{
						return (T)child;
					}

					T childItem = GetFirstVisualChild<T>(child);

					if (childItem != null)
					{
						return childItem;
					}
				}
			}

			return null;
		}

		//---------------------------------------------------------------------------------------------
		// タイマーによってスクロールを実行する
		private void timer_Tick(object sender, EventArgs e)
		{
			switch (FScrollLine)
			{
				case ScrollLine.LineDown:
					FScrollViewer.LineDown();
					ExecuteCallback();
					break;
				case ScrollLine.LineUp:
					FScrollViewer.LineUp();
					ExecuteCallback();
					break;
				default:
					break;
			}
		}

		#endregion メソッド

	} // end of TreeItemScroll class
}

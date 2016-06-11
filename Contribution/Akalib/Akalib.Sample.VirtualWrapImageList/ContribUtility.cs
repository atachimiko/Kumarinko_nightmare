using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Akalib.Sample.VirtualWrapImageList
{
	static class ContribUtility
	{

		#region フィールド

		private const string BAR = "---------------------------------------";

		#endregion フィールド


		#region メソッド

		public static string GetMessage(this ItemsControl itemsControl, string title)
		{
			var outPut = new StringBuilder();
			outPut.AppendLine(string.Format("{0}{1}{2}", title, Environment.NewLine, BAR));
			PrintListBoxState(itemsControl, outPut);

			IItemContainerGenerator generator = itemsControl.ItemContainerGenerator;
			for (int index = 0; index < itemsControl.Items.Count; index++)
			{
				GeneratorPosition position = generator.GeneratorPositionFromIndex(index);
				outPut.Append(string.Format("Index[{0}]={1}: Index={2}, Offset={3}",
												index, itemsControl.Items[index],
												position.Index, position.Offset));
				if (position.Offset == 0)
					outPut.Append(string.Format("   {0}", itemsControl.ItemContainerGenerator.ContainerFromIndex(index).GetHashCode()));
				outPut.AppendLine();
			}

			outPut.AppendLine(string.Format("{0}{1}", BAR, Environment.NewLine));
			return outPut.ToString();
		}

		private static void PrintListBoxState(ItemsControl listBox, StringBuilder outPut)
		{
			outPut.AppendLine("ListBoxの状態");
			outPut.AppendLine(listBox.ItemContainerGenerator.Status.ToString());
			outPut.AppendLine(string.Format("ItemsPanel: {0}", listBox.ItemsPanel.VisualTree.Type));
			outPut.AppendLine(string.Format("{0}", VirtualizingStackPanel.GetVirtualizationMode(listBox)));
			outPut.AppendLine(string.Format("{0}", BAR));
		}

		#endregion メソッド
	}
}

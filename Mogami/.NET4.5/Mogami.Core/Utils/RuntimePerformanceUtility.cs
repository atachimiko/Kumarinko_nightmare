using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mogami.Core.Utils
{
	public static class RuntimePerformanceUtility
	{

		#region メソッド

		/// <summary>
		/// CPUの使用率を取得するためのパフォーマンスカウンタを作成します
		/// </summary>
		/// <returns>CPU使用率を取得するパフォーマンスカウンタ。または、Null。</returns>
		public static System.Diagnostics.PerformanceCounter CreateCpuCounter()
		{
			//コンピュータ名(対象がローカルマシンの場合は「.」とする。
			string machineName = ".";
			//カテゴリ名
			string categoryName = "Processor";
			//カウンタ名
			string counterName = "% Processor Time";
			//インスタンス名
			string instanceName = "_Total";

			if (!System.Diagnostics.PerformanceCounterCategory.Exists(categoryName, machineName))
			{
				return null;
			}
			if (!System.Diagnostics.PerformanceCounterCategory.CounterExists(counterName, categoryName, machineName))
			{
				return null;
			}

			return new System.Diagnostics.PerformanceCounter(categoryName, counterName, instanceName, machineName);
		}

		#endregion メソッド

	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Akalib.Entity
{
	/// <summary>
	/// すべてのエンティティモデルクラスの基礎となる基本クラス
	/// </summary>
	/// <typeparam name="T">主キーの型。EFでは主キーが必須です。</typeparam>
	[DataContract]
	public abstract class Entity<T> : BaseEntity, IEntity<T>
	{
		/// <summary>
		/// 主キーの取得、または設定を行います。
		/// </summary>
		[DataMember]
		public virtual T Id { get; set; }
	}
}
using Akalib.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Mogami.Model
{
	public abstract class ServiceModel : IEntity<long>
	{
		[DataMember]
		public long Id
		{
			get;
			set;
		}
	}
}

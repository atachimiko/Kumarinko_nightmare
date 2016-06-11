using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mogami.Model
{
	/// <summary>
	/// 
	/// </summary>
	[Table("ApMetadata")]
	public sealed class ApMetadata : ServiceModel
	{
		[Required]
		public string Key { get; set; }

		public string Value { get; set; }
	}
}

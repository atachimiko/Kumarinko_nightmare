using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mogami.Core.Infrastructure
{
	public interface IThumbnailManager
	{
		void BuildThumbnail(string thumbnailhash, string baseImageFilePath);
	}
}

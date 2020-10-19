using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageGallery.Workers
{
	public static class Mime
	{
		public static Dictionary<String, String> Types = new Dictionary<String, String>()
		{
			{ "jpg", "image/jpeg" },
			{ "png", "image/png" },
			{ "bmp", "image/bmp" },
			{ "gif", "image/gif" },
			{ "tif", "image/tiff" },
			{ "webp", "image/webp" },
			{ "mp4", "video/mp4" }
		};
	}
}

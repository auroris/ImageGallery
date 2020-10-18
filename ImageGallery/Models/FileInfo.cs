using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageGallery.Models
{
	public class FileInfo
	{
		public FileInfo(String fileName, String type, String[] thumbnail)
		{
			this.FileName = fileName;
			this.Type = type;
			this.Thumbnails = thumbnail;
		}

		public String FileName { get; }
		public String Type { get; }
		public String[] Thumbnails { get; }
	}
}

using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ImageGallery.Models
{
	public class DirectoryInfo
	{
		public String Name { get; }
		public List<FileInfo> Items { get; } = new List<FileInfo>();
		private String basePath = Startup.Configuration.GetValue<String>("GalleryPath");
		private List<String> validExt = new List<String>();
		private int numThumbnails = Startup.Configuration.GetValue<int>("NumThumbnails");

		public DirectoryInfo(String path)
		{
			Name = path.Split(Path.DirectorySeparatorChar).Last();
			validExt.AddRange(Startup.Configuration.GetSection("Videos").Get<string[]>());
			validExt.AddRange(Startup.Configuration.GetSection("Images").Get<string[]>());

			foreach (String dir in Directory.GetDirectories(path))
			{
				Items.Add(new FileInfo(dir.Split(Path.DirectorySeparatorChar).Last(), "dir", GetDirThumbs(dir)));
			}

			foreach (String file in Directory.GetFiles(path))
			{
				String ext = Path.GetExtension(file).ToLower().Trim('.');

				if (validExt.Contains(ext))
				{
					Items.Add(new FileInfo(Path.GetFileNameWithoutExtension(file), ext, new string[] { file.Substring(basePath.Length + 1).Replace('\\', '/') + ":thumbnail" }));
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		private String[] GetDirThumbs(String path)
		{
			List<String> thumbs = new List<String>();

			foreach (String file in Directory.GetFiles(path))
			{
				if (thumbs.Count() >= numThumbnails)
				{
					return thumbs.ToArray();
				}
				
				String ext = Path.GetExtension(file).ToLower().Trim('.');
				if (validExt.Contains(ext))
				{
					thumbs.Add(file.Substring(basePath.Length + 1).Replace('\\', '/') + ":thumbnail");
				}
			}

			foreach (String dir in Directory.GetDirectories(path))
			{
				thumbs.AddRange(GetDirThumbs(dir));

				if (thumbs.Count() >= numThumbnails)
				{
					return thumbs.ToArray();
				}
			}

			return thumbs.ToArray();
		}
	}
}

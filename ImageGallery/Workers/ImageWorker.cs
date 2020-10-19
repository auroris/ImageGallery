using ImageMagick;
using System;
using System.IO;
using Trinet.Core.IO.Ntfs;
using System.Text.Json;

namespace ImageGallery.Workers
{
	public class ImageWorker
	{
		public static FileStream GetResized(String imagePath, String name, int width, int height)
		{
			FileInfo imageFileInfo = new FileInfo(imagePath);

			if (!imageFileInfo.AlternateDataStreamExists(name))
			{
				FileStream fileStream = new FileStream(imagePath, FileMode.Open, FileAccess.Read, FileShare.None);
				using (var image = new MagickImage(fileStream))
				{
					MagickGeometry size = new MagickGeometry(width, height);
					size.IgnoreAspectRatio = false;
					if (image.Width > width || image.Height > height) 
					{ 
						image.Resize(size); 
					}
					AlternateDataStreamInfo adsi = imageFileInfo.GetAlternateDataStream(name, FileMode.Create);
					FileStream writeStream = adsi.OpenWrite();
					image.Write(writeStream);
					writeStream.Close();
					WriteMetadata(image, imageFileInfo);
				}
				fileStream.Close();
			}

			AlternateDataStreamInfo readStream = imageFileInfo.GetAlternateDataStream(name, FileMode.Open);
			return readStream.OpenRead();
		}

		private static void WriteMetadata(MagickImage image, FileInfo imageFileInfo)
		{
			if (imageFileInfo.AlternateDataStreamExists("metadata.json")) { return; }

			IExifProfile exif = image.GetExifProfile();
			IXmpProfile xmp = image.GetXmpProfile();

			Metadata meta = new Metadata();
			meta.Updated = DateTime.Now;

			using (FileStream stream = imageFileInfo.GetAlternateDataStream("metadata.json", FileMode.Create).OpenWrite())
			{
				using (StreamWriter writer = new StreamWriter(stream))
				{
					writer.Write(JsonSerializer.Serialize(meta));
					writer.Flush();
				}
			}
		}
	}
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ImageGallery.Workers
{
	public class Metadata
	{
		public String Title { get; set; }
		public String Description { get; set;}
		public String Copyright { get; set; }
		public DateTime Updated { get; set; }
	}
}

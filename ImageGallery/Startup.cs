using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ImageMagick;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Xabe.FFmpeg;

namespace ImageGallery
{
	public class Startup
	{
		public static IConfiguration Configuration;

		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddControllers();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseRouting();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});

			MagickNET.SetGhostscriptDirectory(Path.Combine(env.ContentRootPath, "imagemagick"));
			MagickNET.SetTempDirectory(Path.Combine(env.ContentRootPath, "temp"));
			MagickNET.SetNativeLibraryDirectory(Path.Combine(env.ContentRootPath, "imagemagick"));
			//MagickNET.Initialize(Path.Combine(env.ContentRootPath, "imagemagick"));
			FFmpeg.SetExecutablesPath(Path.Combine(env.ContentRootPath, "ffmpeg"));
		}
	}
}

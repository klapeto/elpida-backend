using System;
using System.Collections.Generic;
using Elpida.Web.Frontend.Models;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace Elpida.Web.Frontend.Services
{
	public class DownloadService
	{
		public DownloadService(IWebAssemblyHostEnvironment environment)
		{
			string addressBranch;

			if (environment.IsProduction())
			{
				addressBranch = "master";
			}
			else if (environment.IsStaging())
			{
				addressBranch = "staging";
			}
			else
			{
				addressBranch = "develop";
			}

			string binaryUrl = $"https://gitlab.com/dev-hood/elpida/elpida/-/jobs/artifacts/{addressBranch}/raw/{{0}}";
			string sourceUrl = $"https://gitlab.com/dev-hood/elpida/{{0}}/-/archive/{addressBranch}/{{1}}";

			BinariesDownloadLinks = new[]
			{
				new DownloadLinkModel(
					"Windows",
					"x86-64",
					"zip",
					string.Format(binaryUrl, "Elpida-latest-x86_64.zip?job=deploy:windows"),
					string.Format(binaryUrl, "Elpida-latest-x86_64.zip.SHA256SUMS?job=deploy:windows")
				),
				new DownloadLinkModel(
					"Linux",
					"x86-64",
					"AppImage",
					string.Format(binaryUrl, "Elpida-latest-x86_64.AppImage?job=deploy:linux"),
					string.Format(binaryUrl, "Elpida-latest-x86_64.AppImage.SHA256SUMS?job=deploy:linux")
				),
				new DownloadLinkModel(
					"Linux",
					"AArch64",
					"AppImage",
					string.Format(binaryUrl, "Elpida-latest-AArch64.AppImage?job=deploy:linux"),
					string.Format(binaryUrl, "Elpida-latest-AArch64.AppImage.SHA256SUMS?job=deploy:linux")
				),
			};

			SourcesDownloadLinks = new[]
			{
				new DownloadLinkModel(
					"Application",
					"sources",
					"tar.gz",
					string.Format(sourceUrl, "elpida", "elpida-master.tar.gz"),
					null
					),
				new DownloadLinkModel(
					"Web",
					"sources",
					"tar.gz",
					string.Format(sourceUrl, "web", "web-master.tar.gz"),
					null
				),
			};
		}

		public IReadOnlyCollection<DownloadLinkModel> BinariesDownloadLinks { get; }

		public IReadOnlyCollection<DownloadLinkModel> SourcesDownloadLinks { get; }
	}
}
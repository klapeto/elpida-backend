using System;
using Elpida.Backend.Data.Abstractions.Models;
using Elpida.Backend.Services.Abstractions.Dtos;

namespace Elpida.Backend.Services
{
	public static class AssetInfoModelExtensions
	{
		public static AssetInfoDto ToDto(this AssetInfoModel model)
		{
			if (model == null) throw new ArgumentNullException(nameof(model));
			return new AssetInfoDto
			{
				Location = model.Location,
				Size = model.Size,
				Filename = model.Filename,
				Md5 = model.Md5
			};
		}
	}
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Elpida.Backend.Services.Abstractions.Dtos;

namespace Elpida.Backend.Services.Abstractions
{
	public interface IAssetsService
	{
		Task<Uri> CreateAsync(string filename, Stream inputData, CancellationToken cancellationToken = default);
		
		Task<IEnumerable<AssetInfoDto>> GetAssetsAsync(CancellationToken cancellationToken = default);
	}
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Elpida.Backend.Data.Abstractions.Models;

namespace Elpida.Backend.Data.Abstractions
{
	public interface IAssetsRepository
	{
		Task<Uri> CreateAsync(string filename, Stream inputData, CancellationToken cancellationToken = default);
		
		Task<IEnumerable<AssetInfoModel>> GetAssetsAsync(CancellationToken cancellationToken = default);
	}
}
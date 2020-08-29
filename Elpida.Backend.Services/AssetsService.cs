using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Elpida.Backend.Data.Abstractions;
using Elpida.Backend.Services.Abstractions;
using Elpida.Backend.Services.Abstractions.Dtos;

namespace Elpida.Backend.Services
{
	public class AssetsService : IAssetsService
	{
		private readonly IAssetsRepository _assetsRepository;

		public AssetsService(IAssetsRepository assetsRepository)
		{
			_assetsRepository = assetsRepository;
		}

		#region IAssetsService Members

		public Task<Uri> CreateAsync(string filename, Stream inputData, CancellationToken cancellationToken = default)
		{
			if (string.IsNullOrWhiteSpace(filename))
				throw new ArgumentException("Filename is empty!", nameof(filename));
			if (inputData == null) throw new ArgumentNullException(nameof(inputData));

			return _assetsRepository.CreateAsync(filename, inputData, cancellationToken);
		}

		public async Task<IEnumerable<AssetInfoDto>> GetAssetsAsync(CancellationToken cancellationToken = default)
		{
			return (await _assetsRepository.GetAssetsAsync(cancellationToken))
				.Select(m => m.ToDto())
				.ToArray();
		}

		#endregion
	}
}
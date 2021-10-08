using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Elpida.Backend.Services.Abstractions;
using Elpida.Backend.Services.Abstractions.Dtos.Cpu;
using Elpida.Backend.Services.Abstractions.Interfaces;
using Elpida.Backend.Services.Utilities;

namespace Elpida.Web.Frontend.Services
{
	public class CpuService : ICpuService
	{
		public Task<CpuDto> GetSingleAsync(long id, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public Task<PagedResult<CpuPreviewDto>> GetPagedPreviewsAsync(QueryRequest queryRequest, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public Task<CpuDto> GetOrAddAsync(CpuDto dto, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public IEnumerable<FilterExpression> ConstructCustomFilters<T, TR>(Expression<Func<T, TR>> baseExpression)
		{
			throw new NotImplementedException();
		}

		public IEnumerable<FilterExpression> GetFilterExpressions()
		{
			yield return FiltersTransformer.CreateFilter<CpuDto, string>("cpuModelName", model => model.ModelName);
			yield return FiltersTransformer.CreateFilter<CpuDto, string>("cpuVendor", model => model.Vendor);
			yield return FiltersTransformer.CreateFilter<CpuDto, long>("cpuFrequency", model => model.Frequency);
		}
	}
}
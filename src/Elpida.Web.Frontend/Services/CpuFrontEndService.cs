using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Elpida.Backend.Services.Abstractions;
using Elpida.Backend.Services.Abstractions.Dtos.Cpu;
using Elpida.Backend.Services.Abstractions.Interfaces;
using Elpida.Web.Frontend.Interfaces;
using Elpida.Web.Frontend.Models.Filters;

namespace Elpida.Web.Frontend.Services
{
	public class CpuFrontEndService : ICpuService, IFrontendService<CpuDto, CpuPreviewDto>
	{
		public Task<CpuDto> GetSingleAsync(long id, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public Task<PagedResult<CpuPreviewDto>> GetPagedPreviewsAsync(
			QueryRequest queryRequest,
			CancellationToken cancellationToken = default
		)
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

		public IEnumerable<FilterModel> CreateFilterModels()
		{
			yield return new StringFilterModel("CPU vendor", "cpuVendor");
			yield return new NumberFilterModel("CPU frequency", "cpuFrequency");
			yield return new DateTimeFilterModel("Start date", "startDate");
			yield return new RangeFilterModel("CPU cores", "cpuCores", 1, 2000);
		}
	}
}
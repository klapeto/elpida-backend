using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Elpida.Backend.Services.Abstractions.Dtos.Result;
using Elpida.Backend.Services.Abstractions.Dtos.Result.Batch;
using Elpida.Backend.Services.Abstractions.Interfaces;
using Elpida.Web.Frontend.Interfaces;
using Elpida.Web.Frontend.Models;
using Elpida.Web.Frontend.Models.Filters;

namespace Elpida.Web.Frontend.Services;

public class ResultsFrontEndService
	: FrontEndServiceBase<BenchmarkResultDto, BenchmarkResultPreviewDto>,
		IBenchmarkResultsService
{
	private readonly IFrontEndCpuService _cpuService;

	public ResultsFrontEndService(HttpClient httpClient, IFrontEndCpuService cpuService)
		: base(httpClient)
	{
		_cpuService = cpuService;
	}

	protected override string UrlRouteBase => "Result";

	public Task<IList<long>> AddBatchAsync(
		BenchmarkResultsBatchDto batch,
		CancellationToken cancellationToken = default
	)
	{
		throw new NotSupportedException();
	}

	public override QueryModel CreateSimpleQueryModel()
	{
		var filters = _cpuService.CreateSimpleQueryModel()
			.Filters.Concat(
				new FilterModel[]
				{
					new DateTimeFilterModel("Timestamp", "timeStamp"),
				}
			);

		return new QueryModel(filters);
	}

	public override QueryModel CreateAdvancedQueryModel()
	{
		var filters = _cpuService.CreateAdvancedQueryModel()
			.Filters.Concat(
				new FilterModel[]
				{
					new DateTimeFilterModel("Timestamp", "timeStamp"),
				}
			);

		return new QueryModel(filters);
	}
}
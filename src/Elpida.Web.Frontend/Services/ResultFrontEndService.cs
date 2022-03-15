using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Elpida.Backend.Services.Abstractions.Dtos.Result;
using Elpida.Backend.Services.Abstractions.Dtos.Result.Batch;
using Elpida.Web.Frontend.Interfaces;
using Elpida.Web.Frontend.Models;
using Elpida.Web.Frontend.Models.Filters;

namespace Elpida.Web.Frontend.Services;

public class ResultFrontEndService : FrontEndServiceBase<ResultDto, ResultPreviewDto>, IResultFrontEndService
{
	private readonly ICpuFrontEndService _service;

	public ResultFrontEndService(HttpClient httpClient, ICpuFrontEndService service)
		: base(httpClient)
	{
		_service = service;
	}

	protected override string UrlRouteBase => "Result";

	public Task<IList<long>> AddBatchAsync(
		ResultBatchDto batch,
		CancellationToken cancellationToken = default
	)
	{
		throw new NotSupportedException();
	}

	public override QueryModel CreateSimpleQueryModel()
	{
		var filters = _service.CreateSimpleQueryModel()
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
		var filters = _service.CreateAdvancedQueryModel()
			.Filters.Concat(
				new FilterModel[]
				{
					new DateTimeFilterModel("Timestamp", "timeStamp"),
				}
			);

		return new QueryModel(filters);
	}

	public override StringFilterModel? CreateSearchFilterModel()
	{
		return new StringFilterModel("Benchmark name", "benchmarkName");
	}
}
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Elpida.Backend.Services.Abstractions.Dtos.Result;
using Elpida.Backend.Services.Abstractions.Dtos.Result.Batch;
using Elpida.Backend.Services.Abstractions.Interfaces;
using Elpida.Web.Frontend.Models;
using Elpida.Web.Frontend.Models.Filters;

namespace Elpida.Web.Frontend.Services
{
	public class ResultsFrontEndService
		: FrontEndServiceBase<BenchmarkResultDto, BenchmarkResultPreviewDto>,
			IBenchmarkResultsService
	{
		public ResultsFrontEndService(HttpClient httpClient)
			: base(httpClient)
		{
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
			return new (Array.Empty<FilterModel>());
		}

		public override QueryModel CreateAdvancedQueryModel()
		{
			return new (Array.Empty<FilterModel>());
		}
	}
}
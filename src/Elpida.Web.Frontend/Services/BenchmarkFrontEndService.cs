using System.Net.Http;
using Elpida.Backend.Services.Abstractions.Dtos.Benchmark;
using Elpida.Web.Frontend.Interfaces;
using Elpida.Web.Frontend.Models;
using Elpida.Web.Frontend.Models.Filters;

namespace Elpida.Web.Frontend.Services;

public class BenchmarkFrontEndService
	: FrontEndServiceBase<BenchmarkDto, BenchmarkPreviewDto>,
		IBenchmarkFrontEndService
{
	public BenchmarkFrontEndService(HttpClient httpClient)
		: base(httpClient)
	{
	}

	protected override string UrlRouteBase => "Benchmark";

	public override QueryModel CreateSimpleQueryModel()
	{
		return new QueryModel(
			new FilterModel[]
			{
				new OptionFilterModel(
					"Benchmark",
					"benchmarkName",
					new[]
					{
						new OptionModel("L1D Cache Latency", "L1D Cache Latency"),
						new OptionModel("L2D Cache Latency", "L2D Cache Latency"),
						new OptionModel("L3D Cache Latency", "L3D Cache Latency"),
						new OptionModel("DRAM Latency", "DRAM Latency"),
						new OptionModel("Overall memory latency", "Overall memory latency"),
						new OptionModel("Memory Read Bandwidth", "Memory Read Bandwidth"),
						new OptionModel("Floyd Steinberg Dithering", "Floyd Steinberg Dithering"),
						new OptionModel("Png Encoding", "Png Encoding"),
						new OptionModel("Png Decoding", "Png Decoding"),
					}
				),
			}
		);
	}

	public override QueryModel CreateAdvancedQueryModel()
	{
		return new QueryModel(
			new FilterModel[]
			{
				new StringFilterModel("Benchmark name", "benchmarkName"),
			}
		);
	}

	public override StringFilterModel? CreateSearchFilterModel()
	{
		return new StringFilterModel("Benchmark name", "benchmarkName");
	}
}
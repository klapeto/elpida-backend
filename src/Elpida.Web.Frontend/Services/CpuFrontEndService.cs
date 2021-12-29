using System.Net.Http;
using Elpida.Backend.Services.Abstractions;
using Elpida.Backend.Services.Abstractions.Dtos.Cpu;
using Elpida.Web.Frontend.Interfaces;
using Elpida.Web.Frontend.Models;
using Elpida.Web.Frontend.Models.Filters;

namespace Elpida.Web.Frontend.Services;

public class CpuFrontEndService : FrontEndServiceBase<CpuDto, CpuPreviewDto>, IFrontEndCpuService
{
	public CpuFrontEndService(HttpClient httpClient)
		: base(httpClient)
	{
	}

	protected override string UrlRouteBase => "Cpu";

	public override QueryModel CreateAdvancedQueryModel()
	{
		return new QueryModel(
			new FilterModel[]
			{
				new StringFilterModel("CPU Vendor", "cpuVendor"),
				new StringFilterModel("CPU Model", "cpuModelName"),
				new NumberFilterModel("CPU Frequency", "cpuFrequency"),
			}
		);
	}

	public override StringFilterModel? CreateSearchFilterModel()
	{
		return new StringFilterModel("Cpu model name", "cpuModelName");
	}

	public override QueryModel CreateSimpleQueryModel()
	{
		return new QueryModel(
			new FilterModel[]
			{
				new OptionFilterModel(
					"CPU model",
					"cpuModelName",
					new[]
					{
						new OptionModel(null, null),
						new OptionModel("AMD Ryzen 3", "AMD Ryzen 3"),
						new OptionModel("AMD Ryzen 5", "AMD Ryzen 5"),
						new OptionModel("AMD Ryzen 7", "AMD Ryzen 7"),
						new OptionModel("AMD Ryzen 9", "AMD Ryzen 9"),
						new OptionModel("AMD Ryzen Threadripper", "AMD Ryzen Threadripper"),
						new OptionModel("AMD Epyc", "AMD Epyc"),
						new OptionModel("Intel Celeron", "Intel(R) Celeron"),
						new OptionModel("Intel Pentium", "Intel(R) Pentium"),
						new OptionModel("Intel Core i3", "Intel(R) Core(TM) i3"),
						new OptionModel("Intel Core i5", "Intel(R) Core(TM) i5"),
						new OptionModel("Intel Core i7", "Intel(R) Core(TM) i7"),
						new OptionModel("Intel Core i9", "Intel(R) Core(TM) i9"),
						new OptionModel("Intel Xeon", "Intel(R) Xeon(TM)"),
					}
				),
				new RangeFilterModel(
					"Min CPU Frequency",
					"cpuFrequency",
					500_000_000,
					10_000_000_000,
					FilterComparison.GreaterEqual,
					"Hz"
				),
			}
		);
	}
}
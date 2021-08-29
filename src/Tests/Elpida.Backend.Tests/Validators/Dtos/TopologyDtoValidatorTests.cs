using System.Collections.Generic;
using Elpida.Backend.Services.Abstractions.Dtos.Topology;
using Elpida.Backend.Validators;

namespace Elpida.Backend.Tests.Validators.Dtos
{
	public class TopologyDtoValidatorTests : ValidatorTest<TopologyDto, TopologyDtoValidator>
	{
		protected override IEnumerable<(TopologyDto, string)> GetInvalidData()
		{
			yield return (new TopologyDto(
				5,
				0,
				null,
				null,
				10,
				10,
				10,
				10,
				Generators.NewRootCpuNode()
			), $"Non Zero {nameof(TopologyDto.Id)}");

			yield return (new TopologyDto(
				-5,
				0,
				null,
				null,
				10,
				10,
				10,
				10,
				Generators.NewRootCpuNode()
			), $"Non Zero {nameof(TopologyDto.Id)}");

			yield return (new TopologyDto(
				0,
				5,
				null,
				null,
				10,
				10,
				10,
				10,
				Generators.NewRootCpuNode()
			), $"Non Zero {nameof(TopologyDto.CpuId)}");

			yield return (new TopologyDto(
				0,
				-5,
				null,
				null,
				10,
				10,
				10,
				10,
				Generators.NewRootCpuNode()
			), $"Non Zero {nameof(TopologyDto.CpuId)}");

			yield return (new TopologyDto(
				0,
				0,
				null,
				null,
				-5,
				10,
				10,
				10,
				Generators.NewRootCpuNode()
			), $"negative {nameof(TopologyDto.TotalLogicalCores)}");

			yield return (new TopologyDto(
				0,
				0,
				null,
				null,
				10,
				-5,
				10,
				10,
				Generators.NewRootCpuNode()
			), $"negative {nameof(TopologyDto.TotalPhysicalCores)}");

			yield return (new TopologyDto(
				0,
				0,
				null,
				null,
				10,
				10,
				-5,
				10,
				Generators.NewRootCpuNode()
			), $"negative {nameof(TopologyDto.TotalNumaNodes)}");

			yield return (new TopologyDto(
				0,
				0,
				null,
				null,
				10,
				10,
				10,
				-5,
				Generators.NewRootCpuNode()
			), $"negative {nameof(TopologyDto.TotalPackages)}");

			yield return (new TopologyDto(
				0,
				0,
				null,
				null,
				10,
				10,
				10,
				10,
				null!
			), $"null {nameof(TopologyDto.Root)}");
		}
	}
}
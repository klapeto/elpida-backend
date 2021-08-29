using System.Collections.Generic;
using Elpida.Backend.Services.Abstractions.Dtos.Topology;
using Elpida.Backend.Validators;

namespace Elpida.Backend.Tests.Validators.Dtos
{
	public class CpuNodeDtoValidatorTests : ValidatorTest<CpuNodeDto, CpuNodeDtoValidator>
	{
		protected override IEnumerable<(CpuNodeDto, string)> GetInvalidData()
		{
			yield return (new CpuNodeDto(
					(ProcessorNodeType)(-5),
					"Test",
					5,
					1,
					null,
					null
				),
				$"negative {nameof(CpuNodeDto.NodeType)}");

			yield return (new CpuNodeDto(
					(ProcessorNodeType)5000,
					"Test",
					5,
					1,
					null,
					null
				),
				$"invalid {nameof(CpuNodeDto.NodeType)}");

			yield return (new CpuNodeDto(
					ProcessorNodeType.Die,
					null!,
					5,
					1,
					null,
					null
				),
				$"null {nameof(CpuNodeDto.Name)}");

			yield return (new CpuNodeDto(
					ProcessorNodeType.Die,
					string.Empty,
					5,
					1,
					null,
					null
				),
				$"null {nameof(CpuNodeDto.Name)}");

			yield return (new CpuNodeDto(
					ProcessorNodeType.Die,
					" ",
					5,
					1,
					null,
					null
				),
				$"null {nameof(CpuNodeDto.Name)}");

			yield return (new CpuNodeDto(
					ProcessorNodeType.Die,
					new string('A', 100),
					5,
					1,
					null,
					null
				),
				$"null {nameof(CpuNodeDto.Name)}");
		}
	}
}
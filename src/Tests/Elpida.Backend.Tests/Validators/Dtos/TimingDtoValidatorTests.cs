using System.Collections.Generic;
using Elpida.Backend.Services.Abstractions.Dtos.Result;
using Elpida.Backend.Validators;

namespace Elpida.Backend.Tests.Validators.Dtos
{
	public class TimingDtoValidatorTests : ValidatorTest<TimingDto, TimingDtoValidator>
	{
		protected override IEnumerable<(TimingDto, string)> GetInvalidData()
		{
			yield return (new TimingDto(
				-1.0,
				0.0,
				0.0,
				0.0,
				0.0,
				0.0,
				0.0,
				0.0
			), $"negative {nameof(TimingDto.NotifyOverhead)}");

			yield return (new TimingDto(
				0.0,
				-1.0,
				0.0,
				0.0,
				0.0,
				0.0,
				0.0,
				0.0
			), $"negative {nameof(TimingDto.WakeupOverhead)}");

			yield return (new TimingDto(
				0.0,
				0.0,
				-1.0,
				0.0,
				0.0,
				0.0,
				0.0,
				0.0
			), $"negative {nameof(TimingDto.SleepOverhead)}");

			yield return (new TimingDto(
				0.0,
				0.0,
				0.0,
				-1.0,
				0.0,
				0.0,
				0.0,
				0.0
			), $"negative {nameof(TimingDto.NowOverhead)}");

			yield return (new TimingDto(
				0.0,
				0.0,
				0.0,
				0.0,
				-1.0,
				0.0,
				0.0,
				0.0
			), $"negative {nameof(TimingDto.LockOverhead)}");

			yield return (new TimingDto(
				0.0,
				0.0,
				0.0,
				0.0,
				0.0,
				-1.0,
				0.0,
				0.0
			), $"negative {nameof(TimingDto.LoopOverhead)}");

			yield return (new TimingDto(
				0.0,
				0.0,
				0.0,
				0.0,
				0.0,
				0.0,
				-1.0,
				0.0
			), $"negative {nameof(TimingDto.JoinOverhead)}");

			yield return (new TimingDto(
				0.0,
				0.0,
				0.0,
				0.0,
				0.0,
				0.0,
				0.0,
				-1.0
			), $"negative {nameof(TimingDto.TargetTime)}");
		}
	}
}
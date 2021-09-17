// =========================================================================
//
// Elpida HTTP Rest API
//
// Copyright (C) 2021 Ioannis Panagiotopoulos
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as
// published by the Free Software Foundation, either version 3 of the
// License, or (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Affero General Public License for more details.
//
// You should have received a copy of the GNU Affero General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
// =========================================================================

using System.Collections.Generic;
using Elpida.Backend.Services.Abstractions.Dtos.Result;

namespace Elpida.Backend.Tests.Validators.Dtos
{
	internal class TimingDtoValidatorTests : ValidatorTest<TimingDto>
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
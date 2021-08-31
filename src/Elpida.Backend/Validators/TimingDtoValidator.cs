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

using Elpida.Backend.Services.Abstractions.Dtos.Result;
using FluentValidation;

namespace Elpida.Backend.Validators
{
	internal class TimingDtoValidator : AbstractValidator<TimingDto>
	{
		public TimingDtoValidator()
		{
			RuleFor(dto => dto.JoinOverhead)
				.GreaterThanOrEqualTo(0.0);

			RuleFor(dto => dto.LockOverhead)
				.GreaterThanOrEqualTo(0.0);

			RuleFor(dto => dto.LoopOverhead)
				.GreaterThanOrEqualTo(0.0);

			RuleFor(dto => dto.NotifyOverhead)
				.GreaterThanOrEqualTo(0.0);

			RuleFor(dto => dto.NowOverhead)
				.GreaterThanOrEqualTo(0.0);

			RuleFor(dto => dto.SleepOverhead)
				.GreaterThanOrEqualTo(0.0);

			RuleFor(dto => dto.TargetTime)
				.GreaterThanOrEqualTo(0.0);

			RuleFor(dto => dto.WakeupOverhead)
				.GreaterThanOrEqualTo(0.0);
		}
	}
}
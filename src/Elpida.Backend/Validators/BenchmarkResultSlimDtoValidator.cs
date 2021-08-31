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

using System;
using System.Linq;
using Elpida.Backend.Services.Abstractions.Dtos.Result.Batch;
using FluentValidation;

namespace Elpida.Backend.Validators
{
	internal class BenchmarkResultSlimDtoValidator : AbstractValidator<BenchmarkResultSlimDto>
	{
		public BenchmarkResultSlimDtoValidator()
		{
			RuleFor(dto => dto.Affinity)
				.Must(a => a != null && a.Any() && a.All(i => i >= 0));

			RuleFor(dto => dto.Timestamp)
				.NotEmpty()
				.GreaterThan(DateTime.MinValue);

			RuleFor(dto => dto.Uuid)
				.NotEmpty();

			RuleFor(dto => dto.TaskResults)
				.NotEmpty();

			RuleFor(dto => dto.Score)
				.GreaterThan(0.0);
		}
	}
}
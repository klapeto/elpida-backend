/*
 * Elpida HTTP Rest API
 *
 * Copyright (C) 2020 Ioannis Panagiotopoulos
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Affero General Public License as
 * published by the Free Software Foundation, either version 3 of the
 * License, or (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Affero General Public License for more details.
 *
 * You should have received a copy of the GNU Affero General Public License
 * along with this program.  If not, see <https://www.gnu.org/licenses/>.
 */

using Elpida.Backend.Services.Abstractions.Dtos.Result;
using FluentValidation;

namespace Elpida.Backend.Validators
{
	public class ResultValidator : AbstractValidator<ResultDto>
	{
		public ResultValidator()
		{
			RuleFor(dto => dto.Id)
				.Empty();

			RuleFor(dto => dto.Affinity)
				.NotEmpty();

			RuleFor(dto => dto.TimeStamp)
				.Empty();

			RuleFor(dto => dto.Elpida)
				.NotNull();

			RuleFor(dto => dto.Result)
				.NotNull();

			RuleFor(dto => dto.System)
				.NotNull();
		}
	}
}
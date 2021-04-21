/*
 * Elpida HTTP Rest API
 *   
 * Copyright (C) 2021 Ioannis Panagiotopoulos
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

using Elpida.Backend.Services.Abstractions.Dtos;
using FluentValidation;

namespace Elpida.Backend.Validators
{
	public class ResultSpecificationValidator : AbstractValidator<ResultSpecificationDto>
	{
		public ResultSpecificationValidator()
		{
			RuleFor(dto => dto.Name)
				.NotEmpty()
				.MaximumLength(100);

			RuleFor(dto => dto.Description)
				.MaximumLength(250);

			RuleFor(dto => dto.Aggregation)
				.GreaterThanOrEqualTo(0);

			RuleFor(dto => dto.Type)
				.GreaterThanOrEqualTo(0);

			RuleFor(dto => dto.Unit)
				.NotEmpty();
		}
	}
}
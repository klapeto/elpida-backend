/*
 * Elpida HTTP Rest API
 *   
 * Copyright (C) 2020  Ioannis Panagiotopoulos
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
	public class TopologyValidator: AbstractValidator<TopologyDto>
	{
		public TopologyValidator()
		{
			RuleFor(dto => dto.Root)
				.NotNull();
			
			RuleFor(dto => dto.TotalDepth)
				.GreaterThanOrEqualTo(0);
			
			RuleFor(dto => dto.TotalPhysicalCores)
				.GreaterThanOrEqualTo(0);
						
			RuleFor(dto => dto.TotalLogicalCores)
				.GreaterThanOrEqualTo(0);
		}
	}
}
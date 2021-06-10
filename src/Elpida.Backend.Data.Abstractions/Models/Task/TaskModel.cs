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

using System;

namespace Elpida.Backend.Data.Abstractions.Models.Task
{
	public class TaskModel : Entity
	{
		public Guid Uuid { get; set; }

		public string Name { get; set; } = default!;

		public string Description { get; set; } = default!;

		public string ResultName { get; set; } = default!;

		public string ResultDescription { get; set; } = default!;

		public string ResultUnit { get; set; } = default!;

		public int ResultAggregation { get; set; }

		public int ResultType { get; set; }

		public string? InputName { get; set; }

		public string? InputDescription { get; set; }

		public string? InputUnit { get; set; }

		public string? InputProperties { get; set; }

		public string? OutputName { get; set; }

		public string? OutputDescription { get; set; }

		public string? OutputUnit { get; set; }

		public string? OutputProperties { get; set; }
	}
}
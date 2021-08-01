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
using Elpida.Backend.Common;

namespace Elpida.Backend.Services.Abstractions.Dtos.Statistics
{
	public class BenchmarkStatisticsPreviewDto : FoundationDto
	{
		public string CpuVendor { get; set; } = default!;

		public string CpuModelName { get; set; } = default!;

		public string BenchmarkName { get; set; } = default!;

		public string BenchmarkScoreUnit { get; set; } = default!;

		public Guid BenchmarkUuid { get; set; }

		public double Mean { get; set; }

		public long SampleSize { get; set; }

		public ValueComparison Comparison { get; set; }
	}
}
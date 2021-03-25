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

using System;

namespace Elpida.Backend.Services.Abstractions.Dtos.Result
{
	public class ResultPreviewDto
	{
		public long Id { get; set; }
		public string Name { get; set; } = string.Empty;
		public DateTime TimeStamp { get; set; }
		public int ElpidaVersionMajor { get; set; }
		public int ElpidaVersionMinor { get; set; }
		public int ElpidaVersionRevision { get; set; }
		public int ElpidaVersionBuild{ get; set; }
		public string OsName { get; set; } = string.Empty;
		public string OsVersion { get; set; } = string.Empty;
		public string CpuBrand { get; set; } = string.Empty;
		public long CpuFrequency { get; set; }
		public int CpuCores { get; set; }
		public int CpuLogicalCores { get; set; }
		public long MemorySize { get; set; }
	}
}
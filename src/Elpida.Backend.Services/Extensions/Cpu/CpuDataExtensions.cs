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
using Elpida.Backend.Data.Abstractions.Models.Cpu;
using Elpida.Backend.Services.Abstractions.Dtos.Cpu;
using Newtonsoft.Json;

namespace Elpida.Backend.Services.Extensions.Cpu
{
	public static class CpuDataExtensions
	{
		public static CpuDto ToDto(this CpuModel cpuModel)
		{
			return new ()
			{
				Id = cpuModel.Id,
				Architecture = cpuModel.Architecture,
				Vendor = cpuModel.Vendor,
				ModelName = cpuModel.ModelName,
				Frequency = cpuModel.Frequency,
				Smt = cpuModel.Smt,
				AdditionalInfo = JsonConvert.DeserializeObject<Dictionary<string, string>>(cpuModel.AdditionalInfo),
				Caches = JsonConvert.DeserializeObject<CpuCacheDto[]>(cpuModel.Caches),
				Features = JsonConvert.DeserializeObject<string[]>(cpuModel.Features),
			};
		}
	}
}
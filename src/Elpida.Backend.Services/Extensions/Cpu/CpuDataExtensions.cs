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

using System.Collections.Generic;
using Elpida.Backend.Data.Abstractions.Models.Cpu;
using Elpida.Backend.Services.Abstractions.Dtos.Cpu;
using Newtonsoft.Json;

namespace Elpida.Backend.Services.Extensions.Cpu
{
    public static class CpuDataExtensions
    {
        public static CpuDto ToDto(this CpuModel model)
        {
            return new CpuDto
            {
                Id = model.Id,
                Brand = model.Brand,
                Caches = JsonConvert.DeserializeObject<List<CpuCacheDto>>(model.Caches),
                Features = JsonConvert.DeserializeObject<List<string>>(model.Features),
                Frequency = model.Frequency,
                Smt = model.Smt,
                Vendor = model.Vendor,
                AdditionalInfo = JsonConvert.DeserializeObject<Dictionary<string, string>>(model.AdditionalInfo)
            };
        }

        public static CpuModel ToModel(this CpuDto cpuDto)
        {
            return new CpuModel
            {
                Id = cpuDto.Id,
                Brand = cpuDto.Brand,
                Caches = JsonConvert.SerializeObject(cpuDto.Caches),
                Features = JsonConvert.SerializeObject(cpuDto.Features),
                Frequency = cpuDto.Frequency,
                Smt = cpuDto.Smt,
                Vendor = cpuDto.Vendor,
                AdditionalInfo = JsonConvert.SerializeObject(cpuDto.AdditionalInfo)
            };
        }
    }
}
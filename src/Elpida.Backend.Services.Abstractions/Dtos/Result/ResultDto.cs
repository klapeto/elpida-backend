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

using System;
using System.Collections.Generic;

namespace Elpida.Backend.Services.Abstractions.Dtos.Result
{
    public class ResultDto : FountationDto
    {
        public DateTime TimeStamp { get; set; }
        public IList<long> Affinity { get; set; } = new List<long>();
        public ElpidaDto Elpida { get; set; } = new ElpidaDto();
        public SystemDto System { get; set; } = new SystemDto();
        public BenchmarkResultDto Result { get; set; } = new BenchmarkResultDto();
    }
}
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
using System.Collections.Generic;

namespace Elpida.Backend.Data.Abstractions.Models.Result
{
	public class ResultModel : IEntity
	{
		public string Id { get; set; }

		public DateTime TimeStamp { get; set; }
		
		public IList<long> Affinity { get; set; }
		public ElpidaModel Elpida { get; set; }
		public SystemModel System { get; set; }
		public BenchmarkResultModel Result { get; set; }
	}
}
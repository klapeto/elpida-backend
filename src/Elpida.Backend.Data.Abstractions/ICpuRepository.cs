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

using System.Threading;
using System.Threading.Tasks;
using Elpida.Backend.Data.Abstractions.Models.Result;

namespace Elpida.Backend.Data.Abstractions
{
	public interface ICpuRepository
	{
		Task<CpuModel> GetCpuByIdAsync(string id, CancellationToken cancellationToken);

		Task<TopologyModel> GetTopologyByIdAsync(string id, CancellationToken cancellationToken);

		Task CreateCpuAsync(CpuModel cpuModel, CancellationToken cancellationToken);

		Task CreateTopologyAsync(TopologyModel topologyModel, CancellationToken cancellationToken);
	}
}
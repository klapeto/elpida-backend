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

namespace Elpida.Backend.Services.Abstractions
{
	public class StatisticsUpdateRequest
	{
		public StatisticsUpdateRequest(long topologyId, long benchmarkId)
		{
			TopologyId = topologyId;
			BenchmarkId = benchmarkId;
		}

		public long TopologyId { get; }

		public long BenchmarkId { get; }

		public override bool Equals(object? obj)
		{
			if (ReferenceEquals(null, obj))
			{
				return false;
			}

			if (ReferenceEquals(this, obj))
			{
				return true;
			}

			return obj.GetType() == GetType() && Equals((StatisticsUpdateRequest)obj);
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(TopologyId, BenchmarkId);
		}

		private bool Equals(StatisticsUpdateRequest other)
		{
			return TopologyId == other.TopologyId && BenchmarkId == other.BenchmarkId;
		}
	}
}
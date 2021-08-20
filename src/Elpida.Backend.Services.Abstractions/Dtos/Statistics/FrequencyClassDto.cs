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

using System;

namespace Elpida.Backend.Services.Abstractions.Dtos.Statistics
{
	/// <summary>
	///     Represents a frequency class for a benchmark statistics.
	/// </summary>
	public class FrequencyClassDto
	{
		/// <summary>
		///     The low value for this class.
		/// </summary>
		public double Low { get; set; }

		/// <summary>
		///     The high value for this class.
		/// </summary>
		public double High { get; set; }

		/// <summary>
		///     How many samples this class has.
		/// </summary>
		public long Count { get; set; }

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

			return obj.GetType() == GetType() && Equals((FrequencyClassDto)obj);
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(Low, High);
		}

		public override string ToString()
		{
			return $"{{ {Low:r} - {High:r} }}";
		}

		protected bool Equals(FrequencyClassDto other)
		{
			return Low.Equals(other.Low) && High.Equals(other.High);
		}
	}
}
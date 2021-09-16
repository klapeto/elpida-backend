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
	public sealed class FrequencyClassDto
	{
		/// <summary>
		///     Initializes a new instance of the <see cref="FrequencyClassDto" /> class.
		/// </summary>
		/// <param name="low">The low value for this class.</param>
		/// <param name="high">The high value for this class.</param>
		/// <param name="count">How many samples this class has.</param>
		public FrequencyClassDto(double low, double high, long count)
		{
			Low = low;
			High = high;
			Count = count;
		}

		/// <summary>
		///     The low value for this class.
		/// </summary>
		public double Low { get; }

		/// <summary>
		///     The high value for this class.
		/// </summary>
		public double High { get; }

		/// <summary>
		///     How many samples this class has.
		/// </summary>
		public long Count { get; }

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

		private bool Equals(FrequencyClassDto other)
		{
			return Low.Equals(other.Low) && High.Equals(other.High);
		}
	}
}
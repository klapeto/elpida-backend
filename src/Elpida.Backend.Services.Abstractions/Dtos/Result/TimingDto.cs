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

namespace Elpida.Backend.Services.Abstractions.Dtos.Result
{
	/// <summary>
	///     Details of timing of a system.
	/// </summary>
	public class TimingDto
	{
		/// <summary>
		///     How many seconds a thread notify() takes.
		/// </summary>
		public double NotifyOverhead { get; set; }

		/// <summary>
		///     How many seconds a thread wakeup takes.
		/// </summary>
		public double WakeupOverhead { get; set; }

		/// <summary>
		///     How many extra seconds sleep() takes apart from the actual sleep time.
		/// </summary>
		public double SleepOverhead { get; set; }

		/// <summary>
		///     How many seconds a thread now() takes.
		/// </summary>
		public double NowOverhead { get; set; }

		/// <summary>
		///     How many seconds a thread mutex lock takes.
		/// </summary>
		public double LockOverhead { get; set; }

		/// <summary>
		///     How many seconds a bare loop takes.
		/// </summary>
		public double LoopOverhead { get; set; }

		/// <summary>
		///     How many seconds a thread join() takes apart from wait the time.
		/// </summary>
		public double JoinOverhead { get; set; }

		/// <summary>
		///     The minimum time a benchmark can take.
		/// </summary>
		public double TargetTime { get; set; }
	}
}
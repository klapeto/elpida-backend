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
	public sealed class TimingDto
	{
		/// <summary>
		///     Initializes a new instance of the <see cref="TimingDto" /> class.
		/// </summary>
		/// <param name="notifyOverhead">How many seconds a thread notify() takes.</param>
		/// <param name="wakeupOverhead">How many seconds a thread wakeup takes.</param>
		/// <param name="sleepOverhead">How many extra seconds sleep() takes apart from the actual sleep time.</param>
		/// <param name="nowOverhead">How many seconds a thread now() takes.</param>
		/// <param name="lockOverhead">How many seconds a thread mutex lock takes.</param>
		/// <param name="loopOverhead">How many seconds a bare loop takes.</param>
		/// <param name="joinOverhead">How many seconds a thread join() takes apart from wait the time.</param>
		/// <param name="targetTime">The minimum time a benchmark can take.</param>
		public TimingDto(
			double notifyOverhead,
			double wakeupOverhead,
			double sleepOverhead,
			double nowOverhead,
			double lockOverhead,
			double loopOverhead,
			double joinOverhead,
			double targetTime
		)
		{
			NotifyOverhead = notifyOverhead;
			WakeupOverhead = wakeupOverhead;
			SleepOverhead = sleepOverhead;
			NowOverhead = nowOverhead;
			LockOverhead = lockOverhead;
			LoopOverhead = loopOverhead;
			JoinOverhead = joinOverhead;
			TargetTime = targetTime;
		}

		/// <summary>
		///     How many seconds a thread notify() takes.
		/// </summary>
		public double NotifyOverhead { get; }

		/// <summary>
		///     How many seconds a thread wakeup takes.
		/// </summary>
		public double WakeupOverhead { get; }

		/// <summary>
		///     How many extra seconds sleep() takes apart from the actual sleep time.
		/// </summary>
		public double SleepOverhead { get; }

		/// <summary>
		///     How many seconds a thread now() takes.
		/// </summary>
		public double NowOverhead { get; }

		/// <summary>
		///     How many seconds a thread mutex lock takes.
		/// </summary>
		public double LockOverhead { get; }

		/// <summary>
		///     How many seconds a bare loop takes.
		/// </summary>
		public double LoopOverhead { get; }

		/// <summary>
		///     How many seconds a thread join() takes apart from wait the time.
		/// </summary>
		public double JoinOverhead { get; }

		/// <summary>
		///     The minimum time a benchmark can take.
		/// </summary>
		public double TargetTime { get; }
	}
}
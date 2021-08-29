using System;

namespace Elpida.Backend.Services.Abstractions.Dtos.Result.Batch
{
	/// <summary>
	///     Represents a specific task result.
	/// </summary>
	public sealed class TaskResultSlimDto
	{
		/// <summary>
		///     Initializes a new instance of the <see cref="TaskResultSlimDto" /> class.
		/// </summary>
		/// <param name="uuid">The UUID of this Task.</param>
		/// <param name="value">The value of the result.</param>
		/// <param name="time">The total time this task run.</param>
		/// <param name="inputSize">How much data this task received as input.</param>
		/// <param name="statistics">The result statistics.</param>
		public TaskResultSlimDto(Guid uuid, double value, double time, long inputSize, TaskRunStatisticsDto statistics)
		{
			Uuid = uuid;
			Value = value;
			Time = time;
			InputSize = inputSize;
			Statistics = statistics;
		}

		/// <summary>
		///     The UUID of this Task.
		/// </summary>
		public Guid Uuid { get; }

		/// <summary>
		///     The value of the result.
		/// </summary>
		public double Value { get; }

		/// <summary>
		///     The total time this task run.
		/// </summary>
		public double Time { get; }

		/// <summary>
		///     How much data this task received as input.
		/// </summary>
		public long InputSize { get; }

		/// <summary>
		///     The result statistics.
		/// </summary>
		public TaskRunStatisticsDto Statistics { get; }
	}
}
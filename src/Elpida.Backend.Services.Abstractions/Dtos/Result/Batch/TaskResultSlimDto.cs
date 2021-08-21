using System;

namespace Elpida.Backend.Services.Abstractions.Dtos.Result.Batch
{
	/// <summary>
	///     Represents a specific task result.
	/// </summary>
	public class TaskResultSlimDto
	{
		/// <summary>
		///     The UUID of this Task.
		/// </summary>
		public Guid Uuid { get; set; }

		/// <summary>
		///     The value of the result.
		/// </summary>
		public double Value { get; set; }

		/// <summary>
		///     The total time this task run.
		/// </summary>
		public double Time { get; set; }

		/// <summary>
		///     How much data this task received as input.
		/// </summary>
		public long InputSize { get; set; }

		/// <summary>
		///     The result statistics.
		/// </summary>
		public TaskRunStatisticsDto Statistics { get; set; } = new ();
	}
}
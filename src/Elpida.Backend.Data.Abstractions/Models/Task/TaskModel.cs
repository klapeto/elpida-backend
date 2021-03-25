using System;
using System.Collections.Generic;
using Elpida.Backend.Data.Abstractions.Interfaces;

namespace Elpida.Backend.Data.Abstractions.Models.Task
{
	public class TaskModel : Entity
	{
		public Guid Uuid { get; set; }
		public string Name { get; set; } = default!;
		public string Description { get; set; } = default!;

		public string ResultName { get; set; } = default!;
		public string ResultDescription { get; set; } = default!;
		public string ResultUnit { get; set; } = default!;
		public int ResultAggregation { get; set; }
		public int ResultType { get; set; }

		public string? InputName { get; set; }
		public string? InputDescription { get; set; }
		public string? InputUnit { get; set; }
		public string? InputProperties { get; set; }

		public string? OutputName { get; set; }
		public string? OutputDescription { get; set; }
		public string? OutputUnit { get; set; }
		public string? OutputProperties { get; set; }

		public ICollection<BenchmarkModel> Benchmarks { get; set; } = default!;
	}
}
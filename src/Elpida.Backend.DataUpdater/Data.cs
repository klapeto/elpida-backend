using System;
using System.Collections.Generic;
using Elpida.Backend.Services.Abstractions.Dtos;

namespace Elpida.Backend.DataUpdater
{
	internal class BenchmarkData
	{
		public Guid Uuid { get; set; }
		public string Name { get; set; } = string.Empty;

		public IList<Guid> TaskSpecifications { get; set; } = new List<Guid>();
	}
	
	internal class Data
	{
		public IReadOnlyList<BenchmarkData> Benchmarks { get; set; } = Array.Empty<BenchmarkData>();
		public IReadOnlyList<TaskDto> Tasks { get; set; }  = Array.Empty<TaskDto>();
	}
}
using System.Collections.Generic;
using Elpida.Backend.Services.Abstractions.Dtos;

namespace Elpida.Backend.DataUpdater
{
	public class Data
	{
		public IReadOnlyList<BenchmarkDto> Benchmarks { get; set; } = new BenchmarkDto[0];
		public IReadOnlyList<TaskDto> Tasks { get; set; }  = new TaskDto[0];
	}
}
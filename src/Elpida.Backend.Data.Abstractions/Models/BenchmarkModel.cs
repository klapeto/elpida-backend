using System;
using System.Collections.Generic;
using Elpida.Backend.Data.Abstractions.Interfaces;
using Elpida.Backend.Data.Abstractions.Models.Task;

namespace Elpida.Backend.Data.Abstractions.Models
{
	public class BenchmarkModel : Entity
	{
		public Guid Uuid { get; set; }
		public string Name { get; set; } = default!;
		public IList<TaskModel> Tasks { get; set; } = default!;
	}
}
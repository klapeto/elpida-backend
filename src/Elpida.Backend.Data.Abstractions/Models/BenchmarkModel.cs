using System.Collections.Generic;
using Elpida.Backend.Data.Abstractions.Interfaces;

namespace Elpida.Backend.Data.Abstractions.Models
{
	public class BenchmarkModel : IEntity
	{
		public string Id { get; set; } = string.Empty;

		public string Name { get; set; } = string.Empty;

		public IList<string> TaskSpecifications { get; set; } = new List<string>();
	}
}
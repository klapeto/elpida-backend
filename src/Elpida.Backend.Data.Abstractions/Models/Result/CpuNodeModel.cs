using System.Collections.Generic;

namespace Elpida.Backend.Data.Abstractions.Models.Result
{
	public class CpuNodeModel
	{
		public int NodeType { get; set; }
		public string Name { get; set; }
		public ulong? OsIndex { get; set; }
		public ulong? Value { get; set; }
		public IList<CpuNodeModel> Children { get; set; }
		public IList<CpuNodeModel> MemoryChildren { get; set; }
	}
}
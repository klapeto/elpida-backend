using System.Collections.Generic;

namespace Elpida.Backend.Services.Abstractions.Dtos.Result
{
	public class CpuNodeDto
	{
		public int NodeType { get; set; }
		public string Name { get; set; }
		public ulong? OsIndex { get; set; }
		public ulong? Value { get; set; }
		public IList<CpuNodeDto> Children { get; set; }
		public IList<CpuNodeDto> MemoryChildren { get; set; }
	}
}
using System;

namespace Elpida.Backend.Services.Abstractions.Dtos.Result
{
	public class ResultPreviewDto
	{
		public string Id { get; set; }

		public string Name { get; set; }
		public DateTime TimeStamp { get; set; }

		public int ElpidaVersionMajor { get; set; }
		public int ElpidaVersionMinor { get; set; }
		public int ElpidaVersionRevision { get; set; }
		public int ElpidaVersionBuild { get; set; }
		public string OsName { get; set; }
		public string OsVersion { get; set; }
		public string CpuBrand { get; set; }
		public ulong CpuFrequency { get; set; }
		public uint CpuCores { get; set; }
		public uint CpuLogicalCores { get; set; }
		public ulong MemorySize { get; set; }
	}
}
using System.Collections.Generic;
using System.Linq;
using Elpida.Backend.Services.Abstractions;
using Elpida.Backend.Services.Abstractions.Dtos.Cpu;
using Elpida.Backend.Services.Abstractions.Dtos.Elpida;
using Elpida.Backend.Services.Abstractions.Dtos.Os;
using Elpida.Backend.Services.Abstractions.Dtos.Result;
using Elpida.Backend.Services.Abstractions.Dtos.Topology;

namespace Elpida.Backend.Tests
{
	internal static class Generators
	{
		public static PageRequest NewPage()
		{
			return new (10, 10, 10);
		}

		public static VersionDto NewVersion()
		{
			return new (5, 1, 2, 0);
		}

		public static CompilerDto NewCompiler()
		{
			return new ("GCC", "10.0");
		}

		public static ElpidaDto NewElpida()
		{
			return new (3, NewVersion(), NewCompiler());
		}

		public static MemoryDto NewMemory()
		{
			return new (465454, 4096);
		}

		public static TimingDto NewTiming()
		{
			return new (123, 456, 789, 1569, 8799, 132, 64, 789);
		}

		public static SystemDto NewSystem()
		{
			return new (
				NewCpu(),
				NewOs(),
				NewTopology(),
				NewMemory(),
				NewTiming()
			);
		}

		public static CpuNodeDto NewRootCpuNode()
		{
			return new (ProcessorNodeType.Machine, "Machine", 0, 0, Enumerable.Repeat(NewChildNode(), 4).ToArray(),
				Enumerable.Repeat(NewChildNode(), 4).ToArray());
		}

		public static CpuNodeDto NewChildNode()
		{
			return new (ProcessorNodeType.Core, "Core", 0, 0, null, null);
		}

		public static TopologyDto NewTopology()
		{
			return new (7, 3, "x86_64", "Ryzen TR", 128, 64, 4, 1, NewRootCpuNode());
		}

		public static OsDto NewOs()
		{
			return new (8, "Linux", "KDE Neon", "21.1");
		}

		public static CpuCacheDto NewCache()
		{
			return new ("L5", "Satoko Hojo", 4654641, 1320);
		}

		public static CpuDto NewCpu()
		{
			return new (
				3,
				"ARM",
				"Samsung",
				"Exynos",
				52654684351,
				true,
				new Dictionary<string, string>(),
				Enumerable.Repeat(NewCache(), 4).ToArray(),
				Enumerable.Repeat("LOL", 4).ToArray()
			);
		}
	}
}
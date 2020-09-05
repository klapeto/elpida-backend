using System;

namespace Elpida.Backend.Services.Abstractions
{
	public class QueryInstance<T>
	{
		public T Value { get; set; }
		public string Comp { get; set; }
	}

	public class QueryRequest
	{
		public bool Descending { get; set; }

		public string OrderBy { get; set; }

		public PageRequest PageRequest { get; set; }

		public QueryInstance<string> CpuVendor { get; set; }

		public QueryInstance<string> CpuBrand { get; set; }

		public QueryInstance<string> BenchmarkName { get; set; }

		public QueryInstance<string> TaskName { get; set; }

		public QueryInstance<DateTime> StartTime { get; set; }

		public QueryInstance<DateTime> EndTime { get; set; }

		public QueryInstance<string> OsCategory { get; set; }

		public QueryInstance<string> OsName { get; set; }

		public QueryInstance<string> OsVersion { get; set; }

		public QueryInstance<ulong> CpuFrequency { get; set; }

		public QueryInstance<uint> CpuCores { get; set; }
		
		public QueryInstance<uint> CpuLogicalCores { get; set; }
		
		public QueryInstance<ulong> MemorySize { get; set; }
	}
}
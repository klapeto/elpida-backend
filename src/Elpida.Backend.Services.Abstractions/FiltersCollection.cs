using System;
using Elpida.Backend.Services.Abstractions;

public class FiltersCollection
{
	public QueryInstance<string> CpuVendor { get; set; }
	
	public QueryInstance<string> CpuBrand { get; set; }
	
	public QueryInstance<string> Name { get; set; }
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
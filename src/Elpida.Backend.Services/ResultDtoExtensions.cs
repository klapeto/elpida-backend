/*
 * Elpida HTTP Rest API
 *   
 * Copyright (C) 2020  Ioannis Panagiotopoulos
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Affero General Public License as
 * published by the Free Software Foundation, either version 3 of the
 * License, or (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Affero General Public License for more details.
 *
 * You should have received a copy of the GNU Affero General Public License
 * along with this program.  If not, see <https://www.gnu.org/licenses/>.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using Elpida.Backend.Data.Abstractions.Models.Result;
using Elpida.Backend.Services.Abstractions.Dtos.Result;

namespace Elpida.Backend.Services
{
	public static class ResultDtoExtensions
	{
		public static ResultModel ToModel(this ResultDto resultDto)
		{
			if (resultDto == null) throw new ArgumentNullException(nameof(resultDto));
			var resultModel = new ResultModel
			{
				Id = resultDto.Id,
				TimeStamp = resultDto.TimeStamp,
				Elpida =
					new ElpidaModel
					{
						Compiler = new CompilerModel
						{
							Name = resultDto.Elpida.Compiler.Name,
							Version = resultDto.Elpida.Compiler.Version
						},
						Version = new VersionModel
						{
							Build = resultDto.Elpida.Version.Build,
							Major = resultDto.Elpida.Version.Major,
							Minor = resultDto.Elpida.Version.Minor,
							Revision = resultDto.Elpida.Version.Revision
						}
					},
				Result =
					new BenchmarkResultModel
					{
						Name = resultDto.Result.Name,
						TaskResults = resultDto.Result.TaskResults.Select(d => new TaskResultModel
						{
							Description = d.Description,
							Name = d.Name,
							Suffix = d.Suffix,
							Time = d.Time,
							Type = d.Type,
							Value = d.Value,
							InputSize = d.InputSize
						}).ToList()
					},
				Affinity = resultDto.Affinity.ToList(),
				System = new SystemModel
				{
					Os = new OsModel
					{
						Category = resultDto.System.Os.Category,
						Name = resultDto.System.Os.Name,
						Version = resultDto.System.Os.Version
					},
					Cpu =
						new CpuModel
						{
							Brand = resultDto.System.Cpu.Brand,
							AdditionalInfo = resultDto.System.Cpu.AdditionalInfo,
							Frequency = resultDto.System.Cpu.Frequency,
							Smt = resultDto.System.Cpu.Smt,
							Features = resultDto.System.Cpu.Features,
							Vendor = resultDto.System.Cpu.Vendor,
							Caches = resultDto.System.Cpu.Caches.Select(c => new CpuCacheModel
							{
								Associativity = c.Associativity,
								Name = c.Name,
								Size = c.Size,
								LineSize = c.LineSize,
								LinesPerTag = c.LinesPerTag
							}).ToList()
						},
					Memory =
						new MemoryModel
						{
							PageSize = resultDto.System.Memory.PageSize,
							TotalSize = resultDto.System.Memory.TotalSize
						},
					Topology = new TopologyModel
					{
						TotalDepth = resultDto.System.Topology.TotalDepth,
						TotalLogicalCores = resultDto.System.Topology.TotalLogicalCores,
						TotalPhysicalCores = resultDto.System.Topology.TotalLogicalCores,
						Root = new CpuNodeModel
						{
							Name = resultDto.System.Topology.Root.Name,
							Value = resultDto.System.Topology.Root.Value,
							NodeType = resultDto.System.Topology.Root.NodeType,
							OsIndex = resultDto.System.Topology.Root.OsIndex
						}
					}
				}
			};

			if (resultDto.System.Topology.Root.Children != null)
			{
				resultModel.System.Topology.Root.Children = new List<CpuNodeModel>();
				foreach (var child in resultDto.System.Topology.Root.Children)
				{
					resultModel.System.Topology.Root.Children.Add(CreateChild(child));
				}
			}

			if (resultDto.System.Topology.Root.MemoryChildren != null)
			{
				resultModel.System.Topology.Root.MemoryChildren = new List<CpuNodeModel>();
				foreach (var child in resultDto.System.Topology.Root.MemoryChildren)
				{
					resultModel.System.Topology.Root.MemoryChildren.Add(CreateChild(child));
				}
			}


			return resultModel;
		}

		private static CpuNodeModel CreateChild(CpuNodeDto cpuNodeDto)
		{
			return new CpuNodeModel
			{
				Name = cpuNodeDto.Name,
				Value = cpuNodeDto.Value,
				NodeType = cpuNodeDto.NodeType,
				OsIndex = cpuNodeDto.OsIndex,
				Children = cpuNodeDto.Children?.Select(CreateChild).ToList(),
				MemoryChildren = cpuNodeDto.MemoryChildren?.Select(CreateChild).ToList()
			};
		}
	}
}
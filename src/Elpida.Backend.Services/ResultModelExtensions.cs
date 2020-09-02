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
	public static class ResultModelExtensions
	{
		public static ResultDto ToDto(this ResultModel resultModel)
		{
			if (resultModel == null) throw new ArgumentNullException(nameof(resultModel));
			var resultDto = new ResultDto
			{
				TimeStamp = resultModel.TimeStamp,
				Id = resultModel.Id,
				Elpida =
					new ElpidaDto
					{
						Compiler = new CompilerDto
						{
							Name = resultModel.Elpida.Compiler.Name,
							Version = resultModel.Elpida.Compiler.Version
						},
						Version = new VersionDto
						{
							Build = resultModel.Elpida.Version.Build,
							Major = resultModel.Elpida.Version.Major,
							Minor = resultModel.Elpida.Version.Minor,
							Revision = resultModel.Elpida.Version.Revision
						}
					},
				Affinity = resultModel.Affinity.ToList(),
				Result =
					new BenchmarkResultDto
					{
						Name = resultModel.Result.Name,
						TaskResults = resultModel.Result.TaskResults.Select(d => new TaskResultDto
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
				System = new SystemDto
				{
					Os = new OsDto
					{
						Category = resultModel.System.Os.Category,
						Name = resultModel.System.Os.Name,
						Version = resultModel.System.Os.Version
					},
					Cpu =
						new CpuDto
						{
							Brand = resultModel.System.Cpu.Brand,
							Family = resultModel.System.Cpu.Family,
							Frequency = resultModel.System.Cpu.Frequency,
							Model = resultModel.System.Cpu.Model,
							Smt = resultModel.System.Cpu.Smt,
							Stepping = resultModel.System.Cpu.Stepping,
							Features = resultModel.System.Cpu.Features,
							Vendor = resultModel.System.Cpu.Vendor,
							TurboBoost = resultModel.System.Cpu.TurboBoost,
							TurboBoost3 = resultModel.System.Cpu.TurboBoost3,
							Caches = resultModel.System.Cpu.Caches.Select(c => new CpuCacheDto
							{
								Associativity = c.Associativity,
								Name = c.Name,
								Size = c.Size,
								LineSize = c.LineSize,
								LinesPerTag = c.LinesPerTag
							}).ToList()
						},
					Memory =
						new MemoryDto
						{
							PageSize = resultModel.System.Memory.PageSize,
							TotalSize = resultModel.System.Memory.TotalSize
						},
					Topology = new TopologyDto
					{
						TotalDepth = resultModel.System.Topology.TotalDepth,
						TotalLogicalCores = resultModel.System.Topology.TotalLogicalCores,
						TotalPhysicalCores = resultModel.System.Topology.TotalLogicalCores,
						Root = new CpuNodeDto
						{
							Name = resultModel.System.Topology.Root.Name,
							Value = resultModel.System.Topology.Root.Value,
							NodeType = resultModel.System.Topology.Root.NodeType,
							OsIndex = resultModel.System.Topology.Root.OsIndex
						}
					}
				}
			};

			if (resultModel.System.Topology.Root.Children != null)
			{
				resultDto.System.Topology.Root.Children = new List<CpuNodeDto>();
				foreach (var child in resultModel.System.Topology.Root.Children)
				{
					resultDto.System.Topology.Root.Children.Add(CreateChild(child));
				}
			}

			if (resultModel.System.Topology.Root.MemoryChildren != null)
			{
				resultDto.System.Topology.Root.MemoryChildren = new List<CpuNodeDto>();
				foreach (var child in resultModel.System.Topology.Root.MemoryChildren)
				{
					resultDto.System.Topology.Root.MemoryChildren.Add(CreateChild(child));
				}
			}

			return resultDto;
		}

		private static CpuNodeDto CreateChild(CpuNodeModel cpuNodeModel)
		{
			return new CpuNodeDto
			{
				Name = cpuNodeModel.Name,
				Value = cpuNodeModel.Value,
				NodeType = cpuNodeModel.NodeType,
				OsIndex = cpuNodeModel.OsIndex,
				Children = cpuNodeModel.Children?.Select(CreateChild).ToList(),
				MemoryChildren = cpuNodeModel.MemoryChildren?.Select(CreateChild).ToList()
			};
		}
	}
}
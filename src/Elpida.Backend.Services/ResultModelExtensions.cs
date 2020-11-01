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
using Elpida.Backend.Data.Abstractions.Models;
using Elpida.Backend.Data.Abstractions.Models.Result;
using Elpida.Backend.Services.Abstractions.Dtos.Result;

namespace Elpida.Backend.Services
{
	public static class ResultModelExtensions
	{
		public static CpuCacheDto ToDto(this CpuCacheModel model)
		{
			if (model == null) throw new ArgumentNullException(nameof(model));

			return new CpuCacheDto
			{
				Associativity = model.Associativity,
				Name = model.Name,
				Size = model.Size,
				LineSize = model.LineSize,
				LinesPerTag = model.LinesPerTag
			};
		}
		
		public static CpuDto ToDto(this CpuModel model)
		{
			if (model == null) throw new ArgumentNullException(nameof(model));

			return new CpuDto
			{
				Brand = model.Brand,
				Caches = model.Caches.Select(c => c.ToDto()).ToList(),
				Features = model.Features,
				Frequency = model.Frequency,
				Smt = model.Smt,
				Vendor = model.Vendor,
				AdditionalInfo = model.AdditionalInfo
			};
		}

		public static TopologyDto ToDto(this TopologyModel model)
		{
			if (model == null) throw new ArgumentNullException(nameof(model));

			var topology = new TopologyDto
			{
				TotalDepth = model.TotalDepth,
				TotalLogicalCores = model.TotalLogicalCores,
				TotalPhysicalCores = model.TotalLogicalCores,
				Root = new CpuNodeDto
				{
					Name = model.Root.Name,
					Value = model.Root.Value,
					NodeType = model.Root.NodeType,
					OsIndex = model.Root.OsIndex
				}
			};

			if (model.Root.Children != null)
			{
				topology.Root.Children = new List<CpuNodeDto>();
				foreach (var child in model.Root.Children)
				{
					topology.Root.Children.Add(CreateChild(child));
				}
			}

			if (model.Root.MemoryChildren != null)
			{
				topology.Root.MemoryChildren = new List<CpuNodeDto>();
				foreach (var child in model.Root.MemoryChildren)
				{
					topology.Root.MemoryChildren.Add(CreateChild(child));
				}
			}

			return topology;
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


		public static ResultDto ToDto(this ResultProjection resultModel)
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
					Cpu = resultModel.System.Cpu.ToDto(),
					Memory =
						new MemoryDto
						{
							PageSize = resultModel.System.Memory.PageSize,
							TotalSize = resultModel.System.Memory.TotalSize
						},
					Topology = resultModel.System.Topology.ToDto(),
				}
			};

			return resultDto;
		}
	}
}
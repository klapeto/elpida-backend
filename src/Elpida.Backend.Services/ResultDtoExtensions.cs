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
		public static CpuCacheModel ToModel(this CpuCacheDto cpuCacheDto)
		{
			if (cpuCacheDto == null)
			{
				throw new ArgumentNullException(nameof(cpuCacheDto));
			}

			return new CpuCacheModel
			{
				Associativity = cpuCacheDto.Associativity,
				Name = cpuCacheDto.Name,
				Size = cpuCacheDto.Size,
				LineSize = cpuCacheDto.LineSize,
				LinesPerTag = cpuCacheDto.LinesPerTag
			};
		}

		public static CpuNodeModel ToModel(this CpuNodeDto cpuNodeDto)
		{
			if (cpuNodeDto == null)
			{
				throw new ArgumentNullException(nameof(cpuNodeDto));
			}

			return new CpuNodeModel
			{
				Name = cpuNodeDto.Name,
				Value = cpuNodeDto.Value,
				NodeType = cpuNodeDto.NodeType,
				OsIndex = cpuNodeDto.OsIndex,
				Children = cpuNodeDto.Children?.Select(c => c.ToModel()).ToList() ?? new List<CpuNodeModel>(),
				MemoryChildren = cpuNodeDto.MemoryChildren?.Select(c => c.ToModel()).ToList() ??
				                 new List<CpuNodeModel>()
			};
		}

		public static CpuModel ToModel(this CpuDto cpuDto, string id)
		{
			if (cpuDto == null)
			{
				throw new ArgumentNullException(nameof(cpuDto));
			}

			if (string.IsNullOrWhiteSpace(id))
			{
				throw new ArgumentException("'id' cannot be empty", nameof(id));
			}

			return new CpuModel
			{
				Id = id,
				Brand = cpuDto.Brand,
				Caches = cpuDto.Caches.Select(c => c.ToModel()).ToList(),
				Features = cpuDto.Features,
				Frequency = cpuDto.Frequency,
				Smt = cpuDto.Smt,
				Vendor = cpuDto.Vendor,
				AdditionalInfo = cpuDto.AdditionalInfo
			};
		}

		public static TopologyModel ToModel(this TopologyDto topologyDto, string id)
		{
			if (topologyDto == null)
			{
				throw new ArgumentNullException(nameof(topologyDto));
			}

			if (string.IsNullOrWhiteSpace(id))
			{
				throw new ArgumentException("'id' cannot be empty", nameof(id));
			}

			return new TopologyModel
			{
				Id = id,
				TotalDepth = topologyDto.TotalDepth,
				TotalLogicalCores = topologyDto.TotalLogicalCores,
				TotalPhysicalCores = topologyDto.TotalLogicalCores,
				Root = topologyDto.Root.ToModel()
			};
		}

		public static ElpidaModel ToModel(this ElpidaDto elpidaDto)
		{
			if (elpidaDto == null)
			{
				throw new ArgumentNullException(nameof(elpidaDto));
			}

			return new ElpidaModel
			{
				Compiler = elpidaDto.Compiler.ToModel(),
				Version = elpidaDto.Version.ToModel()
			};
		}

		public static CompilerModel ToModel(this CompilerDto compilerDto)
		{
			if (compilerDto == null)
			{
				throw new ArgumentNullException(nameof(compilerDto));
			}

			return new CompilerModel
			{
				Name = compilerDto.Name,
				Version = compilerDto.Version
			};
		}


		public static VersionModel ToModel(this VersionDto versionDto)
		{
			if (versionDto == null)
			{
				throw new ArgumentNullException(nameof(versionDto));
			}

			return new VersionModel
			{
				Build = versionDto.Build,
				Major = versionDto.Major,
				Minor = versionDto.Minor,
				Revision = versionDto.Revision
			};
		}

		public static TaskResultModel ToModel(this TaskResultDto taskResultDto)
		{
			if (taskResultDto == null)
			{
				throw new ArgumentNullException(nameof(taskResultDto));
			}

			return new TaskResultModel
			{
				Description = taskResultDto.Description,
				Name = taskResultDto.Name,
				Suffix = taskResultDto.Suffix,
				Time = taskResultDto.Time,
				Type = taskResultDto.Type,
				Value = taskResultDto.Value,
				InputSize = taskResultDto.InputSize
			};
		}

		public static BenchmarkResultModel ToModel(this BenchmarkResultDto benchmarkResultDto)
		{
			if (benchmarkResultDto == null)
			{
				throw new ArgumentNullException(nameof(benchmarkResultDto));
			}

			return new BenchmarkResultModel
			{
				Name = benchmarkResultDto.Name,
				TaskResults = benchmarkResultDto.TaskResults.Select(r => r.ToModel()).ToList()
			};
		}

		public static OsModel ToModel(this OsDto osDto)
		{
			if (osDto == null)
			{
				throw new ArgumentNullException(nameof(osDto));
			}

			return new OsModel
			{
				Category = osDto.Category,
				Name = osDto.Name,
				Version = osDto.Version
			};
		}

		public static MemoryModel ToModel(this MemoryDto memoryDto)
		{
			if (memoryDto == null)
			{
				throw new ArgumentNullException(nameof(memoryDto));
			}

			return new MemoryModel
			{
				PageSize = memoryDto.PageSize,
				TotalSize = memoryDto.TotalSize
			};
		}


		public static SystemModel ToModel(this SystemDto systemDto, string cpuId, string topologyId)
		{
			if (systemDto == null)
			{
				throw new ArgumentNullException(nameof(systemDto));
			}

			if (string.IsNullOrWhiteSpace(cpuId))
			{
				throw new ArgumentException("'cpuId' cannot be empty", nameof(cpuId));
			}

			if (string.IsNullOrWhiteSpace(topologyId))
			{
				throw new ArgumentException("'topologyId' cannot be empty", nameof(topologyId));
			}

			return new SystemModel
			{
				Memory = systemDto.Memory.ToModel(),
				Os = systemDto.Os.ToModel(),
				CpuId = cpuId,
				TopologyId = topologyId
			};
		}

		public static ResultModel ToModel(this ResultDto resultDto, string id, string cpuId, string topologyId)
		{
			if (resultDto == null)
			{
				throw new ArgumentNullException(nameof(resultDto));
			}

			if (string.IsNullOrWhiteSpace(id))
			{
				throw new ArgumentException("id cannot be empty", nameof(id));
			}
			
			if (string.IsNullOrWhiteSpace(cpuId))
			{
				throw new ArgumentException("cpuId cannot be empty", nameof(cpuId));
			}
			
			if (string.IsNullOrWhiteSpace(topologyId))
			{
				throw new ArgumentException("cpuId cannot be empty", nameof(topologyId));
			}

			return new ResultModel
			{
				Id = id,
				TimeStamp = resultDto.TimeStamp,
				Elpida = resultDto.Elpida.ToModel(),
				Result = resultDto.Result.ToModel(),
				Affinity = resultDto.Affinity.ToList(),
				System = resultDto.System.ToModel(cpuId, topologyId)
			};
		}
	}
}
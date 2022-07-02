// =========================================================================
//
// Elpida HTTP Rest API
//
// Copyright (C) 2021 Ioannis Panagiotopoulos
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as
// published by the Free Software Foundation, either version 3 of the
// License, or (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Affero General Public License for more details.
//
// You should have received a copy of the GNU Affero General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
// =========================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using Elpida.Backend.Data.Abstractions.Models.Cpu;
using Elpida.Backend.Data.Abstractions.Models.ElpidaVersion;
using Elpida.Backend.Data.Abstractions.Models.Os;
using Elpida.Backend.Data.Abstractions.Models.Topology;
using Elpida.Backend.Services.Abstractions.Dtos.Cpu;
using Elpida.Backend.Services.Abstractions.Dtos.Elpida;
using Elpida.Backend.Services.Abstractions.Dtos.Os;
using Elpida.Backend.Services.Abstractions.Dtos.Topology;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Elpida.Backend.Services.Tests
{
	public static class AssertExtensions
	{
		public static void AssertEqual(this TopologyModel model, TopologyDto dto)
		{
			Assert.AreEqual(model.Id, dto.Id);
			Assert.AreEqual(model.Cpu.Id, dto.CpuId);
			Assert.AreEqual(model.TotalPackages, dto.TotalPackages);
			Assert.AreEqual(model.TotalNumaNodes, dto.TotalNumaNodes);
			Assert.AreEqual(model.TotalPhysicalCores, dto.TotalPhysicalCores);
			Assert.AreEqual(model.TotalLogicalCores, dto.TotalLogicalCores);
			Assert.AreEqual(model.Cpu.ModelName, dto.CpuModelName);

			var root = JsonConvert.DeserializeObject<CpuNodeDto>(model.Root);

			AssertEqual(root!, dto.Root);
		}

		public static void AssertEqual(this CpuNodeDto a, CpuNodeDto b)
		{
			Assert.AreEqual(a.Name, b.Name);
			Assert.AreEqual(a.Value, b.Value);
			Assert.AreEqual(a.NodeType, b.NodeType);
			Assert.AreEqual(a.OsIndex, b.OsIndex);

			if (a.Children != null)
			{
				Assert.NotNull(b.Children);
				Assert.AreEqual(a.Children.Length, b.Children!.Length);

				for (var i = 0; i < a.Children.Length; i++)
				{
					AssertEqual(a.Children[i], b.Children[i]);
				}
			}
			else
			{
				Assert.Null(b.Children);
			}

			if (a.MemoryChildren != null)
			{
				Assert.NotNull(b.MemoryChildren);
				Assert.AreEqual(a.MemoryChildren.Length, b.MemoryChildren!.Length);

				for (var i = 0; i < a.MemoryChildren.Length; i++)
				{
					AssertEqual(a.MemoryChildren[i], b.MemoryChildren[i]);
				}
			}
			else
			{
				Assert.Null(b.MemoryChildren);
			}
		}

		public static void AssertEqual(this CpuModel model, CpuDto dto)
		{
			Assert.AreEqual(model.Id, dto.Id);
			Assert.AreEqual(model.Architecture, dto.Architecture);
			Assert.AreEqual(model.Frequency, dto.Frequency);
			Assert.AreEqual(model.Smt, dto.Smt);
			Assert.AreEqual(model.Vendor, dto.Vendor);
			Assert.AreEqual(model.ModelName, dto.ModelName);

			JsonConvert.DeserializeObject<IDictionary<string, string>?>(model.AdditionalInfo)
				!.AssertCollectionsEqual(
					dto.AdditionalInfo!,
					(a, b) =>
					{
						Assert.AreEqual(a.Key, b.Key);
						Assert.AreEqual(a.Value, b.Value);
					}
				);

			JsonConvert.DeserializeObject<string[]>(model.Features)
				!.AssertCollectionsEqual(dto.Features!);

			JsonConvert.DeserializeObject<CpuCacheDto[]?>(model.Caches)
				!.AssertCollectionsEqual(
					dto.Caches!,
					(a, b) =>
					{
						Assert.AreEqual(a.Associativity, b.Associativity);
						Assert.AreEqual(a.Name, b.Name);
						Assert.AreEqual(a.Size, b.Size);
						Assert.AreEqual(a.LineSize, b.LineSize);
					}
				);
		}

		public static void AssertEqual(this ElpidaVersionModel versionModel, ElpidaVersionDto versionDto)
		{
			Assert.AreEqual(versionModel.Id, versionDto.Id);
			Assert.AreEqual(versionModel.CompilerName, versionDto.Compiler.Name);
			Assert.AreEqual(versionModel.CompilerVersion, versionDto.Compiler.Version);
			Assert.AreEqual(versionModel.VersionMajor, versionDto.Version.Major);
			Assert.AreEqual(versionModel.VersionMinor, versionDto.Version.Minor);
			Assert.AreEqual(versionModel.VersionRevision, versionDto.Version.Revision);
			Assert.AreEqual(versionModel.VersionBuild, versionDto.Version.Build);
		}

		public static void AssertEqual(this OperatingSystemModel model, OperatingSystemDto dto)
		{
			Assert.AreEqual(model.Id, dto.Id);
			Assert.AreEqual(model.Category, dto.Category);
			Assert.AreEqual(model.Name, dto.Name);
			Assert.AreEqual(model.Version, dto.Version);
		}

		public static void AssertCollectionsEqual<T>(
			this IEnumerable<T> a,
			IEnumerable<T> b,
			Action<T, T>? assertAction = null
		)
		{
			var aArr = a.ToArray();
			var bArr = b.ToArray();

			Assert.AreEqual(aArr.Length, bArr.Length);

			for (var i = 0; i < aArr.Length; i++)
			{
				if (assertAction != null)
				{
					assertAction(aArr[i], bArr[i]);
				}
				else
				{
					Assert.AreEqual(aArr[i], bArr[i]);
				}
			}
		}
	}
}
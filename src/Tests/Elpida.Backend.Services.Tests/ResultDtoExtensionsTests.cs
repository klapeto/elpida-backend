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
using Elpida.Backend.Services.Abstractions.Dtos.Result;
using NUnit.Framework;

namespace Elpida.Backend.Services.Tests
{
	public class ResultDtoExtensionsTests
	{
		[Test]
		public void ToResultModel_Null_ThrowsArgumentNullException()
		{
			Assert.Throws<ArgumentNullException>(() => ((ResultDto) null).ToModel());
		}
		
		[Test]
		public void ToModelSuccess()
		{
			const double tolerance = 0.01;
			var dto = Generators.CreateNewResultDto();
			var model = dto.ToModel();

			Assert.AreEqual(dto.Id, model.Id);
			Assert.AreEqual(dto.TimeStamp, model.TimeStamp);

			Assert.True(Helpers.AssertCollectionsAreEqual(dto.Affinity, model.Affinity, (a, b) => a == b));

			Assert.AreEqual(dto.Elpida.Compiler.Name, model.Elpida.Compiler.Name);
			Assert.AreEqual(dto.Elpida.Compiler.Version, model.Elpida.Compiler.Version);
			Assert.AreEqual(dto.Elpida.Version.Build, model.Elpida.Version.Build);
			Assert.AreEqual(dto.Elpida.Version.Major, model.Elpida.Version.Major);
			Assert.AreEqual(dto.Elpida.Version.Minor, model.Elpida.Version.Minor);
			Assert.AreEqual(dto.Elpida.Version.Revision, model.Elpida.Version.Revision);

			Assert.AreEqual(dto.Result.Name, model.Result.Name);

			Assert.True(Helpers.AssertCollectionsAreEqual(dto.Result.TaskResults, model.Result.TaskResults,
				(a, b) => a.Name == b.Name
				          && a.Description == b.Description
				          && a.Suffix == b.Suffix
				          && Math.Abs(a.Time - b.Time) < tolerance
				          && a.Type == b.Type
				          && Math.Abs(a.Value - b.Value) < tolerance
				          && a.InputSize == b.InputSize));

			Assert.AreEqual(dto.System.Cpu.Brand, model.System.Cpu.Brand);
			Assert.AreEqual(dto.System.Cpu.Family, model.System.Cpu.Family);
			Assert.AreEqual(dto.System.Cpu.Frequency, model.System.Cpu.Frequency);
			Assert.AreEqual(dto.System.Cpu.Model, model.System.Cpu.Model);
			Assert.AreEqual(dto.System.Cpu.Smt, model.System.Cpu.Smt);
			Assert.AreEqual(dto.System.Cpu.Stepping, model.System.Cpu.Stepping);
			Assert.AreEqual(dto.System.Cpu.Vendor, model.System.Cpu.Vendor);
			Assert.AreEqual(dto.System.Cpu.TurboBoost, model.System.Cpu.TurboBoost);
			Assert.AreEqual(dto.System.Cpu.TurboBoost3, model.System.Cpu.TurboBoost3);

			Assert.True(Helpers.AssertCollectionsAreEqual(dto.System.Cpu.Features, model.System.Cpu.Features,
				(a, b) => a == b));

			Assert.True(Helpers.AssertCollectionsAreEqual(dto.System.Cpu.Caches, model.System.Cpu.Caches, (a, b) =>
				a.Associativity == b.Associativity
				&& a.Name == b.Name
				&& a.Size == b.Size
				&& a.LineSize == b.LineSize
				&& a.LinesPerTag == b.LinesPerTag));


			Assert.AreEqual(dto.System.Memory.PageSize, model.System.Memory.PageSize);
			Assert.AreEqual(dto.System.Memory.TotalSize, model.System.Memory.TotalSize);

			Assert.AreEqual(dto.System.Os.Category, model.System.Os.Category);
			Assert.AreEqual(dto.System.Os.Name, model.System.Os.Name);
			Assert.AreEqual(dto.System.Os.Version, model.System.Os.Version);

			Assert.AreEqual(dto.System.Topology.TotalDepth, dto.System.Topology.TotalDepth);
			Assert.AreEqual(dto.System.Topology.TotalLogicalCores, dto.System.Topology.TotalLogicalCores);
			Assert.AreEqual(dto.System.Topology.TotalPhysicalCores, dto.System.Topology.TotalPhysicalCores);

			Helpers.AssertTopology(model.System.Topology.Root, dto.System.Topology.Root);
		}
	}
}
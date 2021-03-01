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
			Assert.Throws<ArgumentNullException>(() => ((ResultDto) null).ToModel("x", "y", "z"));
		}
		
		[Test]
		[TestCase(null, "Test", "Test")]
		[TestCase("Test", null, "Test")]
		[TestCase("Test", "Test", null)]
		public void ToResultModel_Null_ThrowsArgumentException(string id, string cpuId, string topologyId)
		{
			Assert.Throws<ArgumentException>(() => new ResultDto().ToModel(id, cpuId, topologyId));
		}
		
		[Test]
		public void ToModelSuccess()
		{
			const double tolerance = 0.01;
			var dto = Generators.CreateNewResultDto();
			var model = dto.ToModel(dto.Id, "y", "z");

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


			Assert.AreEqual(dto.System.Memory.PageSize, model.System.Memory.PageSize);
			Assert.AreEqual(dto.System.Memory.TotalSize, model.System.Memory.TotalSize);

			Assert.AreEqual(dto.System.Os.Category, model.System.Os.Category);
			Assert.AreEqual(dto.System.Os.Name, model.System.Os.Name);
			Assert.AreEqual(dto.System.Os.Version, model.System.Os.Version);

		}
	}
}
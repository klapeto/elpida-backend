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
using Elpida.Backend.Data.Abstractions.Models.Result;
using NUnit.Framework;

namespace Elpida.Backend.Services.Tests
{
	public class ResultModelExtensionsTests
	{
		[Test]
		public void ToDto_Null_ThrowsArgumentNullException()
		{
			Assert.Throws<ArgumentNullException>(() => ((ResultModel) null).ToDto(new CpuModel(), new TopologyModel()));
			Assert.Throws<ArgumentNullException>(() => ((ResultModel) null).ToDto(new CpuModel(), null));
			Assert.Throws<ArgumentNullException>(() => ((ResultModel) null).ToDto(null, new TopologyModel()));
		}

		[Test]
		public void ToDto_Success()
		{
			const double tolerance = 0.01;
			var model = Generators.CreateNewResultModel(Guid.NewGuid().ToString("N"));
			var dto = model.ToDto(Generators.CreateNewCpuModel(), Generators.CreateNewTopology());

			Assert.AreEqual(model.Id, dto.Id);
			Assert.AreEqual(model.TimeStamp, dto.TimeStamp);

			Assert.True(Helpers.AssertCollectionsAreEqual(model.Affinity, dto.Affinity, (a, b) => a == b));

			Assert.AreEqual(model.Elpida.Compiler.Name, dto.Elpida.Compiler.Name);
			Assert.AreEqual(model.Elpida.Compiler.Version, dto.Elpida.Compiler.Version);
			Assert.AreEqual(model.Elpida.Version.Build, dto.Elpida.Version.Build);
			Assert.AreEqual(model.Elpida.Version.Major, dto.Elpida.Version.Major);
			Assert.AreEqual(model.Elpida.Version.Minor, dto.Elpida.Version.Minor);
			Assert.AreEqual(model.Elpida.Version.Revision, dto.Elpida.Version.Revision);

			Assert.AreEqual(model.Result.Name, dto.Result.Name);

			Assert.True(Helpers.AssertCollectionsAreEqual(model.Result.TaskResults, dto.Result.TaskResults,
				(a, b) => a.Name == b.Name
				          && a.Description == b.Description
				          && a.Suffix == b.Suffix
				          && Math.Abs(a.Time - b.Time) < tolerance
				          && a.Type == b.Type
				          && Math.Abs(a.Value - b.Value) < tolerance
				          && a.InputSize == b.InputSize));


			Assert.AreEqual(model.System.Memory.PageSize, dto.System.Memory.PageSize);
			Assert.AreEqual(model.System.Memory.TotalSize, dto.System.Memory.TotalSize);

			Assert.AreEqual(model.System.Os.Category, dto.System.Os.Category);
			Assert.AreEqual(model.System.Os.Name, dto.System.Os.Name);
			Assert.AreEqual(model.System.Os.Version, dto.System.Os.Version);
		}
	}
}
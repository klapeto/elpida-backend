// /*
//  * Elpida HTTP Rest API
//  *   
//  * Copyright (C) 2020  Ioannis Panagiotopoulos
//  *
//  * This program is free software: you can redistribute it and/or modify
//  * it under the terms of the GNU Affero General Public License as
//  * published by the Free Software Foundation, either version 3 of the
//  * License, or (at your option) any later version.
//  *
//  * This program is distributed in the hope that it will be useful,
//  * but WITHOUT ANY WARRANTY; without even the implied warranty of
//  * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  * GNU Affero General Public License for more details.
//  *
//  * You should have received a copy of the GNU Affero General Public License
//  * along with this program.  If not, see <https://www.gnu.org/licenses/>.
//  */
//
// using NUnit.Framework;
//
// namespace Elpida.Backend.Services.Tests
// {
// 	public class ResultPreviewModelExtensionsTests
// 	{
// 		[Test]
// 		public void ToDto_Success()
// 		{
// 			var model = Generators.CreateResultPreviewModel(146);
// 			var dto = model.ToDto();
//
// 			Assert.AreEqual(model.Id, dto.Id);
// 			Assert.AreEqual(model.Name, dto.Name);
// 			Assert.AreEqual(model.CpuBrand, dto.CpuBrand);
// 			Assert.AreEqual(model.CpuCores, dto.CpuCores);
// 			Assert.AreEqual(model.CpuFrequency, dto.CpuFrequency);
// 			Assert.AreEqual(model.MemorySize, dto.MemorySize);
// 			Assert.AreEqual(model.OsName, dto.OsName);
// 			Assert.AreEqual(model.OsVersion, dto.OsVersion);
// 			Assert.AreEqual(model.TimeStamp, dto.TimeStamp);
// 			Assert.AreEqual(model.CpuLogicalCores, dto.CpuLogicalCores);
// 			Assert.AreEqual(model.ElpidaVersionBuild, dto.ElpidaVersionBuild);
// 			Assert.AreEqual(model.ElpidaVersionMajor, dto.ElpidaVersionMajor);
// 			Assert.AreEqual(model.ElpidaVersionMinor, dto.ElpidaVersionMinor);
// 			Assert.AreEqual(model.ElpidaVersionRevision, dto.ElpidaVersionRevision);
// 		}
// 	}
// }
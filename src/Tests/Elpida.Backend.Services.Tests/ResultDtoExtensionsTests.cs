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
// using System;
// using System.Linq;
// using Elpida.Backend.Services.Abstractions.Dtos.Result;
// using Elpida.Backend.Services.Extensions.Result;
// using Newtonsoft.Json;
// using NUnit.Framework;
//
// namespace Elpida.Backend.Services.Tests
// {
// 	public class ResultDtoExtensionsTests
// 	{
// 		[Test]
// 		public void ToModelSuccess()
// 		{
// 			const double tolerance = 0.01;
// 			var dto = Generators.CreateNewResultDto();
// 			var model = dto.ToModel(Generators.CreateNewResultModel(41).Benchmark, Generators.CreateNewResultModel(41).Topology, Generators.CreateNewResultModel(41).TaskResults);
//
// 			Assert.AreEqual(model.Id, 0);
// 			Assert.AreEqual(model.TimeStamp, dto.TimeStamp);
//
// 			Assert.AreEqual(model.Affinity, JsonConvert.SerializeObject(dto.Affinity));
//
// 			Assert.AreEqual(model.CompilerName, dto.Elpida.Compiler.Name);
// 			Assert.AreEqual(model.CompilerVersion, dto.Elpida.Compiler.Version);
// 			
// 			Assert.AreEqual(model.ElpidaVersionBuild, dto.Elpida.Version.Build);
// 			Assert.AreEqual(model.ElpidaVersionMajor, dto.Elpida.Version.Major);
// 			Assert.AreEqual(model.ElpidaVersionMinor, dto.Elpida.Version.Minor);
// 			Assert.AreEqual(model.ElpidaVersionRevision, dto.Elpida.Version.Revision);
//
// 			Assert.AreEqual(model.PageSize, dto.System.Memory.PageSize);
// 			Assert.AreEqual(model.MemorySize, dto.System.Memory.TotalSize);
//
// 			Assert.AreEqual(model.OsCategory, dto.System.Os.Category);
// 			Assert.AreEqual(model.OsName, dto.System.Os.Name);
// 			Assert.AreEqual(model.OsVersion, dto.System.Os.Version);
//
// 			Assert.AreEqual(model.JoinOverhead, dto.System.Timing.JoinOverhead);
// 			Assert.AreEqual(model.LockOverhead, dto.System.Timing.LockOverhead);
// 			Assert.AreEqual(model.LoopOverhead, dto.System.Timing.LoopOverhead);
// 			Assert.AreEqual(model.NotifyOverhead, dto.System.Timing.NotifyOverhead);
// 			Assert.AreEqual(model.NowOverhead, dto.System.Timing.NowOverhead);
// 			Assert.AreEqual(model.SleepOverhead, dto.System.Timing.SleepOverhead);
// 			Assert.AreEqual(model.TargetTime, dto.System.Timing.TargetTime);
// 			Assert.AreEqual(model.WakeupOverhead, dto.System.Timing.WakeupOverhead);
//
// 		}
// 	}
// }
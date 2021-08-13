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
// using Elpida.Backend.Data.Abstractions.Models.Cpu;
// using Elpida.Backend.Data.Abstractions.Models.Result;
// using Elpida.Backend.Data.Abstractions.Models.Topology;
// using Elpida.Backend.Services.Extensions.Result;
// using Newtonsoft.Json;
// using NUnit.Framework;
//
// namespace Elpida.Backend.Services.Tests
// {
// 	public class ResultModelExtensionsTests
// 	{
// 		
// 		[Test]
// 		public void ToDto_Success()
// 		{
// 			const double tolerance = 0.01;
// 			var model = Generators.CreateNewResultModel(546456);
// 			var dto = model.ToDto();
//
// 			Assert.AreEqual(model.Id, dto.Id);
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
// 			Assert.AreEqual(model.Benchmark.Name, dto.Result.Name);
// 			Assert.AreEqual(model.Benchmark.Uuid, dto.Result.Uuid);
//
// 			Assert.AreEqual(model.Topology.Cpu.Caches, JsonConvert.SerializeObject(dto.System.Cpu.Caches));
// 			Assert.AreEqual(model.Topology.Cpu.Brand, dto.System.Cpu.Brand);
// 			Assert.AreEqual(model.Topology.Cpu.Frequency, dto.System.Cpu.Frequency);
// 			Assert.AreEqual(model.Topology.Cpu.Smt, dto.System.Cpu.Smt);
// 			Assert.AreEqual(model.Topology.Cpu.Vendor, dto.System.Cpu.Vendor);
// 			Assert.AreEqual(model.Topology.Cpu.Features, JsonConvert.SerializeObject(dto.System.Cpu.Features));
// 			Assert.AreEqual(model.Topology.Cpu.AdditionalInfo, JsonConvert.SerializeObject(dto.System.Cpu.AdditionalInfo));
//
// 			Assert.AreEqual(model.Topology.Root, JsonConvert.SerializeObject(dto.System.Topology.Root));
// 			Assert.AreEqual(model.Topology.TotalDepth, dto.System.Topology.TotalDepth);
// 			Assert.AreEqual(model.Topology.TotalLogicalCores, dto.System.Topology.TotalLogicalCores);
// 			Assert.AreEqual(model.Topology.TotalPhysicalCores, dto.System.Topology.TotalPhysicalCores);
//
// 			Assert.True(Helpers.AssertCollectionsAreEqual(model.TaskResults, dto.Result.TaskResults,
// 				(a, b) => a.Task.Name == b.Name
// 				          && a.Task.Description == b.Description
// 				          && a.Task.InputName == b.Input?.Name
// 				          && a.Task.InputDescription == b.Input?.Description
// 				          && a.Task.InputProperties == JsonConvert.SerializeObject(b.Input?.RequiredProperties)
// 				          && a.Task.OutputName == b.Output?.Name
// 				          && a.Task.OutputDescription == b.Output?.Description
// 				          && a.Task.OutputProperties == JsonConvert.SerializeObject(b.Output?.RequiredProperties)
// 				          && a.Task.ResultName == b.Result.Name
// 				          && a.Task.ResultDescription == b.Result.Description
// 				          && a.Task.ResultUnit == b.Result.Unit
// 				          && a.Task.ResultAggregation == b.Result.Aggregation
// 				          && a.Task.ResultType == b.Result.Type
// 				          && Math.Abs(a.Time - b.Time) < tolerance
// 				          && Math.Abs(a.Value - b.Value) < tolerance
// 				          && a.InputSize == b.InputSize));
//
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
// 		}
// 	}
// }
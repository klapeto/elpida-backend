/*
 * Elpida HTTP Rest API
 *   
 * Copyright (C) 2021  Ioannis Panagiotopoulos
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

using System.Collections.Generic;
using Elpida.Backend.Services.Abstractions.Dtos.Result;
using NUnit.Framework;

namespace Elpida.Backend.Services.Tests
{
	[TestFixture]
	public class IdProviderTests
	{
		[Test]
		public void ForResult_NoSameValues()
		{
			var provider = new IdProvider();

			var result = Generators.CreateNewResultDto();
			
			var a = provider.GetForResult(result);
			var b = provider.GetForResult(result);
			
			Assert.AreNotEqual(a,b);
		}

		[Test]
		public void ForCpu_Success()
		{
			var provider = new IdProvider();

			var cpu = Generators.CreateNewCpuModel().ToDto();
			
			Assert.AreEqual($"{cpu.Vendor}_{cpu.Brand}_{string.Join("_", cpu.AdditionalInfo.Values)}",provider.GetForCpu(cpu));
		}
		
		[Test]
		public void ForCpu_DoNotChangeWhenValueChanges()
		{
			var provider = new IdProvider();

			var cpu = Generators.CreateNewCpuModel().ToDto();

			var original = provider.GetForCpu(cpu);
			
			Assert.AreEqual(original, provider.GetForCpu(cpu));
			
			cpu.Caches = new List<CpuCacheDto>();
			Assert.AreEqual(original, provider.GetForCpu(cpu));
			
			cpu.Frequency = cpu.Frequency + 1;
			Assert.AreEqual(original, provider.GetForCpu(cpu));
			
			cpu.Smt = !cpu.Smt;
			Assert.AreEqual(original, provider.GetForCpu(cpu));
			
			cpu.Features = new List<string>();
			Assert.AreEqual(original, provider.GetForCpu(cpu));
		}

		[Test]
		public void ForTopology_ValueChanges()
		{
			var provider = new IdProvider();

			var cpuId = "LOLOL";

			var topology = Generators.CreateNewTopology().ToDto();;

			var original = provider.GetForTopology(cpuId, topology);
			
			Assert.AreEqual(original, provider.GetForTopology(cpuId, topology));

			topology.TotalDepth = topology.TotalDepth + 1;
			Assert.AreNotEqual(original, provider.GetForTopology(cpuId, topology));
			
			topology.TotalLogicalCores = topology.TotalLogicalCores + 1;
			Assert.AreNotEqual(original, provider.GetForTopology(cpuId, topology));
			
			topology.TotalPhysicalCores = topology.TotalPhysicalCores + 1;
			Assert.AreNotEqual(original, provider.GetForTopology(cpuId, topology));
			
			topology.Root.Name = $"{topology.Root.Name }sdfds";
			Assert.AreNotEqual(original, provider.GetForTopology(cpuId, topology));
		}
	}
}
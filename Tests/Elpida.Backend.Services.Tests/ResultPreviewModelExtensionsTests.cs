using System;
using NUnit.Framework;

namespace Elpida.Backend.Services.Tests
{
	public class ResultPreviewModelExtensionsTests
	{
		[Test]
		public void ToDto_Success()
		{
			var model = Generators.CreateResultPreviewModel(Guid.NewGuid().ToString("N"));
			var dto = model.ToDto();
			
			Assert.AreEqual(model.Id, dto.Id);
			Assert.AreEqual(model.Name, dto.Name);
			Assert.AreEqual(model.CpuBrand, dto.CpuBrand);
			Assert.AreEqual(model.CpuCores, dto.CpuCores);
			Assert.AreEqual(model.CpuFrequency, dto.CpuFrequency);
			Assert.AreEqual(model.MemorySize, dto.MemorySize);
			Assert.AreEqual(model.OsName, dto.OsName);
			Assert.AreEqual(model.OsVersion, dto.OsVersion);
			Assert.AreEqual(model.TimeStamp, dto.TimeStamp);
			Assert.AreEqual(model.CpuLogicalCores, dto.CpuLogicalCores);
			Assert.AreEqual(model.ElpidaVersionBuild, dto.ElpidaVersionBuild);
			Assert.AreEqual(model.ElpidaVersionMajor, dto.ElpidaVersionMajor);
			Assert.AreEqual(model.ElpidaVersionMinor, dto.ElpidaVersionMinor);
			Assert.AreEqual(model.ElpidaVersionRevision, dto.ElpidaVersionRevision);
		}
	}
}
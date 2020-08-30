using System;
using NUnit.Framework;

namespace Elpida.Backend.Services.Tests
{
	public class ResultModelExtensionsTests
	{
		[Test]
		public void ToDtoSuccess()
		{
			var model = Generators.CreateNewResultModel(Guid.NewGuid().ToString("N"));
			var dto = model.ToResultDto();
			
			Assert.AreEqual(model.Id, dto.Id);
			Assert.AreEqual(model.TimeStamp, dto.TimeStamp);
		}
	}
}
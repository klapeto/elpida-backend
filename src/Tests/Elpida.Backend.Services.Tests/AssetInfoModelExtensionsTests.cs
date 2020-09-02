using System;
using Elpida.Backend.Data.Abstractions.Models;
using NUnit.Framework;

namespace Elpida.Backend.Services.Tests
{
	public class AssetInfoModelExtensionsTests
	{
		[Test]
		public void ToDto_Null_ThrowsArgumentNullException()
		{
			Assert.Throws<ArgumentNullException>(() => ((AssetInfoModel) null).ToDto());
		}

		[Test]
		public void ToDto_Success()
		{
			var model = Generators.CreateAssetInfoModel();
			var dto = model.ToDto();

			Assert.AreEqual(model.Filename, dto.Filename);
			Assert.AreEqual(model.Location.ToString(), dto.Location.ToString());
			Assert.AreEqual(model.Md5, dto.Md5);
			Assert.AreEqual(model.Size, dto.Size);
		}
	}
}
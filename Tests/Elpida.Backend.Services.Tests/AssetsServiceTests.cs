using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Elpida.Backend.Data.Abstractions;
using Elpida.Backend.Data.Abstractions.Models;
using Moq;
using NUnit.Framework;

namespace Elpida.Backend.Services.Tests
{
	public class AssetsServiceTests
	{
		[Test]
		public void Constructor_NullArgument_ThrowsArgumentNullException()
		{
			Assert.Throws<ArgumentNullException>(() => new AssetsService(null));
		}

		[Test]
		[TestCase(null)]
		[TestCase("")]
		public void CreateAsync_InvalidFilename_ThrowsArgumentException(string filename)
		{
			var repoMock = new Mock<IAssetsRepository>(MockBehavior.Strict);
			
			var srv = new AssetsService(repoMock.Object);

			Assert.ThrowsAsync<ArgumentException>(async () => await srv.CreateAsync(null, Stream.Null));

			repoMock.VerifyAll();
			repoMock.VerifyNoOtherCalls();
		}
		
		[Test]
		public void CreateAsync_NullInputData_ThrowsArgumentNullException()
		{
			var repoMock = new Mock<IAssetsRepository>(MockBehavior.Strict);
			
			var srv = new AssetsService(repoMock.Object);

			Assert.ThrowsAsync<ArgumentNullException>(async () => await srv.CreateAsync("Test", null));

			repoMock.VerifyAll();
			repoMock.VerifyNoOtherCalls();
		}
		
		[Test]
		public async Task GetAssetsAsync_Success()
		{
			var repoMock = new Mock<IAssetsRepository>(MockBehavior.Strict);
			
			var assetModels = new []
			{
				new AssetInfoModel
				{
					Filename = "Test1",
					Location = new Uri("https://beta.elpida.dev"),
					Md5 = "0000000",
					Size = 5
				}, 
			};
			
			repoMock.Setup(r => r.GetAssetsAsync( default))
				.Returns(() => Task.FromResult<IEnumerable<AssetInfoModel>>(assetModels))
				.Verifiable();

			var srv = new AssetsService(repoMock.Object);

			var result = (await srv.GetAssetsAsync()).ToArray();

			Assert.AreEqual(assetModels.Length, result.Length);
			
			repoMock.VerifyAll();
			repoMock.VerifyNoOtherCalls();
		}
		
		[Test]
		public async Task CreateAsync_Success()
		{
			var repoMock = new Mock<IAssetsRepository>(MockBehavior.Strict);

			const string filename = "test.exe";
			var uri = new Uri("https://beta.elpida.dev");

			repoMock.Setup(r => r.CreateAsync(filename, Stream.Null, default))
				.Returns(() => Task.FromResult(uri))
				.Verifiable();

			var srv = new AssetsService(repoMock.Object);

			var result = await srv.CreateAsync(filename, Stream.Null);

			Assert.AreEqual(uri.ToString(), result.ToString());
			
			repoMock.VerifyAll();
			repoMock.VerifyNoOtherCalls();
		}
	}
}
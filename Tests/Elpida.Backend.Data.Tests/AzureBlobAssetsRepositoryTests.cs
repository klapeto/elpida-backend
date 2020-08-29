using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Elpida.Backend.Data.Tests.Dummies;
using Moq;
using NUnit.Framework;

namespace Elpida.Backend.Data.Tests
{
	public class AzureBlobAssetsRepositoryTests
	{
		// Some of these Unit tests are retarded. I just wanted to get maximum coverage but...

		[Test]
		public void CreateAsync_NullInputData_ThrowsArgumentNullException()
		{
			var containerClient = new Mock<BlobContainerClient>(MockBehavior.Strict);
			var client = new Mock<BlobClient>(MockBehavior.Strict);
			var repo = new AzureBlobsAssetsRepository(new MockWrapperFactory(client.Object, containerClient.Object));

			Assert.ThrowsAsync<ArgumentNullException>(async () => await repo.CreateAsync("PlaceHolder", null));
		}

		[Test]
		public void CreateAsync_NullFilename_ThrowsArgumentException()
		{
			var containerClient = new Mock<BlobContainerClient>(MockBehavior.Strict);
			var client = new Mock<BlobClient>(MockBehavior.Strict);
			var repo = new AzureBlobsAssetsRepository(new MockWrapperFactory(client.Object, containerClient.Object));

			Assert.ThrowsAsync<ArgumentException>(async () => await repo.CreateAsync(null, Stream.Null));
		}

		[Test]
		public void CreateAsync_EmptyFilename_ThrowsArgumentException()
		{
			var containerClient = new Mock<BlobContainerClient>(MockBehavior.Strict);
			var client = new Mock<BlobClient>(MockBehavior.Strict);
			var repo = new AzureBlobsAssetsRepository(new MockWrapperFactory(client.Object, containerClient.Object));

			Assert.ThrowsAsync<ArgumentException>(async () => await repo.CreateAsync(string.Empty, Stream.Null));
		}

		[Test]
		public void CreateAsync_WhiteSpaceFilename_ThrowsArgumentException()
		{
			var containerClient = new Mock<BlobContainerClient>(MockBehavior.Strict);
			var client = new Mock<BlobClient>(MockBehavior.Strict);
			var repo = new AzureBlobsAssetsRepository(new MockWrapperFactory(client.Object, containerClient.Object));

			Assert.ThrowsAsync<ArgumentException>(async () => await repo.CreateAsync("\n\t  \t \n", Stream.Null));
		}

		[Test]
		public void CreateAsync_InvalidFilename_ThrowsArgumentException()
		{
			var containerClient = new Mock<BlobContainerClient>(MockBehavior.Strict);
			var client = new Mock<BlobClient>(MockBehavior.Strict);
			var repo = new AzureBlobsAssetsRepository(new MockWrapperFactory(client.Object, containerClient.Object));

			Assert.ThrowsAsync<ArgumentException>(async () => await repo.CreateAsync("/haha/lol", Stream.Null));
		}

		[Test]
		public void Constructor_NullArgument_ThrowsArgumentNullException()
		{
			Assert.Throws<ArgumentNullException>(() => new AzureBlobsAssetsRepository(null));
		}

		[Test]
		public async Task CreateAsync_Success()
		{
			var containerClient = new Mock<BlobContainerClient>(MockBehavior.Strict);
			var client = new Mock<BlobClient>(MockBehavior.Strict);

			var uri = new Uri("https://beta.elpida.dev");

			client.Setup(c => c.UploadAsync(Stream.Null, true, It.IsAny<CancellationToken>()))
				.ReturnsAsync(Response.FromValue((BlobContentInfo) null, null))
				.Verifiable();
			client.SetupGet(c => c.Uri)
				.Returns(uri)
				.Verifiable();

			var repo = new AzureBlobsAssetsRepository(new MockWrapperFactory(client.Object, containerClient.Object));
			var res = await repo.CreateAsync("haha", Stream.Null);

			Assert.AreEqual(uri.ToString(), res.ToString());

			client.VerifyAll();
			client.VerifyNoOtherCalls();

			containerClient.VerifyAll();
			containerClient.VerifyNoOtherCalls();
		}

		private static BlobItem CreateBlobItem(string name, byte[] md5, long size)
		{
			// Level of Magic: Voldermort
			// Retarded tests, Just for fun and to squize all possible coverage

			var blobType = typeof(BlobItem);
			var propertiesType = typeof(BlobItemProperties);

			var blob = blobType.Assembly.CreateInstance(blobType.FullName, false,
				BindingFlags.Instance | BindingFlags.NonPublic, null, null, null, null);
			blobType.GetProperty(nameof(BlobItem.Name)).SetValue(blob, name);


			var props = propertiesType.Assembly.CreateInstance(propertiesType.FullName, false,
				BindingFlags.Instance | BindingFlags.NonPublic, null, null, null, null);
			propertiesType.GetProperty(nameof(BlobItemProperties.ContentHash)).SetValue(props, md5);
			propertiesType.GetProperty(nameof(BlobItemProperties.ContentLength)).SetValue(props, size);

			blobType.GetProperty(nameof(BlobItem.Properties)).SetValue(blob, props);
			return (BlobItem) blob;
		}


		[Test]
		public async Task GetAssetsAsync_Success()
		{
			var containerClient = new Mock<BlobContainerClient>(MockBehavior.Strict);
			var client = new Mock<BlobClient>(MockBehavior.Strict);

			containerClient.Setup(c => c.GetBlobsAsync(It.IsAny<BlobTraits>(), It.IsAny<BlobStates>(),
					It.IsAny<string>(), It.IsAny<CancellationToken>()))
				.Returns(() => new DummyAsyncPageable<BlobItem>(new[] {CreateBlobItem("WTF", new byte[] {255}, 255)}))
				.Verifiable();

			containerClient.SetupGet(c => c.Uri)
				.Returns(new Uri("https://beta.elpida.dev"))
				.Verifiable();

			var repo = new AzureBlobsAssetsRepository(new MockWrapperFactory(client.Object, containerClient.Object));
			await repo.GetAssetsAsync();

			client.VerifyAll();
			client.VerifyNoOtherCalls();

			containerClient.VerifyAll();
			containerClient.VerifyNoOtherCalls();
		}

		#region Nested type: MockWrapperFactory

		private class MockWrapperFactory : IBlobClientFactory
		{
			private readonly BlobClient _blobClient;
			private readonly BlobContainerClient _blobContainerClient;

			public MockWrapperFactory(BlobClient blobClient, BlobContainerClient blobContainerClient)
			{
				_blobClient = blobClient;
				_blobContainerClient = blobContainerClient;
			}

			#region IBlobClientFactory Members

			public BlobClient CreateClient(string filename)
			{
				return _blobClient;
			}

			public BlobContainerClient CreateContainerClient()
			{
				return _blobContainerClient;
			}

			#endregion
		}

		#endregion
	}
}
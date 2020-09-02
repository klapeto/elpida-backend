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
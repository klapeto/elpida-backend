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
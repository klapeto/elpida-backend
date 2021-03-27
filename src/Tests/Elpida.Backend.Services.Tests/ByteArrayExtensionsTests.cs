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

using Elpida.Backend.Services.Extensions;
using NUnit.Framework;

namespace Elpida.Backend.Services.Tests
{
	[TestFixture]
	public class ByteArrayExtensionsTests
	{
		[Test]
		public void Success()
		{
			Assert.AreEqual("3112FCA8571B", new byte[] {0x31, 0x12, 0xFC, 0xA8, 0x57, 0x1B}.ToHexString());
			
			Assert.AreEqual("CD84ACF04891", new byte[] {0xCD, 0x84, 0xAC, 0xF0, 0x48, 0x91}.ToHexString());
		}
	}
}
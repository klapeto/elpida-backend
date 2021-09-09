// =========================================================================
//
// Elpida HTTP Rest API
//
// Copyright (C) 2021 Ioannis Panagiotopoulos
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as
// published by the Free Software Foundation, either version 3 of the
// License, or (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Affero General Public License for more details.
//
// You should have received a copy of the GNU Affero General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
// =========================================================================

using System;
using Elpida.Backend.Common.Exceptions;
using NUnit.Framework;

namespace Elpida.Backend.Common.Tests
{
	[TestFixture]
	internal class NotFoundExceptionTests
	{
		[Test]
		public void Id_Valid()
		{
			var id = Guid.NewGuid();
			var ex = new NotFoundException("This item was not found", id);

			Assert.AreEqual(id.ToString(), ex.Id);
		}

		[Test]
		public void Id_Long_Valid()
		{
			const long id = 512L;
			var ex = new NotFoundException("This item was not found", id);

			Assert.AreEqual(id.ToString(), ex.Id);
		}
	}
}
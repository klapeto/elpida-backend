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
using Elpida.Backend.Services.Abstractions;
using NUnit.Framework;

namespace Elpida.Backend.Services.Tests
{
	[TestFixture]
	public class NonDefaultValueAttributeTests
	{
		[Test]
		[TestCase(default(int))]
		[TestCase(default(long))]
		[TestCase(default(double))]
		[TestCase(default(string))]
		[TestCase(default(bool))]
		[TestCase(default(object))]
		public void IsValid_DefaultValues_False(object? obj)
		{
			Assert.False(new NonDefaultValueAttribute().IsValid(obj));
		}

		[Test]
		public void IsValid_DefaultGuid_False()
		{
			Assert.False(new NonDefaultValueAttribute().IsValid(Guid.Empty));
		}

		[Test]
		public void IsValid_DefaultDateTime_False()
		{
			Assert.False(new NonDefaultValueAttribute().IsValid(new DateTime()));
		}

		[Test]
		public void IsValid_NonDefaultDateTime_True()
		{
			Assert.True(new NonDefaultValueAttribute().IsValid(DateTime.UtcNow));
		}

		[Test]
		public void IsValid_NonDefaultGuid_True()
		{
			Assert.True(new NonDefaultValueAttribute().IsValid(Guid.NewGuid()));
		}

		[Test]
		public void IsValid_NonDefaultValue_True()
		{
			Assert.True(new NonDefaultValueAttribute().IsValid("LOOL"));
		}
	}
}
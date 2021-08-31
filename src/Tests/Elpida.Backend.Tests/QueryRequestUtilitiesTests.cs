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
using System.Linq;
using System.Text.Json;
using Elpida.Backend.Services.Abstractions;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Elpida.Backend.Tests
{
	[TestFixture]
	public class QueryRequestUtilitiesTests
	{
		[Test]
		[TestCase(5)]
		[TestCase("lol")]
		[TestCase(5.1f)]
		[TestCase(5.1d)]
		[TestCase(4L)]
		[TestCase(9UL)]
		[TestCase(7U)]
		[TestCase('c')]
		[TestCase(true)]
		[TestCase(false)]
		[TestCase((byte)12)]
		[TestCase((sbyte)12)]
		[TestCase((short)12)]
		[TestCase((ushort)12)]
		[TestCase(null)]
		public void LiteralValue_DoesNotChange(object value)
		{
			var request = new QueryRequest(
				new PageRequest(10, 10, 0),
				new FilterInstance[] { new ("test", value, "equal") },
				"data",
				true
			);

			var newRequest = QueryRequestUtilities.PreProcessQuery(request);

			Assert.AreEqual(value, newRequest.Filters.First().Value);
		}

		[Test]
		public void DateValue_ConvertsValue()
		{
			var obj = DateTime.UtcNow;
			var serialized = JsonConvert.SerializeObject(obj.ToString("O"));
			var document = JsonDocument.Parse(serialized);

			var request = new QueryRequest(
				new PageRequest(10, 10, 0),
				new FilterInstance[] { new ("test", document.RootElement, "equal") },
				"data",
				true
			);

			var newRequest = QueryRequestUtilities.PreProcessQuery(request);

			Assert.AreEqual(obj, newRequest.Filters.First().Value);
		}

		[Test]
		[TestCase(45)]
		[TestCase(456.0)]
		[TestCase(true)]
		[TestCase(false)]
		[TestCase("LOL")]
		public void LiteralValue_ConvertsValue(object obj)
		{
			var serialized = JsonConvert.SerializeObject(obj);
			var document = JsonDocument.Parse(serialized);

			var request = new QueryRequest(
				new PageRequest(10, 10, 0),
				new FilterInstance[] { new ("test", document.RootElement, "equal") },
				"data",
				true
			);

			var newRequest = QueryRequestUtilities.PreProcessQuery(request);

			Assert.AreEqual(obj, newRequest.Filters.First().Value);
		}

		[Test]
		public void NullValue_ThrowsArgumentException()
		{
			var serialized = JsonConvert.SerializeObject(null);
			var document = JsonDocument.Parse(serialized);

			var request = new QueryRequest(
				new PageRequest(10, 10, 0),
				new FilterInstance[] { new ("test", document.RootElement, "equal") },
				"data",
				true
			);

			Assert.Throws<ArgumentException>(() => QueryRequestUtilities.PreProcessQuery(request));
		}

		[Test]
		public void NullFilters_DoesNothing()
		{
			var request = new QueryRequest(
				new PageRequest(10, 10, 0),
				null,
				"data",
				true
			);

			var newRequest = QueryRequestUtilities.PreProcessQuery(request);

			Assert.Null(newRequest.Filters);
		}
	}
}
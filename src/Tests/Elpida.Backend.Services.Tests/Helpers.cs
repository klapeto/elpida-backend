using System;
using System.Collections.Generic;
using System.Linq;
using Elpida.Backend.Data.Abstractions.Models.Result;
using Elpida.Backend.Services.Abstractions.Dtos.Result;
using NUnit.Framework;

namespace Elpida.Backend.Services.Tests
{
	public static class Helpers
	{
		public static void AssertTopology(CpuNodeModel model, CpuNodeDto dto)
		{
			Assert.AreEqual(model.Name, dto.Name);
			Assert.AreEqual(model.Value, dto.Value);
			Assert.AreEqual(model.NodeType, dto.NodeType);
			Assert.AreEqual(model.OsIndex, dto.OsIndex);

			AssertCollectionsAreEqual(model.MemoryChildren, dto.MemoryChildren, (a, b) =>
			{
				AssertTopology(a, b);
				return true;
			});

			AssertCollectionsAreEqual(model.Children, dto.Children, (a, b) =>
			{
				AssertTopology(a, b);
				return true;
			});
		}

		public static bool AssertCollectionsAreEqual<T, R>(IEnumerable<T> ea, IEnumerable<R> eb,
			Func<T, R, bool> assertion)
		{
			var a = ea.ToArray();
			var b = eb.ToArray();

			Assert.True(a.Length == b.Length);

			// ReSharper disable once LoopCanBeConvertedToQuery
			for (var i = 0; i < a.Length; i++)
			{
				if (!assertion(a[i], b[i])) return false;
			}

			return true;
		}
	}
}
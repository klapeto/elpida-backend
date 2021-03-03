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
using System.Linq;
using Elpida.Backend.Data.Abstractions.Models.Result;
using Elpida.Backend.Services.Abstractions.Dtos.Result;
using NUnit.Framework;

namespace Elpida.Backend.Services.Tests
{
	public static class Helpers
	{
		public static bool AreEqual(TaskOutlierModel model, TaskOutlierDto dto, double tolerance)
		{
			return Math.Abs(model.Time - dto.Time) < tolerance
			       && Math.Abs(model.Value - dto.Value) < tolerance;
		}
		
		public static bool AreEqual(TaskStatisticsModel model, TaskStatisticsDto dto, double tolerance)
		{
			return Math.Abs(model.Max - dto.Max) < tolerance
			       && Math.Abs(model.Min - dto.Min) < tolerance
			       && Math.Abs(model.Mean - dto.Mean) < tolerance
			       && Math.Abs(model.Sd - dto.Sd) < tolerance
			       && Math.Abs(model.Tau - dto.Tau) < tolerance
			       && Math.Abs(model.SampleSize - dto.SampleSize) < tolerance
			       && Math.Abs(model.MarginOfError - dto.MarginOfError) < tolerance;
		}

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
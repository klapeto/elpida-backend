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
using System.Collections.Generic;
using System.Linq;

namespace Elpida.Backend.Services.Utilities
{
	public static class StatisticsHelpers
	{
		private static readonly (long SampleSize, double Tau)[] Taus =
		{
			(3, 1.1511), (4, 1.4250), (5, 1.5712),
			(6, 1.6563), (7, 1.7110), (8, 1.7491),
			(9, 1.7770), (10, 1.7984), (11, 1.8153),
			(12, 1.8290), (13, 1.8403), (14, 1.8498),
			(15, 1.8579), (16, 1.8649), (17, 1.8710),
			(18, 1.8764), (19, 1.8811), (20, 1.8853),
			(21, 1.8891), (22, 1.8926), (23, 1.8957),
			(24, 1.8985), (25, 1.9011), (26, 1.9035),
			(27, 1.9057), (28, 1.9078), (29, 1.9096),
			(30, 1.9114), (31, 1.9130), (32, 1.9146),
			(33, 1.9160), (34, 1.9174), (35, 1.9186),
			(36, 1.9198), (37, 1.9209), (38, 1.9220),
			(39, 1.9230), (40, 1.9240), (42, 1.9257),
			(44, 1.9273), (46, 1.9288), (48, 1.9301),
			(50, 1.9314), (55, 1.9340), (60, 1.9362),
			(65, 1.9381), (70, 1.9397), (80, 1.9423),
			(90, 1.9443), (100, 1.9459), (200, 1.9530),
			(500, 1.9572), (1000, 1.9586), (5000, 1.9597),
		};

		public static double CalculateTau(long sampleSize)
		{
			// From the same algorithm used in Elpida cpp
			long tableSize = Taus.Length;
			var i = tableSize / 2;

			long j = 4;

			while (true)
			{
				if (i == 0)
				{
					return Taus[0].Tau;
				}

				if (i >= tableSize)
				{
					return Taus[tableSize - 1].Tau;
				}

				if (i < tableSize - 1
				    && Taus[i].SampleSize <= sampleSize
				    && Taus[i + 1].SampleSize > sampleSize)
				{
					return Taus[i].Tau;
				}

				if (Taus[i].SampleSize < sampleSize)
				{
					i += Math.Max(1, tableSize / j);
				}
				else
				{
					i -= Math.Max(1, tableSize / j);
				}

				j <<= 1;
			}
		}

		public static double CalculateArithmeticMean<T>(ICollection<T> population, Func<T, double> getter)
		{
			return population.Sum(getter) / population.Count;
		}

		public static double CalculateHarmonicMean<T>(ICollection<T> population, Func<T, double> getter)
		{
			return population.Count / population.Sum(sample => 1.0 / getter(sample));
		}

		public static double CalculateMarginOfError(double standardDeviation, long size)
		{
			return standardDeviation / Math.Sqrt(size);
		}

		public static double CalculateStandardDeviation<T>(ICollection<T> population, Func<T, double> getter)
		{
			var mean = CalculateArithmeticMean(population, getter);

			var deviation = population
				                .Sum(sample => Math.Pow(getter(sample) - mean, 2.0))
			                / population.Count;

			return Math.Sqrt(deviation);
		}
	}
}
using System.Collections.Generic;
using System.Globalization;

namespace Elpida.Web.Frontend.Services
{
	public static class ValueConverterService
	{
		private static readonly string[] PrefixesSI =
		{
			"p",
			"n",
			"μ",
			"m",
			string.Empty,
			"K",
			"M",
			"G",
			"T",
			"P",
			"E",
			"Z",
			"Y",
		};

		private static readonly double[] ScaleValuesSI =
		{
			1.0 / 1000.0 / 1000.0 / 1000.0 / 1000.0,
			1.0 / 1000.0 / 1000.0 / 1000.0,
			1.0 / 1000.0 / 1000.0,
			1.0 / 1000.0,
			1.0,
			1000.0,
			1000.0 * 1000.0,
			1000.0 * 1000.0 * 1000.0,
			1000.0 * 1000.0 * 1000.0 * 1000.0,
			1000.0 * 1000.0 * 1000.0 * 1000.0 * 1000.0,
			1000.0 * 1000.0 * 1000.0 * 1000.0 * 1000.0 * 1000.0,
			1000.0 * 1000.0 * 1000.0 * 1000.0 * 1000.0 * 1000.0 * 1000.0,
			1000.0 * 1000.0 * 1000.0 * 1000.0 * 1000.0 * 1000.0 * 1000.0 * 1000.0,
		};

		public static (double Scale, string Suffix) GetValueScaleSI(double value)
		{
			return GetValueScale(value, ScaleValuesSI, PrefixesSI);
		}

		public static (string Value, string Suffix) ConvertToSI(double value, int decimals = 2)
		{
			return GetValueScaleStringImpl(value, ScaleValuesSI, PrefixesSI, decimals);
		}

		private static (string Value, string Suffix) GetValueScaleStringImpl(
			double value,
			IReadOnlyList<double> denominators,
			IReadOnlyList<string> prefixes,
			int decimals
		)
		{
			if (value == 0)
			{
				return (value.ToString(CultureInfo.InvariantCulture), string.Empty);
			}

			var (scale, suffix) = GetValueScale(value, denominators, prefixes);

			if (scale <= 1.0)
			{
				decimals = 0; // if value is under 1000 do not show decimals
			}

			return ((value / scale).ToString($"F{decimals}"), suffix);
		}

		private static (double Scale, string Suffix) GetValueScale(
			double value,
			IReadOnlyList<double> denominators,
			IReadOnlyList<string> prefixes
		)
		{
			if (value == 0)
			{
				return (1.0, string.Empty);
			}

			var i = prefixes.Count - 1;
			while (i > 0)
			{
				if (value >= denominators[i])
				{
					break;
				}

				i--;
			}

			return (denominators[i], prefixes[i]);
		}
	}
}
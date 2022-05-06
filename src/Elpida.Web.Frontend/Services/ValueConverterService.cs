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

		private static readonly string[] PrefixesIEC =
		{
			string.Empty,
			"Ki",
			"Mi",
			"Gi",
			"Ti",
			"Pi",
			"Ei",
			"Zi",
			"Yi",
		};

		private static readonly double[] ScaleValuesIEC =
		{
			1.0,
			1024.0,
			1024.0 * 1024.0,
			1024.0 * 1024.0 * 1024.0,
			1024.0 * 1024.0 * 1024.0 * 1024.0,
			1024.0 * 1024.0 * 1024.0 * 1024.0 * 1024.0,
			1024.0 * 1024.0 * 1024.0 * 1024.0 * 1024.0 * 1024.0,
			1024.0 * 1024.0 * 1024.0 * 1024.0 * 1024.0 * 1024.0 * 1024.0,
			1024.0 * 1024.0 * 1024.0 * 1024.0 * 1024.0 * 1024.0 * 1024.0 * 1024.0,
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

		public static (string Value, string Suffix) ConvertToIEC(double value, int decimals = 2)
		{
			return GetValueScaleStringImpl(value, ScaleValuesIEC, PrefixesIEC, decimals);
		}

		public static string ConvertToStringIEC(double value, int decimals = 2, bool spaceBetween = false)
		{
			var (newValue, suffix) = ConvertToIEC(value, decimals);
			return $"{newValue}{(spaceBetween ? " " : string.Empty)}{suffix}";
		}

		public static string ConvertToStringSI(double value, int decimals = 2, bool spaceBetween = false)
		{
			var (newValue, suffix) = ConvertToSI(value, decimals);
			return $"{newValue}{(spaceBetween ? " " : string.Empty)}{suffix}";
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
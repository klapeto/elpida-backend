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

using System.Collections.Generic;

namespace Elpida.Backend.Services
{
	public static class ByteArrayExtensions
	{
		private static readonly uint[] Lookup32 = CreateLookup32();

		public static string ToHexString(this IReadOnlyList<byte> array)
		{
			return ByteArrayToHexViaLookup32(array);
		}

		private static uint[] CreateLookup32()
		{
			var result = new uint[256];
			for (var i = 0; i < 256; i++)
			{
				var s = i.ToString("X2");
				result[i] = s[0] + ((uint) s[1] << 16);
			}

			return result;
		}

		private static string ByteArrayToHexViaLookup32(IReadOnlyList<byte> bytes)
		{
			//https://stackoverflow.com/questions/311165/how-do-you-convert-a-byte-array-to-a-hexadecimal-string-and-vice-versa/24343727#24343727
			var lookup32 = Lookup32;
			var result = new char[bytes.Count * 2];
			for (var i = 0; i < bytes.Count; i++)
			{
				var val = lookup32[bytes[i]];
				result[2 * i] = (char) val;
				result[2 * i + 1] = (char) (val >> 16);
			}

			return new string(result);
		}
	}
}
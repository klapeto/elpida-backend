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

using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Elpida.Backend.Common.Extensions
{
	public static class StringExtensions
	{
		public static string ToHashString(this string str)
		{
			using var md5 = MD5.Create();
			using var ms = new MemoryStream(Encoding.UTF8.GetBytes(str));

			return md5.ComputeHash(ms).ToHexString();
		}
	}
}
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
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using Elpida.Backend.Services.Abstractions;
using Elpida.Backend.Services.Abstractions.Dtos.Cpu;
using Elpida.Backend.Services.Abstractions.Dtos.Result;
using Elpida.Backend.Services.Abstractions.Dtos.Topology;
using Elpida.Backend.Services.Abstractions.Interfaces;
using Elpida.Backend.Services.Extensions;

namespace Elpida.Backend.Services
{
	public class IdProvider : IIdProvider
	{
		#region IIdProvider Members

		public string GetForCpu(CpuDto cpuDto)
		{
			var builder = new StringBuilder();

			builder.Append(cpuDto.Vendor);
			builder.Append('_');
			builder.Append(cpuDto.Brand);

			foreach (var infoPair in cpuDto.AdditionalInfo.
				ToArray()
				.OrderBy(c => c.Key))
			{
				builder.Append('_');
				builder.Append(infoPair.Value);
			}

			return builder.ToString();
		}

		public string GetForTopology(string cpuId, TopologyDto topologyDto)
		{
			using var stream = new MemoryStream();

			var formatter = new BinaryFormatter();
			formatter.Serialize(stream, topologyDto);
			stream.Position = 0;

			using var md5 = MD5.Create();
			return $"{cpuId}_{md5.ComputeHash(stream).ToHexString()}";
		}

		public string GetForResult(ResultDto resultDto)
		{
			return Guid.NewGuid().ToString("N");
		}

		#endregion
	}
}
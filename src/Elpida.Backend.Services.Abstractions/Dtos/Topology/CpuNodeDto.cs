/*
 * Elpida HTTP Rest API
 *
 * Copyright (C) 2020 Ioannis Panagiotopoulos
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

namespace Elpida.Backend.Services.Abstractions.Dtos.Topology
{
	public enum ProcessorNodeType
	{
		Machine,
		Package,
		NumaNode,
		Group,
		Die,
		Core,
		L1ICache,
		L1DCache,
		L2ICache,
		L2DCache,
		L3ICache,
		L3DCache,
		L4Cache,
		L5Cache,
		ExecutionUnit,
		Unknown,
	}

	/// <summary>
	///     Details of a cpu node.
	/// </summary>
	[Serializable]
	public class CpuNodeDto
	{
		/// <summary>
		///     The cpu node type.
		/// </summary>
		public ProcessorNodeType NodeType { get; set; }

		/// <summary>
		///     The name of this node.
		/// </summary>
		/// <example>Core</example>
		public string Name { get; set; } = string.Empty;

		/// <summary>
		///     The index assigned by the Operating System.
		/// </summary>
		public long? OsIndex { get; set; }

		/// <summary>
		///     A value representing the size of the node.
		/// </summary>
		public long? Value { get; set; }

		/// <summary>
		///     The child nodes of this node.
		/// </summary>
		public IList<CpuNodeDto>? Children { get; set; }

		/// <summary>
		///     The memory child nodes of this node.
		/// </summary>
		public IList<CpuNodeDto>? MemoryChildren { get; set; }
	}
}
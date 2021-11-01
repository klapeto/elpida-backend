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
using System.ComponentModel.DataAnnotations;

namespace Elpida.Backend.Services.Abstractions.Dtos.Topology
{
	/// <summary>
	///     Details of a cpu node.
	/// </summary>
	public sealed class CpuNodeDto
	{
		/// <summary>
		///     The cpu node type.
		/// </summary>
		[Required]
		[EnumDataType(typeof(ProcessorNodeType))]
		public ProcessorNodeType NodeType { get; init; }

		/// <summary>
		///     The name of this node.
		/// </summary>
		/// <example>Core</example>
		[Required]
		[MaxLength(50)]
		public string Name { get; init; }

		/// <summary>
		///     The index assigned by the Operating System.
		/// </summary>
		public long? OsIndex { get; init; }

		/// <summary>
		///     A value representing the size of the node.
		/// </summary>
		[Range(0, long.MaxValue)]
		public long? Value { get; set; }

		/// <summary>
		///     The child nodes of this node.
		/// </summary>
		public CpuNodeDto[]? Children { get; init; }

		/// <summary>
		///     The memory child nodes of this node.
		/// </summary>
		public CpuNodeDto[]? MemoryChildren { get; init; }

		public void SanitizeValues()
		{
			switch (NodeType)
			{
				case ProcessorNodeType.L1DCache:
				case ProcessorNodeType.L1ICache:
				case ProcessorNodeType.L2DCache:
				case ProcessorNodeType.L2ICache:
				case ProcessorNodeType.L3DCache:
				case ProcessorNodeType.L4Cache:
				case ProcessorNodeType.L5Cache:
					break;
				default:
					Value = null;
					break;
			}

			if (Children != null)
			{
				foreach (var child in Children)
				{
					child.SanitizeValues();
				}
			}

			if (MemoryChildren == null)
			{
				return;
			}

			{
				foreach (var child in MemoryChildren)
				{
					child.SanitizeValues();
				}
			}

		}
	}
}
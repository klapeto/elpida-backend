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
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Elpida.Backend.DataSeed
{
	internal static class ParallelExecutor
	{
		public static Task ParallelExecAsync<T>(
			IEnumerable<T> enumerable,
			Func<T, CancellationToken, Task> callback,
			CancellationToken cancellationToken = default
		)
		{
			return Task.WhenAll(
				enumerable
					.Select(item => callback(item, cancellationToken))
					.ToArray()
			);
		}

		public static Task ProcessFilesInDirectoryAsync<T>(
			string directory,
			IServiceProvider serviceProvider,
			Func<IServiceProvider, T, CancellationToken, Task> itemProcessor,
			CancellationToken cancellationToken = default
		)
		{
			return ParallelExecAsync(
				Directory.EnumerateFiles(directory),
				async (file, ct) =>
				{
					var data = JsonConvert.DeserializeObject<T?>(await File.ReadAllTextAsync(file, ct));

					if (data != null)
					{
						await ProcessItemsAsync(new[] { data }, serviceProvider, itemProcessor, ct);
					}
				},
				cancellationToken
			);
		}

		public static Task ProcessItemsAsync<T>(
			IEnumerable<T> data,
			IServiceProvider serviceProvider,
			Func<IServiceProvider, T, CancellationToken, Task> itemProcessor,
			CancellationToken cancellationToken = default
		)
		{
			return ParallelExecAsync(
				data,
				async (item, token) =>
				{
					using var scope = serviceProvider.CreateScope();
					await itemProcessor(scope.ServiceProvider, item, token);
				},
				cancellationToken
			);
		}
	}
}
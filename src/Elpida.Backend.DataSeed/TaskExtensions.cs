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

using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Elpida.Backend.DataSeed
{
	internal static class TaskExtensions
	{
		public static async Task TimeProcedure(this Task operation, ILogger logger, string name)
		{
			using (logger.BeginScope("Operation: '{Name}'", name))
			{
				logger.LogInformation("Operation started");

				var stopWatch = Stopwatch.StartNew();

				await operation;

				stopWatch.Stop();

				logger.LogInformation("Operation completed: {Time}", stopWatch.Elapsed);
			}
		}
	}
}
/*
 * Elpida HTTP Rest API
 *   
 * Copyright (C) 2021 Ioannis Panagiotopoulos
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

using Elpida.Backend.Data.Abstractions.Models;
using Elpida.Backend.Services.Abstractions.Dtos.Result;

namespace Elpida.Backend.Services.Extensions
{
    public static class OsExtensions
    {
        public static OsModel ToModel(this OsDto osDto)
        {
            return new OsModel
            {
                Id = osDto.Id,
                Category = osDto.Category,
                Name = osDto.Name,
                Version = osDto.Version
            };
        }

        public static OsDto ToDto(this OsModel osModel)
        {
            return new OsDto
            {
                Id = osModel.Id,
                Category = osModel.Category,
                Name = osModel.Name,
                Version = osModel.Version
            };
        }
    }
}
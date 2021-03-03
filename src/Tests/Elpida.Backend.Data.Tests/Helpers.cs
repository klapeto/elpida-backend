/*
 * Elpida HTTP Rest API
 *   
 * Copyright (C) 2021  Ioannis Panagiotopoulos
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
using System.Linq.Expressions;
using System.Reflection;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Elpida.Backend.Data.Tests
{
	public class Helpers
	{
		public static bool AreEqual<T, TR>(PipelineDefinition<T, TR> a, PipelineDefinition<T, TR> b)
		{
			return a.ToString() == b.ToString();
		}

		public static BsonDocumentSortDefinition<TR> GetSortObject<T, TR>(Expression<Func<TR, T>> field, bool desc)
		{
			dynamic body = field.Body;

			var member = (MemberInfo) body.Member;
			return new BsonDocumentSortDefinition<TR>(new BsonDocument(new Dictionary<string, object>
				{[(member.Name.ToLower() == "id" ? "_id" : member.Name)] = (desc ? -1 : 1)}));
		}
	}
}
using Elpida.Backend.Data.Abstractions.Models;
using MongoDB.Bson.Serialization;

namespace Elpida.Backend.Data
{
	public static class TypeRegister
	{
		public static void RegisterDBTypes()
		{
			if (!BsonClassMap.IsClassMapRegistered(typeof(ResultModel)))
				BsonClassMap.RegisterClassMap<ResultModel>(cm =>
				{
					cm.AutoMap();
					cm.MapIdProperty(model => model.Id);
				});

			if (!BsonClassMap.IsClassMapRegistered(typeof(StatisticModel)))
				BsonClassMap.RegisterClassMap<StatisticModel>(cm =>
				{
					cm.AutoMap();
					cm.MapIdProperty(model => model.Id);
				});
		}
	}
}
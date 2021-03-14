using Elpida.Backend.Data.Abstractions;
using Elpida.Backend.Data.Abstractions.Models;
using Elpida.Backend.Data.Abstractions.Models.Task;
using Elpida.Backend.Data.Abstractions.Repositories;
using MongoDB.Driver;

namespace Elpida.Backend.Data
{
	public class MongoTaskRepository: MongoRepository<TaskModel>, ITaskRepository
	{
		public MongoTaskRepository(IMongoCollection<TaskModel> collection) 
			: base(collection)
		{
		}
	}
}
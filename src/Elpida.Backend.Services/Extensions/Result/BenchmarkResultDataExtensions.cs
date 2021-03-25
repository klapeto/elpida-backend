using System.Collections.Generic;
using System.Linq;
using Elpida.Backend.Data.Abstractions.Models;
using Elpida.Backend.Data.Abstractions.Models.Result;
using Elpida.Backend.Data.Abstractions.Models.Task;
using Elpida.Backend.Services.Abstractions.Dtos.Result;

namespace Elpida.Backend.Services.Extensions.Result
{
	public static class BenchmarkResultDataExtensions
	{
		// public static BenchmarkResultDto ToDto(this BenchmarkResultModel model, 
		// 	BenchmarkModel benchmarkModel,
		// 	IEnumerable<TaskModel> tasks)
		// {
		// 	return new BenchmarkResultDto
		// 	{
		// 		Id = model.BenchmarkId,
		// 		Name = benchmarkModel.Name,
		// 		TaskResults = model.TaskResults
		// 			.Join(tasks,
		// 				resultModel => resultModel.TaskId,
		// 				taskModel => taskModel.Id,
		// 				(resultModel, taskModel) => resultModel.ToDto(taskModel))
		// 			.ToList()
		// 	};
		// }
		//
		// public static BenchmarkResultModel ToModel(this BenchmarkResultDto dto)
		// {
		// 	return new BenchmarkResultModel
		// 	{
		// 		BenchmarkId = dto.Id,
		// 		TaskResults = dto.TaskResults.Select(r => r.ToModel()).ToList()
		// 	};
		// }
	}
}
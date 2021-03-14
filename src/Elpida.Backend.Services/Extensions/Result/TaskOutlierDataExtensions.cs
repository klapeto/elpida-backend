using Elpida.Backend.Data.Abstractions.Models.Result;
using Elpida.Backend.Services.Abstractions.Dtos.Result;

namespace Elpida.Backend.Services.Extensions.Result
{
	public static class TaskOutlierDataExtensions
	{
		public static TaskOutlierDto ToDto(this TaskOutlierModel outlierModel)
		{
			return new TaskOutlierDto
			{
				Time = outlierModel.Time,
				Value = outlierModel.Value,
			};
		}

		public static TaskOutlierModel ToModel(this TaskOutlierDto outlierDto)
		{
			return new TaskOutlierModel
			{
				Time = outlierDto.Time,
				Value = outlierDto.Value,
			};
		}
	}
}
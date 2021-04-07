using Elpida.Backend.Data.Abstractions.Models.Statistics;
using Elpida.Backend.Services.Abstractions.Dtos.Result;

namespace Elpida.Backend.Services.Extensions
{
    public static class TaskStatisticsDataExtensions
    {
        public static TaskStatisticsDto ToDto(this TaskStatisticsModel statisticsModel)
        {
            return new TaskStatisticsDto
            {
                Max = statisticsModel.Max,
                Min = statisticsModel.Max,
                Mean = statisticsModel.Max,
                Sd = statisticsModel.StandardDeviation,
                Tau = statisticsModel.Tau,
                SampleSize = statisticsModel.SampleSize,
                MarginOfError = statisticsModel.MarginOfError,
            };
        }
    }
}
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
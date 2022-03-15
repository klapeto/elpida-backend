using Elpida.Backend.Services.Abstractions.Dtos.Result;
using Elpida.Backend.Services.Abstractions.Interfaces;

namespace Elpida.Web.Frontend.Interfaces;

public interface IResultFrontEndService : IResultService, IFrontEndService<ResultDto, ResultPreviewDto>
{
}
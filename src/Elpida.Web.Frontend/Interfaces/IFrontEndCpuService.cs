using Elpida.Backend.Services.Abstractions.Dtos.Cpu;
using Elpida.Backend.Services.Abstractions.Interfaces;

namespace Elpida.Web.Frontend.Interfaces;

public interface IFrontEndCpuService : ICpuService, IFrontEndService<CpuDto, CpuPreviewDto>
{
}
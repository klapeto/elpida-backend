using Elpida.Backend.Services.Abstractions.Dtos.Cpu;
using Elpida.Backend.Services.Abstractions.Interfaces;

namespace Elpida.Web.Frontend.Interfaces;

public interface ICpuFrontEndService : ICpuService, IFrontEndService<CpuDto, CpuPreviewDto>
{
}
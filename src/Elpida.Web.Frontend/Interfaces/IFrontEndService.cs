using Elpida.Backend.Services.Abstractions.Dtos;
using Elpida.Backend.Services.Abstractions.Interfaces;
using Elpida.Web.Frontend.Models;

namespace Elpida.Web.Frontend.Interfaces;

public interface IFrontEndService<TDto, TPreview> : IService<TDto, TPreview>
	where TDto : FoundationDto
	where TPreview : FoundationDto
{
	QueryModel CreateSimpleQueryModel();

	QueryModel CreateAdvancedQueryModel();
}
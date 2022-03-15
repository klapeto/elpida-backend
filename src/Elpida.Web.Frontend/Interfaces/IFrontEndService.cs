using Elpida.Backend.Services.Abstractions.Dtos;
using Elpida.Backend.Services.Abstractions.Interfaces;
using Elpida.Web.Frontend.Models;
using Elpida.Web.Frontend.Models.Filters;

namespace Elpida.Web.Frontend.Interfaces
{
	public interface IFrontEndService<TDto> : IService<TDto>
		where TDto : FoundationDto
	{
	}

	public interface IFrontEndService<TDto, TPreview>
		: IFrontEndService<TDto>,
			IService<TDto, TPreview>
		where TDto : FoundationDto
		where TPreview : FoundationDto
	{
		QueryModel CreateSimpleQueryModel();

		QueryModel CreateAdvancedQueryModel();

		StringFilterModel? CreateSearchFilterModel();
	}
}
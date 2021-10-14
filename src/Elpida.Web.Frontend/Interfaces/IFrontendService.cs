using System.Collections.Generic;
using Elpida.Backend.Services.Abstractions.Dtos;
using Elpida.Backend.Services.Abstractions.Interfaces;
using Elpida.Web.Frontend.Models.Filters;

namespace Elpida.Web.Frontend.Interfaces
{
	public interface IFrontendService<TDto, TPreview> : IService<TDto, TPreview>
		where TDto : FoundationDto
		where TPreview : FoundationDto
	{
		IEnumerable<FilterModel> CreateFilterModels();
	}
}
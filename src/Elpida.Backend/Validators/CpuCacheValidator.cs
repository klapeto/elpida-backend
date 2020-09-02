using Elpida.Backend.Services.Abstractions.Dtos.Result;
using FluentValidation;

namespace Elpida.Backend.Validators
{
	public class CpuCacheValidator : AbstractValidator<CpuCacheDto>
	{
		public CpuCacheValidator()
		{
			RuleFor(dto => dto.Name)
				.NotEmpty()
				.MaximumLength(50);
		}
	}
}
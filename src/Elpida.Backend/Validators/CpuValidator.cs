using Elpida.Backend.Services.Abstractions.Dtos.Result;
using FluentValidation;

namespace Elpida.Backend.Validators
{
	public class CpuValidator : AbstractValidator<CpuDto>
	{
		public CpuValidator()
		{
			RuleFor(dto => dto.Vendor)
				.NotEmpty()
				.MaximumLength(50);
		}
	}
}
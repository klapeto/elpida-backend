using Elpida.Backend.Services.Abstractions.Dtos.Result;
using FluentValidation;

namespace Elpida.Backend.Validators
{
	public class CompilerValidator : AbstractValidator<CompilerDto>
	{
		public CompilerValidator()
		{
			RuleFor(dto => dto.Name)
				.NotEmpty()
				.MaximumLength(100);

			RuleFor(dto => dto.Version)
				.NotEmpty()
				.MaximumLength(50);
		}
	}
}
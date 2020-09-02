using Elpida.Backend.Services.Abstractions.Dtos.Result;
using FluentValidation;

namespace Elpida.Backend.Validators
{
	public class OsValidator : AbstractValidator<OsDto>
	{
		public OsValidator()
		{
			RuleFor(dto => dto.Category)
				.NotEmpty()
				.MaximumLength(30);

			RuleFor(dto => dto.Name)
				.NotEmpty()
				.MaximumLength(100);

			RuleFor(dto => dto.Version)
				.NotEmpty()
				.MaximumLength(50);
		}
	}
}
using Elpida.Backend.Services.Abstractions.Dtos.Result;
using FluentValidation;

namespace Elpida.Backend.Validators
{
	public class ElpidaValidator : AbstractValidator<ElpidaDto>
	{
		public ElpidaValidator()
		{
			RuleFor(dto => dto.Compiler)
				.NotNull();

			RuleFor(dto => dto.Version)
				.NotNull();
		}
	}
}
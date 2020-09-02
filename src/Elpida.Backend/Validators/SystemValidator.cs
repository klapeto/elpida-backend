using Elpida.Backend.Services.Abstractions.Dtos.Result;
using FluentValidation;

namespace Elpida.Backend.Validators
{
	public class SystemValidator : AbstractValidator<SystemDto>
	{
		public SystemValidator()
		{
			RuleFor(dto => dto.Cpu)
				.NotNull();

			RuleFor(dto => dto.Memory)
				.NotNull();

			RuleFor(dto => dto.Topology)
				.NotNull();
		}
	}
}
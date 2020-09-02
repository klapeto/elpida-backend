using Elpida.Backend.Services.Abstractions.Dtos.Result;
using FluentValidation;

namespace Elpida.Backend.Validators
{
	public class CpuNodeValidator : AbstractValidator<CpuNodeDto>
	{
		public CpuNodeValidator()
		{
			RuleFor(dto => dto.Name)
				.NotEmpty()
				.MaximumLength(100);

			RuleFor(dto => dto.NodeType)
				.GreaterThanOrEqualTo(0);
		}
	}
}
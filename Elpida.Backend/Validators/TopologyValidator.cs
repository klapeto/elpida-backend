using Elpida.Backend.Services.Abstractions.Dtos.Result;
using FluentValidation;

namespace Elpida.Backend.Validators
{
	public class TopologyValidator: AbstractValidator<TopologyDto>
	{
		public TopologyValidator()
		{
			RuleFor(dto => dto.Root)
				.NotNull();
		}
	}
}
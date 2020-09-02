using Elpida.Backend.Services.Abstractions.Dtos.Result;
using FluentValidation;

namespace Elpida.Backend.Validators
{
	public class TaskResultValidator : AbstractValidator<TaskResultDto>
	{
		public TaskResultValidator()
		{
			RuleFor(dto => dto.Name)
				.NotEmpty()
				.MaximumLength(100);

			RuleFor(dto => dto.Description)
				.MaximumLength(250);

			RuleFor(dto => dto.Suffix)
				.NotEmpty()
				.MaximumLength(30);

			RuleFor(dto => dto.Time)
				.GreaterThan(0.0);

			RuleFor(dto => dto.Type)
				.GreaterThanOrEqualTo(0);

			RuleFor(dto => dto.Value)
				.GreaterThan(0.0);

			RuleFor(dto => dto.InputSize)
				.GreaterThan(0ul);
		}
	}
}
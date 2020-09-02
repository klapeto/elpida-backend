using Elpida.Backend.Services.Abstractions.Dtos.Result;
using FluentValidation;

namespace Elpida.Backend.Validators
{
	public class BenchmarkResultValidator : AbstractValidator<BenchmarkResultDto>
	{
		public BenchmarkResultValidator()
		{
			RuleFor(dto => dto.Name)
				.NotEmpty()
				.MaximumLength(100);

			RuleFor(dto => dto.TaskResults)
				.NotEmpty();
		}
	}
}
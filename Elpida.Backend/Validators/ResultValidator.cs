using Elpida.Backend.Services.Abstractions.Dtos.Result;
using FluentValidation;

namespace Elpida.Backend.Validators
{
	public class ResultValidator : AbstractValidator<ResultDto>
	{
		public ResultValidator()
		{
			RuleFor(dto => dto.Id)
				.Null();

			RuleFor(dto => dto.Affinity)
				.NotEmpty();

			RuleFor(dto => dto.TimeStamp)
				.Empty();

			RuleFor(dto => dto.Elpida)
				.NotNull();

			RuleFor(dto => dto.Result)
				.NotNull();

			RuleFor(dto => dto.System)
				.NotNull();
		}
	}
}
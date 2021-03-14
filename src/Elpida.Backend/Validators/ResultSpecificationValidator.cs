using Elpida.Backend.Services.Abstractions.Dtos;
using FluentValidation;

namespace Elpida.Backend.Validators
{
	public class ResultSpecificationValidator : AbstractValidator<ResultSpecificationDto>
	{
		public ResultSpecificationValidator()
		{
			RuleFor(dto => dto.Name)
				.NotEmpty()
				.MaximumLength(100);

			RuleFor(dto => dto.Description)
				.MaximumLength(250);

			RuleFor(dto => dto.Aggregation)
				.GreaterThanOrEqualTo(0);

			RuleFor(dto => dto.Type)
				.GreaterThanOrEqualTo(0);

			RuleFor(dto => dto.Unit)
				.NotEmpty();
		}
	}
}
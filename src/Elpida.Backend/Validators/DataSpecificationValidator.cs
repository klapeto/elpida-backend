using Elpida.Backend.Services.Abstractions.Dtos;
using FluentValidation;

namespace Elpida.Backend.Validators
{
	public class DataSpecificationValidator : AbstractValidator<DataSpecificationDto>
	{
		public DataSpecificationValidator()
		{
			RuleFor(dto => dto.Name)
				.NotEmpty()
				.MaximumLength(100);

			RuleFor(dto => dto.Description)
				.MaximumLength(250);

			RuleFor(dto => dto.RequiredProperties)
				.NotNull();

			RuleFor(dto => dto.Unit)
				.NotEmpty();
		}
	}
}
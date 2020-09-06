using Elpida.Backend.Services.Abstractions;
using FluentValidation;

namespace Elpida.Backend.Validators
{
	public class QueryRequestValidator: AbstractValidator<QueryRequest>
	{
		public QueryRequestValidator()
		{
			RuleFor(request => request.PageRequest)
				.NotNull();
		}
	}
}
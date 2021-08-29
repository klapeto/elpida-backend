using System.Collections.Generic;
using Elpida.Backend.Services.Abstractions;
using Elpida.Backend.Validators;

namespace Elpida.Backend.Tests.Validators
{
	public class QueryRequestValidatorTests : ValidatorTest<QueryRequest, QueryRequestValidator>
	{
		protected override IEnumerable<(QueryRequest, string)> GetInvalidData()
		{
			yield return (new QueryRequest(null!, null, null, false), $"null {nameof(QueryRequest.PageRequest)}");
		}
	}
}
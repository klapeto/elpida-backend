using System.Collections.Generic;
using Elpida.Backend.Services.Abstractions;
using Elpida.Backend.Validators;

namespace Elpida.Backend.Tests.Validators
{
	public class PageRequestValidatorTests : ValidatorTest<PageRequest, PageRequestValidator>
	{
		protected override IEnumerable<(PageRequest, string)> GetInvalidData()
		{
			yield return (new PageRequest(-5, 10, 50), $"negative {nameof(PageRequest.Next)}");
			yield return (new PageRequest(5, -1, 50), $"negative {nameof(PageRequest.Count)}");
			yield return (new PageRequest(5, 0, 50), $"zero {nameof(PageRequest.Count)}");
			yield return (new PageRequest(5, PageRequest.MaxCount + 1, 50), $"very large {nameof(PageRequest.Count)}");
		}
	}
}
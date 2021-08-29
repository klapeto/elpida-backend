using System.Collections.Generic;
using Elpida.Backend.Services.Abstractions;
using Elpida.Backend.Validators;

namespace Elpida.Backend.Tests.Validators
{
	public class FilterInstanceValidatorTests : ValidatorTest<FilterInstance, FilterInstanceValidator>
	{
		protected override IEnumerable<(FilterInstance, string)> GetInvalidData()
		{
			yield return (new FilterInstance(null!, "Test", "eq"), $"null {nameof(FilterInstance.Name)}");
			yield return (new FilterInstance(string.Empty, "Test", "eq"), $"null {nameof(FilterInstance.Name)}");
			yield return (new FilterInstance(" ", "Test", "eq"), $"null {nameof(FilterInstance.Name)}");

			yield return (new FilterInstance("Test", null!, "Test"), $"null {nameof(FilterInstance.Value)}");

			yield return (new FilterInstance("Test", "Test", null!), $"null {nameof(FilterInstance.Comparison)}");
			yield return (new FilterInstance("Test", "Test", string.Empty),
				$"null {nameof(FilterInstance.Comparison)}");

			yield return (new FilterInstance("Test", "Test", " "), $"null {nameof(FilterInstance.Comparison)}");
		}
	}
}
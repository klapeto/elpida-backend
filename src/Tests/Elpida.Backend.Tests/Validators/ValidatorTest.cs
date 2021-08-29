using System.Collections.Generic;
using FluentValidation;
using NUnit.Framework;

namespace Elpida.Backend.Tests.Validators
{
	[TestFixture]
	public abstract class ValidatorTest<TDto, TValidator>
		where TValidator : AbstractValidator<TDto>, new()
	{
		[Test]
		public void TestInvalidData()
		{
			var validator = new TValidator();

			foreach (var (data, type) in GetInvalidData())
			{
				var result = validator.Validate(data);
				Assert.False(result.IsValid, type);
			}
		}

		protected abstract IEnumerable<(TDto, string)> GetInvalidData();
	}
}
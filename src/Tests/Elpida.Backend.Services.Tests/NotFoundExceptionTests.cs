using System;
using Elpida.Backend.Services.Abstractions;
using NUnit.Framework;

namespace Elpida.Backend.Services.Tests
{
	public class NotFoundExceptionTests
	{
		[Test]
		public void Id_Valid()
		{
			var id = Guid.NewGuid().ToString("N");
			var ex = new NotFoundException(id);

			Assert.AreEqual(id, ex.Id);
		}
	}
}
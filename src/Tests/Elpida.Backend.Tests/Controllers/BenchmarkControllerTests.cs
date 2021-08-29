using Elpida.Backend.Controllers;
using Elpida.Backend.Services.Abstractions.Interfaces;
using Moq;
using NUnit.Framework;

namespace Elpida.Backend.Tests.Controllers
{
	[TestFixture]
	public class BenchmarkControllerTests
	{
		[Test]
		public void Success()
		{
			var service = new Mock<IBenchmarkService>(MockBehavior.Strict);
			var controller = new BenchmarkController(service.Object);
		}
	}
}
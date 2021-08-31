using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;

namespace Elpida.Backend.Tests
{
	[TestFixture]
	public class ApiKeyAuthenticationAttributeTests
	{
		private class ApiOptions : IOptions<ApiKeys>
		{
			public ApiOptions(ApiKeys value)
			{
				Value = value;
			}

			public ApiKeys Value { get; }
		}

		private static (ActionExecutingContext actionExecutingContext, ActionExecutionDelegate next) GetTestingPack()
		{
			var actionContext = new ActionContext(
				new DefaultHttpContext(),
				new RouteData(new RouteValueDictionary(string.Empty)),
				new ActionDescriptor()
			);

			var actionExecuting = new ActionExecutingContext(
				actionContext,
				new List<IFilterMetadata>(),
				new Dictionary<string, object>(),
				new object()
			);

			return (actionExecuting, () => Task.FromResult(
				new ActionExecutedContext(actionContext, new List<IFilterMetadata>(), new object())
			));
		}

		[Test]
		public async Task NoHeader_SetsUnauthorizedResult()
		{
			var attribute = new ApiKeyAuthenticationAttribute("Test_Key");

			var (actionExecuting, next) = GetTestingPack();

			await attribute.OnActionExecutionAsync(
				actionExecuting,
				next
			);

			Assert.NotNull(actionExecuting.Result);
			Assert.True(actionExecuting.Result.GetType() == typeof(UnauthorizedResult));
		}

		[Test]
		public async Task Header_WrongKey_SetsUnauthorizedResult()
		{
			const string keyName = "Test_Key";
			var attribute = new ApiKeyAuthenticationAttribute(keyName);

			var (actionExecuting, next) = GetTestingPack();

			var mock = new Mock<IServiceProvider>(MockBehavior.Strict);
			mock.Setup(p => p.GetService(typeof(IOptions<ApiKeys>)))
				.Returns(new ApiOptions(new ApiKeys { [keyName] = "LOL" }));

			actionExecuting.HttpContext.RequestServices = mock.Object;
			actionExecuting.HttpContext.Request.Headers.Add("api_key", "haha");

			await attribute.OnActionExecutionAsync(
				actionExecuting,
				next
			);

			Assert.NotNull(actionExecuting.Result);
			Assert.True(actionExecuting.Result.GetType() == typeof(UnauthorizedResult));

			mock.Verify(p => p.GetService(typeof(IOptions<ApiKeys>)), Times.Once);
		}

		[Test]
		public void Header_NonExistingKey_ThrowsArgumentException()
		{
			const string keyName = "Test_Key";
			var attribute = new ApiKeyAuthenticationAttribute(keyName);

			var (actionExecuting, next) = GetTestingPack();

			var mock = new Mock<IServiceProvider>(MockBehavior.Strict);
			mock.Setup(p => p.GetService(typeof(IOptions<ApiKeys>)))
				.Returns(new ApiOptions(new ApiKeys { ["hahhaha"] = "LOL" }));

			actionExecuting.HttpContext.RequestServices = mock.Object;
			actionExecuting.HttpContext.Request.Headers.Add("api_key", "haha");

			Assert.ThrowsAsync<ArgumentException>(
				() => attribute.OnActionExecutionAsync(
					actionExecuting,
					next
				)
			);

			mock.Verify(p => p.GetService(typeof(IOptions<ApiKeys>)), Times.Once);
		}

		[Test]
		public async Task Header_CorrectKey_SetsUnauthorizedResult()
		{
			const string keyName = "Test_Key";
			const string key = "LOOOL";
			var attribute = new ApiKeyAuthenticationAttribute(keyName);

			var (actionExecuting, next) = GetTestingPack();

			var mock = new Mock<IServiceProvider>(MockBehavior.Strict);
			mock.Setup(p => p.GetService(typeof(IOptions<ApiKeys>)))
				.Returns(new ApiOptions(new ApiKeys { [keyName] = key }));

			actionExecuting.HttpContext.RequestServices = mock.Object;
			actionExecuting.HttpContext.Request.Headers.Add("api_key", key);

			var called = false;

			await attribute.OnActionExecutionAsync(
				actionExecuting,
				() =>
				{
					called = true;
					return next();
				}
			);

			Assert.True(called);

			mock.Verify(p => p.GetService(typeof(IOptions<ApiKeys>)), Times.Once);
		}
	}
}
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Elpida.Web.Frontend.Shared.Extensions
{
	internal static class ElementReferenceExtensions
	{
		public static async Task ScrollTo(
			this ElementReference elementReference,
			IJSRuntime js,
			int top,
			int left,
			string behavior
		)
		{
			await js.InvokeVoidAsync("interopFunctions.scrollTo", elementReference, top, left, behavior);
		}
	}
}
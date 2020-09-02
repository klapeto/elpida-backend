using System;

namespace Elpida.Backend.Services.Abstractions
{
	public class NotFoundException : Exception
	{
		public NotFoundException(string id)
		{
			Id = id;
		}

		public string Id { get; }
	}
}
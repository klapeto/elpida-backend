using System;

namespace Elpida.Backend.Services.Abstractions.Exceptions
{
	public class ConflictException : Exception
	{
		public ConflictException(string id, string message)
			: base(message)
		{
			Id = id;
		}

		public string Id { get; }
	}
}
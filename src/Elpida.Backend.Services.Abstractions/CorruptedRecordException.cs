using System;

namespace Elpida.Backend.Services.Abstractions
{
	public class CorruptedRecordException : Exception
	{
		public CorruptedRecordException(string id)
		{
			Id = id;
		}

		public string Id { get; }
	}
}
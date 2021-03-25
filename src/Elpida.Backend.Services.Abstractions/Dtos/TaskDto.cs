using System;

namespace Elpida.Backend.Services.Abstractions.Dtos
{
	public class TaskDto
	{
		public long Id { get; set; }
		public Guid Uuid { get; set; }
		public string Name { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;
		public ResultSpecificationDto Result { get; set; } = new ResultSpecificationDto();
		public DataSpecificationDto? Input { get; set; }
		public DataSpecificationDto? Output { get; set; }
	}
}
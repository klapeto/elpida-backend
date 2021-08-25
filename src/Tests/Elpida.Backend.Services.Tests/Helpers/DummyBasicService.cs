using System.Threading;
using System.Threading.Tasks;

namespace Elpida.Backend.Services.Tests.Helpers
{
	internal class DummyBasicService : Service<DummyDto, DummyModel, IDummyRepository>
	{
		public DummyBasicService(IDummyRepository repository)
			: base(repository)
		{
		}

		protected override DummyDto ToDto(DummyModel model)
		{
			return new ()
			{
				Id = model.Id,
				Data = model.Data,
			};
		}

		protected override Task<DummyModel> ProcessDtoAndCreateModelAsync(
			DummyDto dto,
			CancellationToken cancellationToken
		)
		{
			return Task.FromResult(
				new DummyModel
				{
					Id = dto.Id,
					Data = dto.Data,
				}
			);
		}
	}
}
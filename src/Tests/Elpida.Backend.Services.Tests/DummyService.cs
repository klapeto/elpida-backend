using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Elpida.Backend.Common.Lock;
using Elpida.Backend.Services.Abstractions;

namespace Elpida.Backend.Services.Tests
{
	public class DummyService : Service<DummyDto, DummyModel, IDummyRepository>
	{
		
		public DummyService(IDummyRepository repository, ILockFactory lockFactory)
			: base(repository, lockFactory)
		{
		}

		protected override IEnumerable<FilterExpression> GetFilterExpressions()
		{
			return new[]
			{
				CreateFilter("data", m => m.Data),
			};
		}

		public IReadOnlyDictionary<string, LambdaExpression> GetFilters()
		{
			return GetLambdaFilters();
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
using Elpida.Backend.Data.Abstractions.Models;
using Elpida.Backend.Data.Abstractions.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Elpida.Backend.Data
{
    public class ElpidaRepository : EntityRepository<ElpidaModel>, IElpidaRepository
    {
        public ElpidaRepository(ElpidaContext context)
            : base(context, context.Elpidas)
        {
        }
    }
}
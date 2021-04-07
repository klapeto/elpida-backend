using Elpida.Backend.Data.Abstractions.Models;
using Elpida.Backend.Data.Abstractions.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Elpida.Backend.Data
{
    public class OsRepository : EntityRepository<OsModel>, IOsRepository
    {
        public OsRepository(ElpidaContext context)
            : base(context, context.Oses)
        {
        }
    }
}
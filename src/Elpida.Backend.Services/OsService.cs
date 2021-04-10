using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Elpida.Backend.Data.Abstractions.Models;
using Elpida.Backend.Data.Abstractions.Repositories;
using Elpida.Backend.Services.Abstractions;
using Elpida.Backend.Services.Abstractions.Dtos.Result;
using Elpida.Backend.Services.Abstractions.Interfaces;
using Elpida.Backend.Services.Extensions;

namespace Elpida.Backend.Services
{
    public class OsService : Service<OsDto, OsModel>, IOsService
    {
        private static readonly IEnumerable<FilterExpression> OsFilters = new List<FilterExpression>
        {
            CreateFilter("osCategory", model => model.Category),
            CreateFilter("osName", model => model.Name),
            CreateFilter("osVersion", model => model.Version)
        };

        public OsService(IOsRepository osRepository)
            : base(osRepository)
        {
        }

        protected override IEnumerable<FilterExpression> GetFilterExpressions()
        {
            return OsFilters;
        }

        protected override OsDto ToDto(OsModel model)
        {
            return model.ToDto();
        }

        protected override OsModel ToModel(OsDto dto)
        {
            return dto.ToModel();
        }

        protected override Expression<Func<OsModel, bool>> GetCreationBypassCheckExpression(OsDto dto)
        {
            return o =>
                o.Category == dto.Category
                && o.Name == dto.Name
                && o.Version == dto.Version;
        }
    }
}
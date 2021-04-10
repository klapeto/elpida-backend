using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Elpida.Backend.Data.Abstractions.Interfaces;
using Elpida.Backend.Data.Abstractions.Models;
using Elpida.Backend.Services.Abstractions;
using Elpida.Backend.Services.Abstractions.Exceptions;
using Elpida.Backend.Services.Abstractions.Interfaces;
using Elpida.Backend.Services.Utilities;

namespace Elpida.Backend.Services
{
    public abstract class Service<TDto, TModel> : IService<TDto> where TModel : Entity
    {
        protected Service(IRepository<TModel> repository)
        {
            Repository = repository;
        }

        protected IRepository<TModel> Repository { get; }

        public async Task<TDto> GetSingleAsync(long id, CancellationToken cancellationToken = default)
        {
            var entity = await Repository.GetSingleAsync(id, cancellationToken);

            if (entity == null) throw new NotFoundException($"{typeof(TDto).Name} was not found", id);

            return ToDto(entity);
        }

        public async Task<PagedResult<TDto>> GetPagedAsync(QueryRequest queryRequest,
            CancellationToken cancellationToken = default)
        {
            var expressionBuilder = new QueryExpressionBuilder(GetLambdaFilters());

            var result = await Repository.GetMultiplePagedAsync(
                queryRequest.PageRequest.Next,
                queryRequest.PageRequest.Count,
                queryRequest.Descending,
                queryRequest.PageRequest.TotalCount == 0,
                expressionBuilder.GetOrderBy<TModel>(queryRequest),
                expressionBuilder.Build<TModel>(queryRequest.Filters),
                cancellationToken);

            queryRequest.PageRequest.TotalCount = result.TotalCount;

            return new PagedResult<TDto>(result.Items.Select(ToDto).ToList(), queryRequest.PageRequest);
        }

        public async Task<TDto> GetOrAddAsync(TDto dto, CancellationToken cancellationToken = default)
        {
            TModel? entity;
            var bypassExpression = GetCreationBypassCheckExpression(dto);
            if (bypassExpression != null)
            {
                entity = await Repository.GetSingleAsync(bypassExpression, cancellationToken);
                if (entity != null) return ToDto(entity);
            }

            await PreProcessDtoForCreationAsync(dto, cancellationToken);
            entity = ToModel(dto);
            entity.Id = 0;
            entity = await Repository.CreateAsync(entity, cancellationToken);

            await Repository.SaveChangesAsync(cancellationToken);

            await OnEntityCreatedAsync(dto, entity, cancellationToken);

            return ToDto(entity);
        }

        public IEnumerable<FilterExpression> GetFilters<T, TR>(Expression<Func<T, TR>> baseExpression)
        {
            if (typeof(TR) != typeof(TModel))
                throw new ArgumentException($"The type of the TR is not of type: {typeof(TModel).Name}");
            var baseBody = baseExpression.Body;
            foreach (var filter in GetFilterExpressions())
                yield return new FilterExpression(filter.Name,
                    Expression.MakeMemberAccess(baseBody, filter.Expression.Member));
        }

        protected static FilterExpression CreateFilter<T>(string name, Expression<Func<TModel, T>> expression)
        {
            if (expression.Body.NodeType != ExpressionType.MemberAccess)
                throw new InvalidOperationException("The expression body must be member access of the model");
            return new FilterExpression(name.ToLowerInvariant(), (MemberExpression) expression.Body);
        }

        private IReadOnlyDictionary<string, LambdaExpression> GetLambdaFilters()
        {
            return GetFilterExpressions()
                .ToDictionary(p => p.Name,
                    p => Expression.Lambda(p.Expression, GetParameterExpression(p.Expression)));
        }

        private static ParameterExpression GetParameterExpression(Expression expression)
        {
            while (expression.NodeType == ExpressionType.MemberAccess)
                expression = ((MemberExpression) expression).Expression;

            if (expression.NodeType != ExpressionType.Parameter)
                throw new ArgumentException("Expression does not contain parameter");

            return (ParameterExpression) expression;
        }

        protected abstract TDto ToDto(TModel model);

        protected abstract TModel ToModel(TDto dto);

        protected virtual IEnumerable<FilterExpression> GetFilterExpressions()
        {
            yield break;
        }

        protected virtual Task PreProcessDtoForCreationAsync(TDto dto, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        protected virtual Expression<Func<TModel, bool>>? GetCreationBypassCheckExpression(TDto dto)
        {
            return null;
        }

        protected virtual Task OnEntityCreatedAsync(TDto dto, TModel entity, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
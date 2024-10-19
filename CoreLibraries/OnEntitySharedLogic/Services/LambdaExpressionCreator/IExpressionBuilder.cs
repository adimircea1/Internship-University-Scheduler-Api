using System.Linq.Expressions;

namespace OnEntitySharedLogic.Services.LambdaExpressionCreator;

public interface IExpressionBuilder
{
    public Expression<Func<TEntity, object>>? BuildOrderByExpression<TEntity>(string propertyName);
}
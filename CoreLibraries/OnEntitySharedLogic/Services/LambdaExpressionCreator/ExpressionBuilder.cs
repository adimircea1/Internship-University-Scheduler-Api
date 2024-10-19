using System.Linq.Expressions;

namespace OnEntitySharedLogic.Services.LambdaExpressionCreator;

public class ExpressionBuilder : IExpressionBuilder
{
    public Expression<Func<TEntity, object>>? BuildOrderByExpression<TEntity>(string propertyName)
    {
        var entityType = typeof(TEntity);

        var entityProperty = entityType.GetProperties().FirstOrDefault(property => string.Equals(propertyName, property.Name, StringComparison.InvariantCultureIgnoreCase));

        if (entityProperty is null)
        {
            return null;
        }

        var expressionParameter = Expression.Parameter(entityType, "entity");
        var expressionProperty = Expression.Property(expressionParameter, entityProperty);
        var convertedExpressionProperty = Expression.Convert(expressionProperty, typeof(object));
        return Expression.Lambda<Func<TEntity, object>>(convertedExpressionProperty, expressionParameter);
    }
}
using System.Linq.Expressions;

namespace CustomMapper.Interfaces;

public interface ICustomMapperRegistry
{
    void Register<TSource, TDestination>(Action<TSource, TDestination> mapAction, Expression<Func<TSource, TDestination>>? projection = null);

    Action<object, object>? GetMapAction(Type sourceType, Type destinationType);
    LambdaExpression? GetProjectionExpression(Type sourceType, Type destinationType);
}
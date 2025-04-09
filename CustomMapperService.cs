using System.Linq.Expressions;
using CustomMapper.Interfaces;

namespace CustomMapper;

public class CustomMapperService(ICustomMapperRegistry registry) : ICustomMapperService
{
    public void Map(object source, object destination)
    {
        if (source == null || destination == null)
            throw new ArgumentNullException("Source and destination cannot be null.");

        var action = registry.GetMapAction(source.GetType(), destination.GetType());
        if (action == null)
            throw new InvalidOperationException($"No map registered for {source.GetType().Name} → {destination.GetType().Name}");

        action(source, destination);
    }
    public Expression<Func<TSource, TDestination>> Projector<TSource, TDestination>()
    {
        var expr = registry.GetProjectionExpression(typeof(TSource), typeof(TDestination));
        if (expr == null)
            throw new InvalidOperationException($"No projection expression registered for {typeof(TSource).Name} → {typeof(TDestination).Name}");

        return (Expression<Func<TSource, TDestination>>)expr;
    }
}
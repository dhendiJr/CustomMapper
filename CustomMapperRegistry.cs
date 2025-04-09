using System.Linq.Expressions;
using CustomMapper.Interfaces;

namespace CustomMapper;

internal class CustomMapperRegistry : ICustomMapperRegistry
{
    private readonly Dictionary<(Type, Type), Action<object, object>> _mapFuncs = new();
    private readonly Dictionary<(Type, Type), LambdaExpression> _projectionFuncs = new();

    public void Register<TSource, TDestination>(
        Action<TSource, TDestination> mapAction,
        Expression<Func<TSource, TDestination>>? projection = null)
    {
        var key = (typeof(TSource), typeof(TDestination));
        if (_mapFuncs.ContainsKey(key))
        {
            throw new InvalidOperationException($"Duplicate type {typeof(TSource).FullName}.{typeof(TDestination).FullName}.");
        }
        _mapFuncs[key] = (src, dest) => mapAction((TSource)src, (TDestination)dest);
        

        if (projection != null)
        {
            _projectionFuncs[key] = projection;
        }
        
    }

    public Action<object, object>? GetMapAction(Type sourceType, Type destinationType)
    {
        _mapFuncs.TryGetValue((sourceType, destinationType), out var action);
        return action;
    }

    public LambdaExpression? GetProjectionExpression(Type sourceType, Type destinationType)
    {
        _projectionFuncs.TryGetValue((sourceType, destinationType), out var expr);
        return expr;
    }
    
}
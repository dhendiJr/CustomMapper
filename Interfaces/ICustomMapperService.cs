using System.Linq.Expressions;

namespace CustomMapper.Interfaces;

public interface ICustomMapperService
{
    void Map(object source, object destination);
    Expression<Func<TSource, TDestination>> Projector<TSource, TDestination>();
}

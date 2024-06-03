using System.Linq.Expressions;
using Core.Models.BaseModels;

namespace BLL.Interfaces.IServiceInterfaces;

public interface IGenericService<T> where T : BaseEntity
{
    Task<T> GetSingleByCondition(Expression<Func<T, bool>> predicate);
    Task<IEnumerable<T>> GetListByCondition(Expression<Func<T, bool>> predicate);
    Task Add(T obj);
    Task Update(T obj, Expression<Func<T, bool>> condition);
    Task Delete(Expression<Func<T, bool>> predicate);
}
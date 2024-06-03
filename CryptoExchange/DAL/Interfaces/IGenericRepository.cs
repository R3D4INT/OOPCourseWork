using Core.Models;
using Core.Models.BaseModels;
using System.Linq.Expressions;
namespace DAL.Interfaces;

public interface IGenericRepository<T> where T : BaseEntity
{
    Task<Result<IEnumerable<T>>> GetListByConditionAsync(Expression<Func<T, bool>> condition, params Expression<Func<T, object>>[] includes);
    Task<Result<T>> GetSingleByConditionAsync(Expression<Func<T, bool>> condition, params Expression<Func<T, object>>[] includes);
    Task<Result<T>> AddAsync(T item);
    Task<Result<bool>> UpdateAsync(T item, Expression<Func<T, bool>> condition);
    Task<Result<bool>> DeleteAsync(Expression<Func<T, bool>> condition);
}
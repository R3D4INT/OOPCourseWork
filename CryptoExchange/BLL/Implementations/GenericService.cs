using Core.Models.BaseModels;
using System.Linq.Expressions;
using DAL.Interfaces;
using BLL.Interfaces.IServiceInterfaces;

namespace BLL.Implementations;

public abstract class GenericService<T> : IGenericService<T> where T : BaseEntity
{
    private readonly IGenericRepository<T> _repository;

    protected GenericService(IGenericRepository<T> repository)
    {
        _repository = repository;
    }

    public virtual async Task Add(T obj)
    {
        try
        {
            var result = await _repository.AddAsync(obj);

            if (!result.IsSuccess)
            {
                throw new Exception($"Failed to add {typeof(T).Name}.");
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to add {typeof(T).Name}. Exception: {ex.Message}");
        }
    }

    public virtual async Task Delete(Expression<Func<T, bool>> predicate)
    {
        try
        {
            var result = await _repository.DeleteAsync(predicate);

            if (!result.IsSuccess)
            {
                throw new Exception($"Failed to delete {typeof(T).Name}.");
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to delete {typeof(T).Name}. Exception: {ex.Message}");
        }
    }

    public virtual async Task<IEnumerable<T>> GetListByCondition(Expression<Func<T, bool>> predicate)
    {
        try
        {
            var result = await _repository.GetListByConditionAsync(predicate);

            if (!result.IsSuccess)
            {
                throw new Exception($"Failed to get list by condition {typeof(T).Name}s.");
            }

            return result.Data;
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to get all {typeof(T).Name}s. Exception: {ex.Message}");
        }
    }

    public async Task<T> GetSingleByCondition(Expression<Func<T, bool>> predicate)
    {
        try
        {
            var result = await _repository.GetSingleByConditionAsync(predicate);

            if (!result.IsSuccess)
            {
                throw new Exception($"Failed to get by predicate {typeof(T).Name}s.");
            }

            return result.Data;
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to get by predicate {typeof(T).Name}s. Exception: {ex.Message}");
        }
    }

    public virtual async Task Update(T obj, Expression<Func<T, bool>> condition)
    {
        try
        {
            var result = await _repository.UpdateAsync(obj, condition);

            if (!result.IsSuccess)
            {
                throw new Exception($"Failed to update {typeof(T).Name} with Id {obj.Id}.");
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to update {typeof(T).Name} with Id {obj.Id}. Exception: {ex.Message}");
        }
    }
}
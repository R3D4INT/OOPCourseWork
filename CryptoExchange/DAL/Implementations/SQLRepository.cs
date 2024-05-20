using Core.Models.BaseModels;
using Core.Models;
using DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using JsonNet.ContractResolvers;
using Microsoft.EntityFrameworkCore;

namespace DAL.Implementations
{
    public class SQLRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly string _filePath;

        private readonly JsonSerializerOptions _options = new JsonSerializerOptions
        { PropertyNamingPolicy = JsonNamingPolicy.CamelCase, WriteIndented = true };

        public SQLRepository(string filePath = null)
        {
            _filePath = filePath ?? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"{typeof(T).Name}.json");
        }
        public async Task<Result<IEnumerable<T>>> GetListByConditionAsync(Expression<Func<T, bool>> condition, params Expression<Func<T, object>>[] includes)
        {
            try
            {
                using (var context = new AppDbContext())
                {
                    await context.Database.EnsureCreatedAsync();
                    var items = await context.Set<T>().Where(condition).ToListAsync();
                    return Result<IEnumerable<T>>.Success(items);
                }
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<T>>.Fail("Error retrieving data.");
            }
        }
        public async Task<Result<T>> GetSingleByConditionAsync(Expression<Func<T, bool>> condition, params Expression<Func<T, object>>[] includes)
        {
            using (var context = new AppDbContext())
            {
                await context.Database.EnsureCreatedAsync();
                var query = context.Set<T>().AsQueryable();
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }

                var item = await query.FirstOrDefaultAsync(condition);

                if (item != null)
                {
                    return Result<T>.Success(item);
                }
                else
                {
                    return Result<T>.Fail("Item not found.");
                }
            }
        }
        public async Task<Result<T>> AddAsync(T item)
        {
            try
            {
                using (var context = new AppDbContext())
                {
                    await context.Database.EnsureCreatedAsync();
                    context.Set<T>().Add(item);

                    await context.SaveChangesAsync();

                    return Result<T>.Success(item);
                }
            }
            catch (Exception ex)
            {
                return Result<T>.Fail("Error adding data.");
            }
        }
        public async Task<Result<bool>> UpdateAsync(T item, Expression<Func<T, bool>> condition)
        {
            try
            {
                using (var context = new AppDbContext())
                {
                    await context.Database.EnsureCreatedAsync();
                    var itemToUpdate = await context.Set<T>().FirstOrDefaultAsync(condition);

                    if (itemToUpdate == null)
                    {
                        return Result<bool>.Fail("Item not found.");
                    }

                    context.Entry(itemToUpdate).CurrentValues.SetValues(item);
                    await context.SaveChangesAsync();
                    return Result<bool>.Success(true);
                }
            }
            catch (Exception ex)
            {
                return Result<bool>.Fail("Error updating data.");
            }
        }
        public async Task<Result<bool>> DeleteAsync(Expression<Func<T, bool>> condition)
        {
            try
            {
                using (var context = new AppDbContext())
                {
                    await context.Database.EnsureCreatedAsync();
                    var itemsToRemove = await context.Set<T>().Where(condition).ToListAsync();

                    if (!itemsToRemove.Any())
                    {
                        return Result<bool>.Fail("Item not found.");
                    }
                    context.Set<T>().RemoveRange(itemsToRemove);
                    await context.SaveChangesAsync();

                    return Result<bool>.Success(true);
                }
            }
            catch (Exception ex)
            {
                return Result<bool>.Fail("Error deleting data.");
            }
        }
    }
}


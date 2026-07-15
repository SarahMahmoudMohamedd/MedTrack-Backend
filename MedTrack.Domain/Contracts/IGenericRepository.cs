using MedTrack.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MedTrack.Domain.Contracts
{
    public interface IGenericRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        Task<TEntity?> GetByIdAsync(TKey id, params string[] includes);
        Task<IEnumerable<TEntity>> GetAllAsync(params string[] includes);
        Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, params string[] includes);

        Task AddAsync(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);

           Task<(IEnumerable<TEntity> Items, int TotalCount)> GetPagedAsync(
            Expression<Func<TEntity, bool>> predicate,
            int page,
            int pageSize,
            params string[] includes);
    }
}

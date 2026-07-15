using MedTrack.Domain.Contracts;
using MedTrack.Domain.Entities;
using MedTrack.Presistance.Data.DbContexts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedTrack.Presistance.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private Hashtable? _repositories;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public IGenericRepository<TEntity, TKey> GetRepository<TEntity, TKey>() where TEntity : BaseEntity<TKey>
        {
            if (_repositories == null) _repositories = new Hashtable();

            // الـ Key هنا هيكون مثلاً "Patient_Guid" عشان نضمن فريدته تماماً
            var cacheKey = $"{typeof(TEntity).Name}_{typeof(TKey).Name}";

            if (!_repositories.ContainsKey(cacheKey))
            {
                var repositoryType = typeof(GenericRepository<,>); // لاحظي الـ comma (,) لأن الـ Generic هنا بياخد قطعتين TEntity, TKey
                var repositoryInstance = Activator.CreateInstance(
                    repositoryType.MakeGenericType(typeof(TEntity), typeof(TKey)), _context);

                _repositories.Add(cacheKey, repositoryInstance);
            }

            return (IGenericRepository<TEntity, TKey>)_repositories[cacheKey]!;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}

using CarWash.Repository.Repositories.BaseRepository;
using CarWash.Core.Entity;
using CarWash.Repository.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq.Expressions;

namespace CarWash.Repository.Repositories.BaseRepository
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : EntityBase
    {
        protected readonly AppDbContext _context;
        private readonly DbSet<T> _dbSet;

        protected RepositoryBase(AppDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        // Tüm T nesnelerini getirir
        public IQueryable<T> FindAll(bool trackChanges)
        {

            return trackChanges?
                    _dbSet.AsTracking() :
                    _dbSet.AsNoTracking();
        }
        // Belirli bir koşulu karşılayan T nesnelerini getirir
        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges)
        {
            return trackChanges ?
                    _dbSet.Where(expression).AsTracking() :
                    _dbSet.Where(expression).AsNoTracking();
        } 
            
        //Id ile T nesnesi getirir
        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }
        // Belirli bir koşula gore T nesnesinin varligini kontrol eder 
        public async Task<bool> AnyAsync(Expression<Func<T, bool>> expression)
        {
            return await _dbSet.AnyAsync(expression);
        }
        // T nesnesini veritabanına asenkron olarak ekler
        public async Task CreateAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        // T nesnesini günceller
        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        // T nesnesini siler
        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }
        //count
        public async Task<int> GetCountAsync()
        {
            return await _dbSet.CountAsync();
        }

    }
}

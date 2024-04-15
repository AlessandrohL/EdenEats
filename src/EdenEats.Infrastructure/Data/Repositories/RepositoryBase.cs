using EdenEats.Domain.Contracts.Entities;
using EdenEats.Domain.Contracts.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdenEats.Infrastructure.Data.Repositories
{
    public abstract class RepositoryBase<TEntity> : IRepositoryBase<TEntity>
        where TEntity : BaseEntity
    {
        protected readonly ApplicationDbContext _context;

        public RepositoryBase(ApplicationDbContext dbContext)
        {
            _context = dbContext ?? throw new ArgumentNullException($"{nameof(dbContext)} is null");
        }

        public void Create(TEntity entity) => _context.Set<TEntity>().Add(entity);

        public void Remove(TEntity entity) => _context.Set<TEntity>().Remove(entity);

        public void Update(TEntity entity) => _context.Set<TEntity>().Update(entity);
    }
}

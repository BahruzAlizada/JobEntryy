
using JobEntryy.Application.Repositories;
using JobEntryy.Domain.Common;
using JobEntryy.Persistence.Concrete;
using Microsoft.EntityFrameworkCore;

namespace JobEntryy.Persistence.Repositories
{
    public class WriteRepository<T> : IWriteRepository<T> where T : BaseEntity
    {
        public void Activity(T entity)
        {
            using var context = new Context();

            if (entity.Status)
                entity.Status = false;
            else
                entity.Status = true;

            context.Set<T>().Update(entity);
            context.SaveChanges();
        }

        public void Add(T entity)
        {
            using var context = new Context();
            var addEntity = context.Entry(entity);
            addEntity.State = EntityState.Added;

            context.SaveChanges();
        }

        public async Task AddAsync(T entity)
        {
            using var context = new Context();
            var addEntity = context.Entry(entity);
            addEntity.State = EntityState.Added;

            await context.SaveChangesAsync();
        }

        public void Delete(T entity)
        {
            using var context = new Context();
            var deleteEntity = context.Entry(entity);
            deleteEntity.State = EntityState.Deleted;

            context.SaveChanges();
        }

        public void Update(T entity)
        {
            using var context = new Context();
            var updateEntity = context.Entry(entity);
            updateEntity.State = EntityState.Modified;

            context.SaveChanges();
        }

        public async Task UpdateAsync(T entity)
        {
            using var context = new Context();
            var entityUpdate = context.Entry(entity);
            entityUpdate.State = EntityState.Modified;

            await context.SaveChangesAsync();
        }
    }
}

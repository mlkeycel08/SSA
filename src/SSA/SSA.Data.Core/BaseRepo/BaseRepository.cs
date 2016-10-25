using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using SSA.Core.Extensions;

namespace SSA.Data.Core.BaseRepo
{
    public class BaseRepository<T> : IBaseRepository<T> where T : EntityObject
    {
        protected DbContext Db;
        protected DbSet<T> DbSet;

        public BaseRepository(DbContext context)
        {
            Db = context;
            DbSet = Db.Set<T>();
        }

        public virtual void Add(T entity)
        {
            DbSet.Add(entity);
        }

        public virtual void Update(T entity)
        {
            DbSet.Attach(entity);
            Db.Entry(entity).State = EntityState.Modified;
        }

        public virtual void Delete(T entity)
        {
            DbSet.Remove(entity);
        }

        public virtual void Delete(Expression<Func<T, bool>> where)
        {
            var objects = DbSet.Where(where).AsEnumerable();
            foreach (var obj in objects)
                DbSet.Remove(obj);
        }
        public int Count(Expression<Func<T, bool>> where)
        {
            return DbSet.Count(where);
        }
        public T Get(Expression<Func<T, bool>> where)
        {
            return DbSet.Where(where).FirstOrDefault();
        }

        public virtual T GetById(long id)
        {
            return DbSet.Find(id);
        }

        public virtual T GetById(string id)
        {
            return DbSet.Find(id);
        }

        public Task<T> GetAsync(Expression<Func<T, bool>> where)
        {
            return DbSet.Where(where).FirstOrDefaultAsync();
        }

        public virtual IEnumerable<T> GetAll()
        {
            return DbSet.ToList();
        }

        public virtual IEnumerable<T> GetMany(Expression<Func<T, bool>> where)
        {
            return DbSet.Where(where).ToList();
        }

        public Task<List<T>> GetManyAsync(Expression<Func<T, bool>> where)
        {
            return DbSet.Where(where).ToListAsync();
        }

        public IQueryable<T> GetQueryable(Expression<Func<T, bool>> where)
        {
            return DbSet.Where(where);
        }

        //public IQueryable<TResult> GetQueryable<TResult>(Expression<Func<T, bool>> where, Expression<Func<T, TResult>> select) 
        //{
        //    return DbSet.Where(where).OrderBy(w => "Id").Select(select);
        //}
      
        public void Save()
        {
            Db.SaveChanges();
        }
    }
}
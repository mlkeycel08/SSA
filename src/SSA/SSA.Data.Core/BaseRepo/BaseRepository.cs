using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SSA.Data.Core.BaseRepo
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
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

        public virtual T GetById(long id)
        {
            return DbSet.Find(id);
        }

        public virtual T GetById(string id)
        {
            return DbSet.Find(id);
        }

        public int Count(Expression<Func<T, bool>> where)
        {
            return DbSet.Count(where);
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

        public void Save()
        {
            Db.SaveChanges();
        }

        ///// <summary>
        /////     Return a paged list of entities
        ///// </summary>
        ///// <typeparam name="TOrder"></typeparam>
        ///// <param name="page">Which page to retrieve</param>
        ///// <param name="where">Where clause to apply</param>
        ///// <param name="order">Order by to apply</param>
        ///// <returns></returns>
        //public virtual IPagedList<T> GetPage<TOrder>(Page page, Expression<Func<T, bool>> where, Expression<Func<T, TOrder>> order)
        //{
        //    var results = DbSet.OrderBy(order).Where(where).GetPage(page).ToList();
        //    var total = DbSet.Count(where);
        //    return new StaticPagedList<T>(results, page.PageNumber, page.PageSize, total);
        //}
        public T Get(Expression<Func<T, bool>> where)
        {
            return DbSet.Where(where).FirstOrDefault();
        }
    }
}
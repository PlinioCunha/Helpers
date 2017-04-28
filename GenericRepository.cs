using Core.EntityFramework;
using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace Core.Data
{
    public class GenericRepository<T> : IDisposable, IGenericRepository<T> where T : class
    {

        private readonly EntityFramework.PadraoContext _context;

        public PadraoContext getContext
        {
            get { return EntityFramework.ContextManager.Current; }
        }
            
        public GenericRepository()
        {
            this._context = getContext;
        }

        public void Insert(T entity)
        {            
            _context.Entry(entity).State = System.Data.Entity.EntityState.Added;            
        }
        

        public void Delete(T entity)
        {
            _context.Entry(entity).State = System.Data.Entity.EntityState.Deleted;            
        }

        public void Update(T entity)
        {
            _context.Entry(entity).State = System.Data.Entity.EntityState.Modified;            
        }

        public IQueryable<T> SearchFor(System.Linq.Expressions.Expression<Func<T, bool>> predicate = null)
        {
            return (predicate == null ? _context.Set<T>() : _context.Set<T>().Where(predicate));
        }

        public IQueryable<T> OrderByField(IQueryable<T> q, string SortField, bool Ascending)
        {
            var param = Expression.Parameter(typeof(T), "p");
            var prop = Expression.Property(param, SortField);
            var exp = Expression.Lambda(prop, param);
            string method = Ascending ? "OrderBy" : "OrderByDescending";
            Type[] types = new Type[] { q.ElementType, exp.Body.Type };
            var mce = Expression.Call(typeof(Queryable), method, types, q.Expression, exp);
            return q.Provider.CreateQuery<T>(mce);
        }

        public IQueryable<T> SearchFor(string coluna, bool orderbydesc = false, System.Linq.Expressions.Expression<Func<T, bool>> predicate = null)
        {            
            IQueryable<T> lista;
            IQueryable<T> listaDados = (predicate == null ? _context.Set<T>() : _context.Set<T>().Where(predicate));
            lista = listaDados.AsQueryable();

            return String.IsNullOrEmpty(coluna) ? lista : OrderByField(lista, coluna, orderbydesc);
        }

        public IQueryable<T> SearchFor(string coluna, bool orderbydesc = false, int start = 1, int size = 25, System.Linq.Expressions.Expression<Func<T, bool>> predicate = null)
        {
            IQueryable<T> lista;
            IQueryable<T> listaDados = (predicate == null ? _context.Set<T>() : _context.Set<T>().Where(predicate));
            lista = listaDados.AsQueryable();            

            return String.IsNullOrEmpty(coluna) ? lista.Skip(((start - 1) * size)).Take(size) : OrderByField(lista, coluna, orderbydesc).Skip(((start - 1) * size)).Take(size);
        }

        public IQueryable<T> SearchFor(string coluna, bool orderbydesc = false, int start = 1, int size = 25, System.Linq.Expressions.Expression<Func<T, bool>> predicate = null, out int totalRows)
        {
            IQueryable<T> lista;
            IQueryable<T> listaDados = (predicate == null ? _context.Set<T>() : _context.Set<T>().Where(predicate));
            lista = listaDados.AsQueryable();

            totalRows = lista.Count();

            return String.IsNullOrEmpty(coluna) ? lista.Skip(((start - 1) * size)).Take(size) : OrderByField(lista, coluna, orderbydesc).Skip(((start - 1) * size)).Take(size);
        }

        public int CommandSQL(String SQL, params object[] parameters)
        {
            return _context.Database.ExecuteSqlCommand(SQL, parameters);
        }

        public IQueryable<T> AllIncluding(params System.Linq.Expressions.Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _context.Set<T>();
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query;
        }
        
        public T DetalhePredicate(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().FirstOrDefault(predicate);
        }

        public T Detalhe(Int32 id)
        {
            return _context.Set<T>().Find(id);
        }
        public T Detalhe(Guid id)
        {
            return _context.Set<T>().Find(id);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}

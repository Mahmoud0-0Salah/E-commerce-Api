using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IRepositoryBase<T>
    {

        IQueryable<T> FindAll(bool trackChanges = true);
        IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression,
        bool trackChanges = true);
        void Create(T entity);
        void Update(T entity);
        void Delete(T entity);
        void UpdteRange(IEnumerable<T> range);
        void DeleteRange(IEnumerable<T> range);
        void CreateeRange(IEnumerable<T> range);
    }
}

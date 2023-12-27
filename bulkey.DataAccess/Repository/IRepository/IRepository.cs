using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace bulkey.DataAccess.Repository.IRepository
{
    public interface IRepository<T> where T:class 
    {
        // T - Category   
        IEnumerable<T> GetAll(string? includePropreties = null);// Get All record form database.
        T Get(Expression<Func<T, bool>> filter, string? includePropreties = null); //Get Single record form database.
        void Add(T entity);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entity);

    }
}

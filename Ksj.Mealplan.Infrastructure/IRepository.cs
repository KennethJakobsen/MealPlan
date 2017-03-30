using System.Collections.Generic;
using System.ComponentModel;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Ksj.Mealplan.Infrastructure
{
    public interface IRepository<T>
    {
        Task Add(T entity);
        Task<IEnumerable<T>> GetAll();
        Task<T> Find(string name);
        Task<IEnumerable<T>> Search(string name);
    }
}
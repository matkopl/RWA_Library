using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services
{
    public interface IRepository<T>
    {
        public IEnumerable<T> GetAll();
        public T Get(int id);
        public T Create(T value);
        public T Edit(int id, T value);
        public T Delete(int id);
    }
}

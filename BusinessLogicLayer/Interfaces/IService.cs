using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Interfaces
{
    public interface IService<T>
    {
        void Add(T item);
        void Remove(T item);
        void Update(T oldItem, T newItem);
        List<T> GetAll();
    }
}

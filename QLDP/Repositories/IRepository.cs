using QLDP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace QLDP.Repositories
{
    public interface IRepository<T> 
    {
        OperationResult Save(T t);
        T FindById(int id);
        List<T> FindByName(string name);
        List<T> FindAll();
        OperationResult UpdateColumn(int id, string column, string newValue);
        OperationResult UpdateEntity(T t);
        void SaveAll(List<T> data);
        List<T> FindAllByColumn(string column, object value);
        OperationResult Delete(int id);
    }

}

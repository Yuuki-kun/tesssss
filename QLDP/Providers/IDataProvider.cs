using QLDP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLDP.Providers
{
    public interface IDataProvider<T>
    {
        List<T> LoadData();
        OperationResult SaveData(T data);
        T FindById(int id);
        List<T> FindByName(string name);

        OperationResult UpdateColumn(int id, string column, string newValue);
        OperationResult UpdateEntity(T entity);
        OperationResult Delete(int id);

        //get list data by field 
        List<T> FindAllByColumn(string column, object value);
        void SaveAll(List<T> data);

        List<T> FindByPage(int p, int s, int d);
    }
}

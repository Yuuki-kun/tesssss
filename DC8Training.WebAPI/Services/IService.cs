using QLDP.Models;

namespace DC8Training.WebAPI.Services
{
    public interface IService<T>
    {
        T FindById(int id);
        List<T> FindByName(string name);
        OperationResult Create(T t);
        OperationResult CreateOrUpdate(T t);
        OperationResult Delete(int id);
        ModelState Validate(T t);
        OperationResult UpdateColumn(int id, string column, string value);
        OperationResult UpdateEntity( T t);
        List<T> GetAll();
        List<T> GetByPage(int page, int size, int d);
    }
}

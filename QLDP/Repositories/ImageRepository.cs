using QLDP.Models;
using QLDP.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLDP.Repositories
{
    public class ImageRepository : IImageRepository
    {
        private readonly IDataProvider<Image> _imageProvider;

        public ImageRepository(IDataProvider<Image> imageProvider)
        {
            _imageProvider = imageProvider;
        }

        public OperationResult Delete(int id)
        {
            if (id <= 0) throw new ArgumentOutOfRangeException("id");
            return _imageProvider.Delete(id);
        }

        public List<Image> FindAll()
        {
            return _imageProvider.LoadData();
        }

        public List<Image> FindAllByColumn(string column, object value)
        {
            return _imageProvider.FindAllByColumn(column, value);
        }

        public Image FindById(int id)
        {
            return _imageProvider.FindById(id);
        }

        public List<Image> FindByName(string name)
        {
            return _imageProvider.FindByName(name);
        }

        public Image Save(Image t)
        {
            return null;
            //return _imageProvider.SaveData(t);
        }

        public void SaveAll(List<Image> data)
        {
            _imageProvider.SaveAll(data);
        }

        public OperationResult UpdateColumn(int id, string column, string newValue)
        {
            return _imageProvider.UpdateColumn(id, column, newValue);
        }

        public OperationResult UpdateEntity(Image t)
        {
            throw new NotImplementedException();
        }

        OperationResult IRepository<Image>.Save(Image t)
        {
            throw new NotImplementedException();
        }
    }
}

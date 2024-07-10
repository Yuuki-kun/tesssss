using QLDP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLDP.Repositories
{
    public interface IMedicineRepository : IRepository<Medicine>
    {
        List<Medicine> GetByPage(int page, int size, int d);
        
    }
}

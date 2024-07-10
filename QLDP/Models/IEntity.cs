using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLDP.Models
{
    public interface IEntity
    {
        int GetId();
        string GetName();
    }
}

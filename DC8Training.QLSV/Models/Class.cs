using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DC8Training.QLSV.Models
{
    internal class Class
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<int> StudentIds { get; set; }
        public int TeacherId { get; set; }
    }
}

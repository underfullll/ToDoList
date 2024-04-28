using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BD
{
    public interface Interface
    {
        void create(string name, string more);
        void read();
        void Update(int id, string name);
        void delete(int id);
    }
}

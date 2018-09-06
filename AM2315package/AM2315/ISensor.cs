using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AM2315package.AM2315
{
    public interface ISensor
    {
        int Connect(int addr);
        int Read();
    }
}

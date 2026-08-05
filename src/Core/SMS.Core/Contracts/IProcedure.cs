using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMS.Core.Contracts
{
    public interface IProcedure
    {
        object Call();
        object Call(object obj);
        string GetName();
    }
}

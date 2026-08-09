using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMS.Core.Managers
{
    public class Response
    {
        public int Id { get; set; }
        public string Message { get; set; } = string.Empty;
        public int CreatedBy { get; set; }
        public string Name { get; set; } = string.Empty;

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keybie
{
    class Config
    {
        public List<RunProcessAction> Action { get; set; }
        public bool AutoConnect { get; set; }
        public string ComPort { get; set; }
        public int BaudRate { get; set; }
    }
}

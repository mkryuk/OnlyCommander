using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlyCommander
{
    enum PType 
    {
        File = 1,
        Directory = 2
    }
    class Item
    {
        public string Path { get; set; }
        public PType Type { get; set; }

        public Item(string path = null, PType type = default(PType))
        {
            Path = path;
            Type = type;
        }
    }
}

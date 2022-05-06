using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestConsole
{
    internal interface ISomeModel
    {
        string ModelType { get; }
    }

    internal class SomeModelA : ISomeModel
    {
        public string ModelType => "a";

        public string Name { get; set; }
    }

    internal class SomeModelB : ISomeModel
    {
        public string ModelType => "b";

        public string Description { get; set; }
    }
}

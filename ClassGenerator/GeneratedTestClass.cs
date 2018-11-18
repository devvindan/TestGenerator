using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassGenerator
{
    public class GeneratedTestClass
    {

        public string Name { get; }
        public string Code { get; }

        public GeneratedTestClass(string name, string code)
        {
            Name = name;
            Code = code;
        }

    }
}

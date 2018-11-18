using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;


namespace ClassGenerator
{
    public static class Generator
    {

        private static int maxInputCount;
        private static int maxProcessingCount;
        private static int maxOutputCount;

        public static Task<string> Generate(string code)
        {
            return Task.Run(() => { return code + "generated_test_class"; });
        }

    }
}

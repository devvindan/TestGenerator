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




        // method that generates test classes for each class found in code
        public static Task<List<GeneratedTestClass>> Generate(string code)
        {        
            return Task.Run(() => {


                // using Roslyn to parse/build syntax tree
                var tree = CSharpSyntaxTree.ParseText(code);
                var syntaxRoot = tree.GetRoot();

                var classDeclarations = syntaxRoot.DescendantNodes().OfType<ClassDeclarationSyntax>();
                
                foreach (var clsInfo in classDeclarations)
                {
                    var publicMethods = clsInfo.DescendantNodes().OfType<MethodDeclarationSyntax>().Where(
                        method => method.Modifiers.Any(modifier => modifier.ValueText == "public"));
                }


                List<GeneratedTestClass> generatedClasses = null;
                return generatedClasses;

            });
        }

    }
}

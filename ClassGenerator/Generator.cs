using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace ClassGenerator
{
    public static class Generator
    {

        private static SyntaxList<UsingDirectiveSyntax> getTemplateUsing(string namespaceName)
        {
            List<UsingDirectiveSyntax> template = new List<UsingDirectiveSyntax>
            {
                UsingDirective(IdentifierName("System")),
                UsingDirective(QualifiedName(
                    IdentifierName("System"),
                    IdentifierName("Linq"))),
                UsingDirective(QualifiedName(
                    QualifiedName(
                        IdentifierName("System"), IdentifierName("Collections")),
                        IdentifierName("Generic"))),
                UsingDirective(
                    QualifiedName(
                        QualifiedName(
                            QualifiedName(
                                IdentifierName("Microsoft"),
                                IdentifierName("VisualStudio")),
                            IdentifierName("TestTools")),
                    IdentifierName("UnitTesting"))),
                UsingDirective(IdentifierName(namespaceName))
            };

            return List(template);

        }

        // method that generates test classes for each class found in code
        public static Task<List<GeneratedTestClass>> Generate(string code)
        {        
            return Task.Run(() => {

                List<GeneratedTestClass> generatedClasses;

                // using Roslyn to parse/build syntax tree
                var tree = CSharpSyntaxTree.ParseText(code);
                var syntaxRoot = tree.GetRoot();

                var classDeclarations = syntaxRoot.DescendantNodes().OfType<ClassDeclarationSyntax>();
                
                foreach (var clsInfo in classDeclarations)
                {

                    // get class name, namespace name and public methods

                    string className = clsInfo.Identifier.ValueText;
                    string clsNamespace = ((NamespaceDeclarationSyntax)clsInfo.Parent).Name.ToString();

                    var publicMethods = clsInfo.DescendantNodes().OfType<MethodDeclarationSyntax>().Where(
                        method => method.Modifiers.Any(modifier => modifier.ValueText == "public"));

                    // use roslyn to generate test class code

                    // declare namespace
                    NamespaceDeclarationSyntax generated_namespace = NamespaceDeclaration(
                        QualifiedName(
                            IdentifierName(className), IdentifierName("Test")));

                    var template_usings = getTemplateUsing(clsNamespace);

                    var template_methods = getTemplateMethods(publicMethods);


                    

                }



                return generatedClasses;

            });
        }

    }
}

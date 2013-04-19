using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Microsoft.CSharp;
using System.Globalization;

namespace August2008.Common.Tools
{
    public class ClassBuilder
    {
        public const string OpPropertySeperator = ".";
        public const string OpWhitespace = " ";
        public const string OpComma = ",";
        public const string OpSemicolon = ";";
        public const string OpAnd = "&&";
        public const string OpOr = "||";
        public const string OpRParen = ")";
        public const string OpLParen = "(";
        public const string OpDq = @"""";
        public const string OpRBrckt = @"}";
        public const string OpLBrckt = @"{";
        public const string OpNot = "!";
        public const string OpSet = "=";

        public const string KeywordReturn = "return";
        public const string KeywordIf = "if";
        public const string KeywordElse = "else";
        public const string KeywordTrue = "true";
        public const string KeywordFalse = "false";
        public const string KeywordAssetId = "AssetId";
        public const string KeywordDate = "Date";
        public const string KeywordValue = "value";

        public const string SymUnderscore = "_";

        public const string CompEquals = "==";
        public const string CompNotequals = "!=";
        public const string CompLessthan = "<";
        public const string CompLessthanequals = "<=";
        public const string CompGreaterthan = ">";
        public const string CompGreaterthanequals = ">=";

        private readonly CodeCompileUnit _compUnit;
        private readonly CodeNamespace _namespace;
        private readonly CodeTypeDeclaration _class;
        private readonly List<Assembly> _assemblies;

        public ClassBuilder(string namespc, string className)
            : this(namespc, className, MemberAttributes.Public)
        {
        }
        public ClassBuilder(string namespc, string className, MemberAttributes attributes)
        {
            _compUnit = new CodeCompileUnit();
            _assemblies = new List<Assembly>();

            _namespace = new CodeNamespace(namespc);
            _class = new CodeTypeDeclaration(className) {Attributes = attributes};

            _compUnit.Namespaces.Add(_namespace);
            _namespace.Types.Add(_class);
        }
        public void AddReference(Assembly assembly)
        {
            if (!_assemblies.Contains(assembly))
                _assemblies.Add(assembly);
        }
        public void InheritFrom(Type baseType)
        {
            _class.BaseTypes.Add(baseType);
        }

        public byte[] GeneratedAssembly { get; set; }

        public void AddImport(string value)
        {
            _namespace.Imports.Add(new CodeNamespaceImport(value));
        }
        public void AddClassAttribute(string attributeName)
        {
            var cad = new CodeAttributeDeclaration(attributeName);
            this.AddCustomAttributes(cad);
        }
        public CodeConstructor GetConstructor()
        {
            var constructor = new CodeConstructor {Attributes = MemberAttributes.Public};
            _class.Members.Add(constructor);

            return constructor;
        }
        public void AddProperty(string propertyName, Type propertyType)
        {
            var cmf = new CodeMemberField
                {
                    Attributes = MemberAttributes.Private,
                    Name = SymUnderscore + propertyName,
                    Type = new CodeTypeReference(propertyType)
                };
            _class.Members.Add(cmf);

            var cmp = new CodeMemberProperty
                {
                    Name = propertyName,
                    HasGet = true,
                    HasSet = true,
                    Type = new CodeTypeReference(propertyType),
                    Attributes = MemberAttributes.Public
                };

            cmp.GetStatements.Add(new CodeSnippetExpression(KeywordReturn + OpWhitespace + cmf.Name));
            cmp.SetStatements.Add(new CodeSnippetExpression(cmf.Name + OpSet + KeywordValue));

            _class.Members.Add(cmp);
        }
        public CodeMemberMethod CreateMethod(string methodName, Type returnType, MemberAttributes attributes)
        {
            var ctr = new CodeTypeReference(returnType);
            var method = new CodeMemberMethod {Name = methodName, Attributes = attributes, ReturnType = ctr};
            _class.Members.Add(method);
            return method;
        }
        public void AddMethodBody(CodeMemberMethod method, string statement)
        {
            method.Statements.Add(new CodeExpressionStatement(new CodeSnippetExpression(statement)));
        }
        public void AddParameter(CodeMemberMethod method, Type parameterType, string parameterName)
        {
            method.Parameters.Add(new CodeParameterDeclarationExpression(parameterType, parameterName));
        }
        public void SetAttributes(MemberAttributes attributes)
        {
            _class.Attributes = attributes;
        }
        public void AddCustomAttributes(CodeAttributeDeclaration customAttribute)
        {
            _class.CustomAttributes.Add(customAttribute);
        }
        public object GetInstance()
        {
            var compParams = new CompilerParameters(new string[] { "mscorlib.dll" })
                {
                    GenerateInMemory = true,
                    IncludeDebugInformation = false,
                    CompilerOptions = "/optimize"
                };

            // this is a temporary fix to not load assemblies from v3.5 directory
            // one possible fix is to find out where the assembly is comming from
            // 
            foreach (var assemblyName in this.GetType().Assembly.GetReferencedAssemblies())
            {
                if (assemblyName.Name.StartsWith("System")
                    && !assemblyName.Name.StartsWith("System.Core")
                    && !assemblyName.Name.StartsWith("System.ServiceModel.Web")
                    && !assemblyName.Name.StartsWith("System.Runtime.Serialization"))
                {
                    compParams.ReferencedAssemblies.Add(assemblyName.Name + ".dll");
                }
            }
            foreach (var assemblyName in Assembly.GetCallingAssembly().GetReferencedAssemblies())
            {
                if (!assemblyName.Name.StartsWith("System", false, CultureInfo.InvariantCulture) ||
                    assemblyName.Name.StartsWith("System.Core") ||
                    assemblyName.Name.StartsWith("System.ServiceModel.Web") ||
                    assemblyName.Name.StartsWith("System.Runtime.Serialization")) continue;

                var name = assemblyName.Name + ".dll";
                if (!compParams.ReferencedAssemblies.Contains(name))
                {
                    compParams.ReferencedAssemblies.Add(name);
                }
            }
            foreach (var assembly in _assemblies)
            {
                compParams.ReferencedAssemblies.Add(assembly.Location);
            }
            var compResults = GetCsCompiler().CompileAssemblyFromDom(compParams, _compUnit);
            object instance;
            if (compResults.Errors.HasErrors == true)
            {
                var sb = new StringBuilder();
                for (var i = 0; i < compResults.Errors.Count; i++)
                {
                    sb.Append(compResults.Errors[i].ErrorText);
                    sb.Append(Environment.NewLine + Environment.NewLine);
                }
                System.Diagnostics.Debug.WriteLine(
                    Environment.NewLine +
                    "DataReaderEngine compiler error(s): " +
                    sb.ToString() +
                    Environment.NewLine);
                instance = null;
            }
            else
            {
                var loadedAssembly = compResults.CompiledAssembly;
                instance = loadedAssembly.CreateInstance(_namespace.Name + "." + _class.Name); ;
            }
            return instance;
        }
        private static CodeDomProvider GetCsCompiler()
        {
            return new Microsoft.CSharp.CSharpCodeProvider();
        }
        public void WriteFile(string fileName)
        {
            var csharp = new CSharpCodeProvider();
            var options = new CodeGeneratorOptions
                {
                    BlankLinesBetweenMembers = true,
                    BracingStyle = "C",
                    ElseOnClosing = false,
                    IndentString = "    "
                };
            var writer = new StreamWriter(fileName);

            csharp.GenerateCodeFromCompileUnit(_compUnit, writer, options);
            writer.Close();
        }
        public void Write(TextWriter writer)
        {
            var csharp = new CSharpCodeProvider();
            var options = new CodeGeneratorOptions
                {
                    BlankLinesBetweenMembers = true,
                    BracingStyle = "C",
                    ElseOnClosing = false,
                    IndentString = "    "
                };
            csharp.GenerateCodeFromCompileUnit(_compUnit, writer, options);
            writer.Close();
        }
    }
}

using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Diagnostics;
using Microsoft.CSharp;
using System.Globalization;

namespace August2008.Common
{
    public class ClassBuilder
    {
        public const string OP_PROPERTY_SEPERATOR = ".";
        public const string OP_WHITESPACE = " ";
        public const string OP_COMMA = ",";
        public const string OP_SEMICOLON = ";";
        public const string OP_AND = "&&";
        public const string OP_OR = "||";
        public const string OP_R_PAREN = ")";
        public const string OP_L_PAREN = "(";
        public const string OP_DQ = @"""";
        public const string OP_R_BRCKT = @"}";
        public const string OP_L_BRCKT = @"{";
        public const string OP_NOT = "!";
        public const string OP_SET = "=";

        public const string KEYWORD_RETURN = "return";
        public const string KEYWORD_IF = "if";
        public const string KEYWORD_ELSE = "else";
        public const string KEYWORD_TRUE = "true";
        public const string KEYWORD_FALSE = "false";
        public const string KEYWORD_ASSET_ID = "AssetId";
        public const string KEYWORD_DATE = "Date";
        public const string KEYWORD_VALUE = "value";

        public const string SYM_UNDERSCORE = "_";

        public const string COMP_EQUALS = "==";
        public const string COMP_NOTEQUALS = "!=";
        public const string COMP_LESSTHAN = "<";
        public const string COMP_LESSTHANEQUALS = "<=";
        public const string COMP_GREATERTHAN = ">";
        public const string COMP_GREATERTHANEQUALS = ">=";

        private CodeCompileUnit _compUnit;
        private CodeNamespace _namespace;
        private CodeTypeDeclaration _class;
        private List<Assembly> _assemblies;
        private byte[] _generatedAssembly;

        public ClassBuilder(string namespc, string className)
            : this(namespc, className, MemberAttributes.Public)
        {
        }
        public ClassBuilder(string namespc, string className, MemberAttributes attributes)
        {
            _compUnit = new CodeCompileUnit();
            _assemblies = new List<Assembly>();

            _namespace = new CodeNamespace(namespc);
            _class = new CodeTypeDeclaration(className);
            _class.Attributes = attributes;

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
        public byte[] GeneratedAssembly
        {
            get { return _generatedAssembly; }
            set { _generatedAssembly = value; }
        }
        public void AddImport(string value)
        {
            _namespace.Imports.Add(new CodeNamespaceImport(value));
        }
        public void AddClassAttribute(string attributeName)
        {
            CodeAttributeDeclaration cad = new CodeAttributeDeclaration(attributeName);
            this.AddCustomAttributes(cad);
        }
        public CodeConstructor GetConstructor()
        {
            CodeConstructor constructor = new CodeConstructor();
            constructor.Attributes = MemberAttributes.Public;
            _class.Members.Add(constructor);

            return constructor;
        }
        public void AddProperty(string propertyName, Type propertyType)
        {
            CodeMemberField cmf = new CodeMemberField();
            cmf.Attributes = MemberAttributes.Private;
            cmf.Name = SYM_UNDERSCORE + propertyName;
            cmf.Type = new CodeTypeReference(propertyType);
            _class.Members.Add(cmf);

            CodeMemberProperty cmp = new CodeMemberProperty();
            cmp.Name = propertyName;
            cmp.HasGet = true;
            cmp.HasSet = true;
            cmp.Type = new CodeTypeReference(propertyType);
            cmp.Attributes = MemberAttributes.Public;

            cmp.GetStatements.Add(new CodeSnippetExpression(KEYWORD_RETURN + OP_WHITESPACE + cmf.Name));
            cmp.SetStatements.Add(new CodeSnippetExpression(cmf.Name + OP_SET + KEYWORD_VALUE));

            _class.Members.Add(cmp);
        }
        public CodeMemberMethod CreateMethod(string methodName, Type returnType, MemberAttributes attributes)
        {
            CodeTypeReference ctr = new CodeTypeReference(returnType);
            CodeMemberMethod method = new CodeMemberMethod();
            method.Name = methodName;
            method.Attributes = attributes;
            method.ReturnType = ctr;
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
            CompilerParameters compParams = new CompilerParameters(new string[] { "mscorlib.dll" });
            compParams.GenerateInMemory = true;
            compParams.IncludeDebugInformation = false;
            compParams.CompilerOptions = "/optimize";

            // this is a temporary fix to not load assemblies from v3.5 directory
            // one possible fix is to find out where the assembly is comming from
            // 
            foreach (AssemblyName assemblyName in this.GetType().Assembly.GetReferencedAssemblies())
            {
                if (assemblyName.Name.StartsWith("System")
                    && !assemblyName.Name.StartsWith("System.Core")
                    && !assemblyName.Name.StartsWith("System.ServiceModel.Web")
                    && !assemblyName.Name.StartsWith("System.Runtime.Serialization"))
                    compParams.ReferencedAssemblies.Add(assemblyName.Name + ".dll");
            }
            foreach (AssemblyName assemblyName in Assembly.GetCallingAssembly().GetReferencedAssemblies())
            {
                if (assemblyName.Name.StartsWith("System", false, CultureInfo.InvariantCulture)
                    && !assemblyName.Name.StartsWith("System.Core")
                    && !assemblyName.Name.StartsWith("System.ServiceModel.Web")
                    && !assemblyName.Name.StartsWith("System.Runtime.Serialization"))
                {
                    string name = assemblyName.Name + ".dll";
                    if (!compParams.ReferencedAssemblies.Contains(name))
                    {
                        compParams.ReferencedAssemblies.Add(name);
                    }
                }
            }
            foreach (Assembly assembly in _assemblies)
                compParams.ReferencedAssemblies.Add(assembly.Location);

            CompilerResults compResults = GetCSCompiler().CompileAssemblyFromDom(compParams, _compUnit);

            object instance;
            if (compResults.Errors.HasErrors == true)
            {
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < compResults.Errors.Count; i++)
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
                Assembly loadedAssembly = compResults.CompiledAssembly;
                instance = loadedAssembly.CreateInstance(_namespace.Name + "." + _class.Name); ;
            }
            return instance;
        }
        private CodeDomProvider GetCSCompiler()
        {
            return new Microsoft.CSharp.CSharpCodeProvider();
        }
        public void WriteFile(string fileName)
        {
            CSharpCodeProvider csharp = new CSharpCodeProvider();
            CodeGeneratorOptions options = new CodeGeneratorOptions();
            options.BlankLinesBetweenMembers = true;
            options.BracingStyle = "C";
            options.ElseOnClosing = false;
            options.IndentString = "    ";
            StreamWriter writer = new StreamWriter(@"c:\\" + fileName);

            csharp.GenerateCodeFromCompileUnit(_compUnit, writer, options);
            writer.Close();
        }
        public void Write(TextWriter writer)
        {
            CSharpCodeProvider csharp = new CSharpCodeProvider();
            CodeGeneratorOptions options = new CodeGeneratorOptions();
            options.BlankLinesBetweenMembers = true;
            options.BracingStyle = "C";
            options.ElseOnClosing = false;
            options.IndentString = "    ";
            csharp.GenerateCodeFromCompileUnit(_compUnit, writer, options);
            writer.Close();
        }
    }
}

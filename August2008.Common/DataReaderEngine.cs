using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;
using System.CodeDom;
using System.Configuration;
using System.Diagnostics;
using MelaniArt.Core.DataAccess;
using System.Text.RegularExpressions;
using System.Data.Common;

namespace MelaniArt.Core.Tools
{ 
    internal sealed class DataReaderEngine
    {
        const string MethodName = "Fetch";
        const string ItemName = "item";

        static Regex invalidChars = new Regex(@"[~`@#$%^&*()\-=+{}:;,|\\\/?<>]|\s+", RegexOptions.Compiled);
        static Regex sprocNameTrimmer = new Regex(@"(\[\w+\]\.|\w+\.)?", RegexOptions.Compiled);
        static Regex whiteSpace = new Regex(@"\s+", RegexOptions.Compiled);
        static Regex parameter = new Regex(@"@[a-zA-Z]+\w+", RegexOptions.Compiled);
        static Hashtable instanceTable = new Hashtable();
        static string @namespace = typeof(DataReaderEngine).Namespace;

        static DataReaderEngine()
        {
            AppDomain.CurrentDomain.DomainUnload += delegate(object sender, EventArgs e)
                {
                    // unload stuff...
                };
        }
        private DataReaderEngine()
        {
        }
        internal DataReaderEngine(Database dataProvider)
        {
            if (dataProvider == null)
                throw new ArgumentNullException("dataProvider");
            this.DataProvider = dataProvider;
        }
        internal void Execute(params object[] args)
        {
            string @class;
            if (!instanceTable.ContainsKey((@class = this.ValidateArgs(args))))
            {
                lock (instanceTable.SyncRoot)
                {
                    if (!instanceTable.ContainsKey(@class))
                    {
                        using (StringWriter writer = new StringWriter(CultureInfo.InvariantCulture))
                        using (DbDataReader reader = this.DataProvider.ExecuteReader(this.CommandBehavior))
                        {
                            ClassBuilder builder = new ClassBuilder(@namespace, @class, MemberAttributes.Static);

                            this.AddExternalAssemblyReferences(builder, args);

                            builder.AddImport("System");
                            builder.AddImport("System.Collections");
                            builder.AddImport("System.Data");
                            builder.AddImport("System.Globalization");

                            CodeMemberMethod method = builder.CreateMethod(MethodName, typeof(bool), MemberAttributes.Static);
                            builder.AddParameter(method, typeof(DbDataReader), "reader");
                            builder.AddParameter(method, typeof(object[]), "args");

                            int counter = 0;
                            writer.WriteLine("// reads data from \"{0}\" procedure.", this.DataProvider.CommandText);
                            writer.WriteLine();
                            do
                            {
                                if ((args.Length - 1) >= counter)
                                {
                                    DataTable schemaTable = reader.GetSchemaTable();
                                    Type argType = args[counter].GetType();
                                    string instanceName = null;

                                    if (args[counter] is IEnumerable)
                                    {
                                        Type genericType = argType.GetGenericArguments()[0];

                                        writer.WriteLine("IList list{0} = args[{1}] as IList;", counter, counter);
                                        writer.WriteLine("");
                                        writer.WriteLine("while (reader.Read())");
                                        writer.WriteLine("{");

                                        if (IsNativeType(genericType))
                                        {
                                            this.WriteNativeDataTypeReaderLogic(writer, schemaTable, genericType);
                                        }
                                        else
                                        {
                                            instanceName = this.ValidateInstanceName(genericType);
                                            writer.WriteLine("{0} {1} = new {2}();", instanceName, ItemName, instanceName);
                                            this.WriteCommonDataReaderLogic(writer, schemaTable, genericType, null, null, ItemName);
                                        }
                                        writer.WriteLine("list{0}.Add(item);", counter);
                                        writer.WriteLine("}");
                                    }
                                    else
                                    {
                                        instanceName = this.ValidateInstanceName(argType);

                                        writer.WriteLine("if (reader.Read())");
                                        writer.WriteLine("{");
                                        writer.WriteLine("{0} {1} = ({2})args[{3}];", instanceName, ItemName, instanceName, counter);

                                        this.WriteCommonDataReaderLogic(writer, schemaTable, argType, null, null, ItemName);

                                        writer.WriteLine("}");
                                    }
                                    if ((counter + 1) != args.Length)
                                    {
                                        writer.WriteLine("if (!reader.NextResult())");
                                        writer.WriteLine("{");
                                        writer.WriteLine("return true;");
                                        writer.WriteLine("}");
                                    }
                                    counter++;
                                }
                            }
                            while (reader.NextResult());

                            writer.WriteLine("return true;");

                            builder.AddMethodBody(method, writer.ToString());

#if DEBUG
                            using (StringWriter writer2 = new StringWriter(CultureInfo.InvariantCulture))
                            {
                                builder.Write(writer2);
                                System.Diagnostics.Debug.WriteLine(writer2.ToString());
                            }
#endif
                            object instance;
                            if ((instance = builder.GetInstance()) == null)
                            {
                                throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "ClassBuilder failed to create the instance for a stored procedure {0}.", this.DataProvider.CommandText));
                            }
                            instanceTable.Add(@class, instance);
                        }
                    }
                }
            }
            this.Execute(@class, args);
        }
        private void Execute(string className, object[] args)
        {
            object instance;
            if ((instance = instanceTable[className]) != null)
            {
                Type type = instance.GetType();
                MethodInfo method = type.GetMethod(MethodName, BindingFlags.NonPublic | BindingFlags.Static);
                using (DbDataReader reader = this.DataProvider.ExecuteReader())
                {
                    method.Invoke(instance, new object[] { reader, args });
                }
            }
        }
        private void WriteCommonDataReaderLogic(StringWriter writer, DataTable schemaTable, Type concreteType, Type declaringType, Type propertyType, string itemName)
        {
            this.ValidateSchemaTable(schemaTable, -1);

            foreach (DataRow row in schemaTable.Rows)
            {
                string columnName = row["ColumnName"].ToString();
                string clrDataTypeName = row["DataType"].ToString();
                string columnOrdinal = row["ColumnOrdinal"].ToString();
                bool allowDBNull = Convert.ToBoolean(row["AllowDBNull"]);

                PropertyInfo property = null;

                if ((property = GetProperty(columnName, concreteType, false)) != null)
                {

                    if (!allowDBNull)
                        writer.WriteLine("{0}.{1} = ({2})reader[{3}];", itemName, property.Name, clrDataTypeName, columnOrdinal);
                    else
                        writer.WriteLine("{0}.{1} = reader.IsDBNull({2}) ? {3} : ({4})reader[{5}];", itemName, property.Name, columnOrdinal, GetDefaultValueAsString(clrDataTypeName), clrDataTypeName, columnOrdinal);
                }
            }
        }
        private void WriteNativeDataTypeReaderLogic(StringWriter writer, DataTable schemaTable, Type type)
        {
            this.ValidateSchemaTable(schemaTable, 1);

            DataRow row = schemaTable.Rows[0];
            string columnName = row["ColumnName"].ToString();
            string clrDataTypeName = row["DataType"].ToString();
            string columnOrdinal = row["ColumnOrdinal"].ToString();
            bool allowDBNull = Convert.ToBoolean(row["AllowDBNull"]);

            if (!allowDBNull)
                writer.WriteLine("{0} item = ({1})reader[{2}];", clrDataTypeName, clrDataTypeName, columnOrdinal);
            else
                writer.WriteLine("{0} item = reader.IsDBNull({1}) ? {2} : ({3})reader[{4}];", clrDataTypeName, columnOrdinal, GetDefaultValueAsString(clrDataTypeName), clrDataTypeName, columnOrdinal);
        }
        private static PropertyInfo GetProperty(string name, Type declaringType, bool throwOnError)
        {
            PropertyInfo property = declaringType.GetProperty(name, BindingFlags.Public | BindingFlags.Instance);
            if (property == null && throwOnError)
            {
                throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Property name {0} does not exist in {1} type but it is defined in configuration settings.", name, declaringType.FullName));
            }
            return property;
        }
        private string ComposeClassName(object[] args)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(SprocNameToClassName(this.DataProvider.CommandText).ToLower());
            for (int i = 0; i < args.Length; i++)
            {
                sb.Append("_");
                Type type = args[i].GetType();
                if (type.IsGenericType)
                {
                    Type genericType = type.GetGenericArguments()[0];
                    sb.Append(type.Name);
                    sb.Append(genericType.Name);
                    continue;
                }
                sb.Append(type.Name);
            }
            return RemoveInvalidChars(sb.ToString());
        }
        private string ValidateArgs(object[] args)
        {
            if (args.Length == 0)
                throw new ArgumentException("At least one valid argument is required to create the wrapper.");
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == null)
                    throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Null argument passed at index {0}.", i));
                Type type = args[i].GetType();
                if (args[i] is IEnumerable && !IsNativeType(type))
                {
                    if (type.IsGenericType)
                    {
                        Type[] genericArgs = type.GetGenericArguments();
                        if (genericArgs.Length > 1)
                            throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Collection type {0} at index {1} contains more than one generic argument.", type.FullName, i));
                        type = genericArgs[0];
                    }
                    else
                    {
                        throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Collection type {0} at index {1} must be generic.", type.FullName, i));
                    }
                    if (!type.IsVisible)
                    {
                        throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Entity type {0} at index {1} is not visible outside the assmbly.", type.FullName, i));
                    }
                    continue;
                }
                if (IsNativeType(type))
                {
                    throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Argument at index {0} is of {1} type, which is not allowed.", type.FullName, i));
                }
            }
            return this.ComposeClassName(args);
        }
        private string ValidateInstanceName(Type type)
        {
            if (!type.IsNested)
                return type.FullName;
            else
                return type.FullName.Replace('+', '.');
        }
        private void ValidateSchemaTable(DataTable schemaTable, int minRows)
        {
            if (schemaTable.Rows.Count == 0)
            {
                throw new ArgumentException("Schema table does not produce any results.");
            }
            if (minRows != -1 && schemaTable.Rows.Count < minRows)
            {
                throw new ArgumentException("Schema table does not have minimum number of required rows.");
            }
        }
        private bool ValidateAutoConvertType(PropertyInfo property)
        {
            if (property.PropertyType.GetInterface("IConvertible") == null)
            {
                throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Property {0} in {1} type is not a valid \"autoConvert\" type.", property.Name, property.DeclaringType.FullName));
            }
            return true;
        }
        private void AddExternalAssemblyReferences(ClassBuilder builder, object[] args)
        {
            builder.AddReference(Assembly.GetExecutingAssembly());
            foreach (object item in args)
            {
                Type type = item.GetType();
                if (!IsNativeType(type))
                {
                    if (item is IEnumerable)
                        type = type.GetGenericArguments()[0];
                    builder.AddReference(Assembly.GetAssembly(type));
                }
            }
        }
        private static bool IsNativeType(Type type)
        {
            return ((type != null) &&
                    type == typeof(String) ||
                    type == typeof(Int16) ||
                    type == typeof(Int32) ||
                    type == typeof(Int64) ||
                    type == typeof(Decimal) ||
                    type == typeof(Double) ||
                    type == typeof(Single) ||
                    type == typeof(Byte) ||
                    type == typeof(Boolean) ||
                    type == typeof(DateTime) ||
                    type == typeof(Char) ||
                    type == typeof(Guid));
        }
        private static bool IsSprocName(string sproc)
        {
            return (!string.IsNullOrEmpty(sproc) && !invalidChars.IsMatch(sproc));
        }
        private static string SprocNameToClassName(string sprocName)
        {
            if (!IsSprocName(sprocName))
                throw new ArgumentException("Sproc name contains invalid characters.");
            return sprocNameTrimmer.Replace(sprocName, string.Empty);
        }
        private static string RemoveInvalidChars(string value)
        {
            if (!string.IsNullOrEmpty(value))
                return invalidChars.Replace(value, string.Empty);
            return string.Empty;
        }
        private static string GetDefaultValueAsString(string dataType)
        {
            switch (dataType)
            {
                case "System.Char":
                case "System.String":                
                    return "String.Empty";
                case "System.Int16":
                    return "(System.Int16)0";
                case "System.Int32":
                case "System.Int64":
                case "System.Decimal":
                case "System.Double":
                case "System.Single":                
                    return "0";
                case "System.Byte":
                    return "(System.Byte)0";
                case "System.DateTime":
                    return "DateTime.MinValue";
                case "System.Guid":
                    return "Guid.Empty";
            }
            return string.Empty;
        }
        internal Database DataProvider
        {
            get;
            set;
        }
        internal CommandBehavior CommandBehavior
        {
            get
            {
                switch (this.DataProvider.DatabaseResolver.ProviderName)
                {
                    case "System.Data.OleDb":
                        return CommandBehavior.KeyInfo;
                    case "System.Data.SqlClient":
                        return CommandBehavior.SchemaOnly;
                    default:
                        return CommandBehavior.SchemaOnly;
                }
            }
        }
    }
}
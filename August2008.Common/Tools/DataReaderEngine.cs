using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Globalization;
using System.IO;
using System.Text;
using System.CodeDom;
using System.Text.RegularExpressions;
using System.Data.Common;

namespace August2008.Common.Tools
{ 
    internal sealed class DataReaderEngine
    {
        const string MethodName = "Fetch";
        const string ItemName = "item";

        static readonly Regex InvalidChars = new Regex(@"[~`@#$%^&*()\-=+{}:;,|\\\/?<>]|\s+", RegexOptions.Compiled);
        static readonly Regex SprocNameTrimmer = new Regex(@"(\[\w+\]\.|\w+\.)?", RegexOptions.Compiled);
        static readonly Hashtable InstanceTable = new Hashtable();
        static readonly string Namespace = typeof(DataReaderEngine).Namespace;

        static DataReaderEngine()
        {
            AppDomain.CurrentDomain.DomainUnload += delegate(object sender, EventArgs e)
                {
                    // unload stuff...
                };
        }
        internal DataReaderEngine(DataAccess dataProvider)
        {
            if (dataProvider == null)
            {
                throw new ArgumentNullException("dataProvider");
            }
            this.DataProvider = dataProvider;
        }
        internal void Execute(params object[] args)
        {
            string @class;
            if (!InstanceTable.ContainsKey((@class = this.ValidateArgs(args))))
            {
                lock (InstanceTable.SyncRoot)
                {
                    if (!InstanceTable.ContainsKey(@class))
                    {
                        using (var writer = new StringWriter(CultureInfo.InvariantCulture))
                        using (var reader = this.DataProvider.ExecuteReader(this.CommandBehavior))
                        {
                            var builder = new ClassBuilder(Namespace, @class, MemberAttributes.Static);

                            AddExternalAssemblyReferences(builder, args);

                            builder.AddImport("System");
                            builder.AddImport("System.Collections");
                            builder.AddImport("System.Data");
                            builder.AddImport("System.Globalization");

                            var method = builder.CreateMethod(MethodName, typeof(bool), MemberAttributes.Static);
                            builder.AddParameter(method, typeof(DbDataReader), "reader");
                            builder.AddParameter(method, typeof(object[]), "args");

                            var counter = 0;
                            writer.WriteLine("// reads data from \"{0}\" procedure.", this.DataProvider.CommandText);
                            writer.WriteLine();
                            do
                            {
                                if ((args.Length - 1) < counter) continue;

                                var schemaTable = reader.GetSchemaTable();
                                var argType = args[counter].GetType();
                                string instanceName = null;

                                if (args[counter] is IEnumerable)
                                {
                                    var genericType = argType.GetGenericArguments()[0];

                                    writer.WriteLine("IList list{0} = args[{1}] as IList;", counter, counter);
                                    writer.WriteLine("");
                                    writer.WriteLine("while (reader.Read())");
                                    writer.WriteLine("{");

                                    if (IsNativeType(genericType))
                                    {
                                        WriteNativeDataTypeReaderLogic(writer, schemaTable, genericType);
                                    }
                                    else
                                    {
                                        instanceName = ValidateInstanceName(genericType);
                                        writer.WriteLine("{0} {1} = new {2}();", instanceName, ItemName, instanceName);
                                        WriteCommonDataReaderLogic(writer, schemaTable, genericType, null, null, ItemName);
                                    }
                                    writer.WriteLine("list{0}.Add(item);", counter);
                                    writer.WriteLine("}");
                                }
                                else
                                {
                                    instanceName = ValidateInstanceName(argType);

                                    writer.WriteLine("if (reader.Read())");
                                    writer.WriteLine("{");
                                    writer.WriteLine("{0} {1} = ({2})args[{3}];", instanceName, ItemName, instanceName, counter);

                                    WriteCommonDataReaderLogic(writer, schemaTable, argType, null, null, ItemName);

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
                            while (reader.NextResult());

                            writer.WriteLine("return true;");

                            builder.AddMethodBody(method, writer.ToString());

#if DEBUG
                            using (var writer2 = new StringWriter(CultureInfo.InvariantCulture))
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
                            InstanceTable.Add(@class, instance);
                        }
                    }
                }
            }
            this.Execute(@class, args);
        }
        private void Execute(string className, IEnumerable args)
        {
            object instance;
            if ((instance = InstanceTable[className]) == null) return;

            var type = instance.GetType();
            var method = type.GetMethod(MethodName, BindingFlags.NonPublic | BindingFlags.Static);
            using (var reader = this.DataProvider.ExecuteReader())
            {
                method.Invoke(instance, new object[] { reader, args });
            }
        }
        private static void WriteCommonDataReaderLogic(TextWriter writer, DataTable schemaTable, Type concreteType, Type declaringType, Type propertyType, string itemName)
        {
            ValidateSchemaTable(schemaTable, -1);

            foreach (DataRow row in schemaTable.Rows)
            {
                var columnName = row["ColumnName"].ToString();
                var clrDataTypeName = row["DataType"].ToString();
                var columnOrdinal = row["ColumnOrdinal"].ToString();
                var allowDbNull = Convert.ToBoolean(row["AllowDBNull"]);

                PropertyInfo property = null;

                if ((property = GetProperty(columnName, concreteType, false)) == null)
                {
                    continue;
                }
                if (!allowDbNull)
                {
                    writer.WriteLine("{0}.{1} = ({2})reader[{3}];", itemName, property.Name, clrDataTypeName, columnOrdinal);
                }
                else
                {
                    writer.WriteLine("{0}.{1} = reader.IsDBNull({2}) ? {3} : ({4})reader[{5}];", itemName, property.Name,
                                     columnOrdinal, GetDefaultValueAsString(clrDataTypeName), clrDataTypeName, columnOrdinal);
                }
            }
        }
        private static void WriteNativeDataTypeReaderLogic(StringWriter writer, DataTable schemaTable, Type type)
        {
            ValidateSchemaTable(schemaTable, 1);

            var row = schemaTable.Rows[0];
            var columnName = row["ColumnName"].ToString();
            var clrDataTypeName = row["DataType"].ToString();
            var columnOrdinal = row["ColumnOrdinal"].ToString();
            var allowDbNull = Convert.ToBoolean(row["AllowDBNull"]);

            if (!allowDbNull)
            {
                writer.WriteLine("{0} item = ({1})reader[{2}];", clrDataTypeName, clrDataTypeName, columnOrdinal);
            }
            else
            {
                writer.WriteLine("{0} item = reader.IsDBNull({1}) ? {2} : ({3})reader[{4}];", clrDataTypeName,
                                 columnOrdinal, GetDefaultValueAsString(clrDataTypeName), clrDataTypeName, columnOrdinal);
            }
        }
        private static PropertyInfo GetProperty(string name, Type declaringType, bool throwOnError)
        {
            var property = declaringType.GetProperty(name, BindingFlags.Public | BindingFlags.Instance);
            if (property == null && throwOnError)
            {
                throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, 
                    "Property name {0} does not exist in {1} type but it is defined in configuration settings.", name, declaringType.FullName));
            }
            return property;
        }
        private string ComposeClassName(IEnumerable<object> args)
        {
            var sb = new StringBuilder();
            sb.Append(SprocNameToClassName(this.DataProvider.CommandText));
            foreach (var t in args)
            {
                sb.Append("_");
                var type = t.GetType();
                if (type.IsGenericType)
                {
                    var genericType = type.GetGenericArguments()[0];
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
            {
                throw new ArgumentException("At least one valid argument is required to create the wrapper.");
            }
            for (var i = 0; i < args.Length; i++)
            {
                if (args[i] == null)
                {
                    throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Null argument passed at index {0}.", i));
                }
                var type = args[i].GetType();
                if (args[i] is IEnumerable && !IsNativeType(type))
                {
                    if (type.IsGenericType)
                    {
                        var genericArgs = type.GetGenericArguments();
                        if (genericArgs.Length > 1)
                        {
                            throw new ArgumentException(string.Format(CultureInfo.InvariantCulture,
                                                                      "Collection type {0} at index {1} contains more than one generic argument.",
                                                                      type.FullName, i));
                        }
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
        private static string ValidateInstanceName(Type type)
        {
            return !type.IsNested ? type.FullName : type.FullName.Replace('+', '.');
        }

        private static void ValidateSchemaTable(DataTable schemaTable, int minRows)
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
        private static void AddExternalAssemblyReferences(ClassBuilder builder, object[] args)
        {
            builder.AddReference(Assembly.GetExecutingAssembly());
            foreach (var item in args)
            {
                var type = item.GetType();
                if (IsNativeType(type)) continue;
                if (item is IEnumerable)
                {
                    type = type.GetGenericArguments()[0];
                }
                builder.AddReference(Assembly.GetAssembly(type));
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
            return (!string.IsNullOrEmpty(sproc) && !InvalidChars.IsMatch(sproc));
        }
        private static string SprocNameToClassName(string sprocName)
        {
            if (!IsSprocName(sprocName))
            {
                throw new ArgumentException("Sproc name contains invalid characters.");
            }
            return SprocNameTrimmer.Replace(sprocName, string.Empty);
        }
        private static string RemoveInvalidChars(string value)
        {
            return !string.IsNullOrEmpty(value) ? InvalidChars.Replace(value, string.Empty) : string.Empty;
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
                case "System.Boolean":
                    return "false";
                case "System.Byte":
                    return "(System.Byte)0";
                case "System.DateTime":
                    return "DateTime.MinValue";
                case "System.Guid":
                    return "Guid.Empty";
                default:
                    return "default(" + dataType + ")";
            }
            return string.Empty;
        }
        internal DataAccess DataProvider
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
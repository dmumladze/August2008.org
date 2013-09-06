using System;
using System.Data;
using System.Data.Common;
using August2008.Common.Exceptions;
using August2008.Common.Interfaces;
using August2008.Common.Tools;

namespace August2008.Common
{
    public sealed class DataAccess : IDisposable
    {
        public const string ReturnValueParameterName = "@info";

        #region [ class member declarations ]
        
        private DbCommand _mainCommand;
        private DbConnection _mainConnection; 
        private DbTransactionManager _transactionManager;
        private bool _mainConnectionIsLocal;
        private bool _isDisposed;

        #endregion

        #region [ initializers ]

        /// <summary>
        /// Initializes a new instance of data provider with a default IDatabaseResolver instance.
        /// </summary>
        public DataAccess() : this(DbConnectionResolver.Default)
        {
        }
        /// <summary>
        /// Initializes a new instance of data provider.
        /// </summary> 
        public DataAccess(IDbConnectionResolver resolver)
        {
            this.Initialize(resolver);
        }
        /// <summary>
        /// Initializes a new instance of data provider with a ITransactionManager. Data provider then uses ITransactionManager's properties to process the transaction.
        /// </summary> 
        public DataAccess(DbTransactionManager transaction)
        {
            try
            {
                _transactionManager = transaction;
                DatabaseResolver = transaction.DatabaseResolver;
                DbProviderFactory = DbProviderFactories.GetFactory(DatabaseResolver.ProviderName);
                _mainConnection = transaction.CurrentTransaction.Connection;
                _mainCommand = DbProviderFactory.CreateCommand();
                _mainConnectionIsLocal = false;
                _isDisposed = false;
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Failed to initialize data provider with a DbTransactionManager", ex);
            }
        }

        private void Initialize(IDbConnectionResolver resolver)
        {
            try
            {
                DatabaseResolver = resolver;
                DbProviderFactory = DbProviderFactories.GetFactory(resolver.ProviderName);
                _mainConnection = DbProviderFactory.CreateConnection();
                _mainConnection.ConnectionString = resolver.ConnectionString;
                _mainCommand = DbProviderFactory.CreateCommand();
                _mainConnectionIsLocal = true;
                _isDisposed = false;
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Failed to initialize data provider", ex);
            }
        }

        #endregion

        #region [ method declarations ]

        /// <summary>
        /// Creates an internal DbCommand object for the stored procedure.
        /// </summary>
        public void CreateStoredProcCommand(string commandText)
        {
            if (IsCommandPending)
            {
                throw new DataAccessException("Cannot create a new command while the other is still pending.");
            }
            if (string.IsNullOrEmpty(commandText))
            {
                return;
            }
            _mainCommand.Parameters.Clear();
            _mainCommand.CommandText = commandText;
            _mainCommand.CommandType = CommandType.StoredProcedure;
            IsCommandPending = true;
        }
        /// <summary>
        /// Creates an internal DbCommand object for the command.
        /// </summary>
        public void CreateTextCommand(string commandText)
        {
            if (IsCommandPending)
            {
                throw new DataAccessException("Cannot create a new command while the other is still pending.");
            }
            if (string.IsNullOrEmpty(commandText))
            {
                return;
            }
            _mainCommand.CommandText = commandText;
            _mainCommand.CommandType = CommandType.Text;
            IsCommandPending = true;
        }
        /// <summary>
        /// Adds an input parameter to the associated DbCommand object.
        /// </summary>
        public void AddInParameter(string name, DbType type, object value)
        {
            AddParameter(name, type, ParameterDirection.Input, object.ReferenceEquals(value, null) ? DBNull.Value : value);
        }
        /// <summary>
        /// Adds an output parameter to the associated DbCommand object.
        /// </summary>
        public void AddOutParameter(string name, DbType type)
        {
            AddParameter(name, type, ParameterDirection.Output, DBNull.Value);
        }
        /// <summary>
        /// Adds a default return parameter to the associated DbCommand object.
        /// </summary>
        public void AddReturnParameter()
        {
            AddReturnParameter(ReturnValueParameterName, DbType.Int32);
        }
        /// <summary>
        /// Adds a return parameter to the associated DbCommand object.
        /// </summary>
        public void AddReturnParameter(string name, DbType type)
        {
            AddParameter(name, type, ParameterDirection.ReturnValue, null);
        }
        /// <summary>
        /// Adds a parameter to the associated DbCommand object.
        /// </summary>
        public void AddParameter(string name, DbType type, ParameterDirection direction, object value)
        {
            var param = DbProviderFactory.CreateParameter();
            param.DbType = type;
            param.ParameterName = name;
            param.Direction = direction;
            param.Value = value;
            _mainCommand.Parameters.Add(param);
        }
        /// <summary>
        /// Removes the parameter from the associated DbCommand object.
        /// </summary>
        public void RemoveParameter(string name)
        {
            int index = _mainCommand.Parameters.IndexOf(name);
            if (index > 0)
            {
                _mainCommand.Parameters.RemoveAt(index);
            }
            else
            {
                throw new DataAccessException(string.Format("Parameter {0} does not exists", name));
            }
        }
        /// <summary>
        /// Auto generates the DbDataReader wrapper class which is used to initialize the entities.
        /// </summary>
        public void ReadInto(params object[] args)
        {
            var engine = new DataReaderEngine(this);
            try
            {
                engine.Execute(args);
            }
            catch (ArgumentException ex)
            {
                throw new DataAccessException("Cannot read data. Check the parameters and try again", ex);
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Cannot read data", ex);
            }
        }
        /// <summary>
        /// Executes commands such as Insert, Update, and Delete
        /// </summary>
        public int ExecuteNonQuery()
        {
            this.OpenConnection();
            IsCommandPending = false;
            RowsAffected = _mainCommand.ExecuteNonQuery();
            return RowsAffected;
        }
        /// <summary>
        /// Executes the DbCommand.
        /// </summary>
        public DbDataReader ExecuteReader()
        {
            this.OpenConnection();
            IsCommandPending = false;
            return _mainCommand.ExecuteReader();
        }
        /// <summary>
        /// Executes the DbCommand.
        /// </summary>
        public DbDataReader ExecuteReader(CommandBehavior behavior)
        {
            this.OpenConnection();
            IsCommandPending = false;
            return _mainCommand.ExecuteReader(behavior);
        }
        /// <summary>
        /// Executes scalar-valued procedures.
        /// </summary>
        public T ExecuteScalar<T>()
        {
            this.OpenConnection();
            IsCommandPending = false;
            var value = _mainCommand.ExecuteScalar();
            if (!(value is System.DBNull || value == null))
            {
                return (T)value;
            }
            return default(T);
        }
        /// <summary>
        /// Extracts the parameter object from DbCommand object.
        /// </summary>
        public T GetParameterValue<T>(string name)
        {
            var param = _mainCommand.Parameters[name];
            if (param != null && param.Value != DBNull.Value)
            {
                return (T)param.Value;
            }
            return default(T);
        }
        /// <summary>
        /// Extracts the return value parameter value from the DbCommand object.
        /// </summary>
        public int GetReturnValue()
        {
            return this.GetParameterValue<int>(ReturnValueParameterName);
        }
        /// <summary>
        /// Resets and removes all the parameters from the main DbCommand object; However, command is still a part of the transaction.
        /// </summary>
        public void ResetCommand(bool clearCommandText = true)
        {
            if (IsCommandPending)
            {
                throw new DataAccessException("Cannot reset the current command without executing it first.");
            }
            IsCommandPending = false;
            RowsAffected = 0;
            _mainCommand.Parameters.Clear();
            if (clearCommandText)
            {
                _mainCommand.CommandText = string.Empty;
            }
        }
        /// <summary>
        /// Manages connection object state.
        /// </summary>
        private void OpenConnection()
        {
            if (_mainConnectionIsLocal)
            {
                if (_mainConnection.State != ConnectionState.Open)
                {
                    _mainCommand.Connection = _mainConnection;
                    _mainConnection.Open();
                }
            }
            else
            {
                if (_transactionManager.IsTransactionPending)
                {
                    _mainCommand.Connection = _transactionManager.CurrentTransaction.Connection;
                    _mainCommand.Transaction = _transactionManager.CurrentTransaction;
                }
            }
        }
        /// <summary>
        /// Attaches a DbTransactionManager to the current command object.
        /// </summary>
        public void AttachTransaction(DbTransactionManager transaction)
        {
            if (transaction == null)            
                throw new NullReferenceException("transaction");

            if (!object.ReferenceEquals(transaction, _transactionManager))
            {
                if (_mainConnection != null)
                {
                    if (_mainConnectionIsLocal)
                    {
                        _mainConnection.Close();
                        _mainConnection.Dispose();
                    }
                    _mainConnection = null;
                }
                _transactionManager = transaction;
                _mainConnection = transaction.CurrentTransaction.Connection;
                _mainConnectionIsLocal = false;
            }
        }
        /// <summary>
        /// Detaches the command object from the transaction.
        /// </summary>
        public void DetachTransaction()
        {
            if (_mainCommand.Transaction == null)
                throw new DataAccessException("There is no transaction associate with the current command.");
            _mainCommand.Transaction = null;
        }
        /// <summary>
        /// Releases resources held by the data provider object.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// Releases resources held by the data provider object.
        /// </summary>
        private void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    if (_mainConnectionIsLocal)
                    {
                        if ((_mainConnection.State == ConnectionState.Open))
                        {
                            _mainConnection.Close();
                        }
                        _mainConnection.Dispose();
                        _mainCommand.Dispose();
                        _mainConnectionIsLocal = false;
                        _mainCommand = null;
                        _mainConnection = null;
                        DbProviderFactory = null;
                    }
                }
                _isDisposed = true;
            }
        }

        #endregion

        #region [ class property declarations ]

        /// <summary>
        /// Gets number of rows affected by the current operation.
        /// </summary>
        public int RowsAffected { get; private set; }

        /// <summary>
        /// Gets or the Transact-SQL statement, table name or stored procedure to execute at the data source.
        /// </summary>
        public string CommandText
        {
            get
            {
                return _mainCommand.CommandText;
            }
        }
        public CommandType CommandType
        {
            get
            {
                return _mainCommand.CommandType;
            }
        }
        public bool IsCommandPending { get; private set; }
        public IDbConnectionResolver DatabaseResolver { get; private set; }
        public DbProviderFactory DbProviderFactory { get; private set; }

        #endregion
    }
}

using System;
using System.Data;
using System.Data.Common;
using System.Configuration;
using MelaniArt.Core.Tools;
using MelaniArt.Core.Exceptions;
using MelaniArt.Core.Interfaces;

namespace MelaniArt.Core.DataAccess
{
    public class Database : IDisposable
    {
        public const string ReturnValueParameterName = "@info";

        #region [ class member declarations ]
        
        private DbCommand _mainCommand;
        private DbConnection _mainConnection; 
        private DbTransactionManager _transactionManager;
        private bool _mainConnectionIsLocal;
        private int _rowsAffected;
        private bool _isDisposed;
        private bool _isCommandPending;
        private IDbConnectionResolver _databaseResolver;
        private DbProviderFactory _providerFactory;

        #endregion

        #region [ initializers ]

        /// <summary>
        /// Initializes a new instance of data provider with a default IDatabaseResolver instance.
        /// </summary>
        public Database() : this(DefaultDatabaseResolver.Instance)
        {
        }
        /// <summary>
        /// Initializes a new instance of data provider.
        /// </summary> 
        public Database(IDbConnectionResolver resolver)
        {
            this.Initialize(resolver);
        }
        /// <summary>
        /// Initializes a new instance of data provider with a ITransactionManager. Data provider then uses ITransactionManager's properties to process the transaction.
        /// </summary> 
        public Database(DbTransactionManager transaction)
        {
            try
            {
                _transactionManager = transaction;
                _databaseResolver = transaction.DatabaseResolver;
                _providerFactory = DbProviderFactories.GetFactory(_databaseResolver.ProviderName);
                _mainConnection = transaction.CurrentTransaction.Connection;
                _mainCommand = _providerFactory.CreateCommand();
                _mainConnectionIsLocal = false;
                _isDisposed = false;
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Failed to initialize data provider with a DbTransactionManager", ex);
            }
        }
        protected virtual void Initialize(IDbConnectionResolver resolver)
        {
            try
            {
                _databaseResolver = resolver;
                _providerFactory = DbProviderFactories.GetFactory(resolver.ProviderName);
                _mainConnection = _providerFactory.CreateConnection();
                _mainConnection.ConnectionString = resolver.ConnectionString;
                _mainCommand = _providerFactory.CreateCommand();
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
            if (_isCommandPending)
                throw new DataAccessException("Cannot create a new command while the other is still pending.");
            if (!string.IsNullOrEmpty(commandText))
            {                
                _mainCommand.CommandText = commandText;
                _mainCommand.CommandType = CommandType.StoredProcedure;
                _isCommandPending = true;
            }
        }
        /// <summary>
        /// Creates an internal DbCommand object for the command.
        /// </summary>
        public void CreateTextCommand(string commandText)
        {
            if (_isCommandPending)
                throw new DataAccessException("Cannot create a new command while the other is still pending.");
            if (!string.IsNullOrEmpty(commandText))
            {
                _mainCommand.CommandText = commandText;
                _mainCommand.CommandType = CommandType.Text;
                _isCommandPending = true;
            }
        }
        /// <summary>
        /// Adds an input parameter to the associated DbCommand object.
        /// </summary>
        public void AddInParameter(string name, DbType paramType, object value)
        {
            AddParameter(name, paramType, ParameterDirection.Input, object.ReferenceEquals(value, null) ? DBNull.Value : value);
        }
        /// <summary>
        /// Adds an output parameter to the associated DbCommand object.
        /// </summary>
        public void AddOutParameter(string name, DbType paramType)
        {
            AddParameter(name, paramType, ParameterDirection.Output, DBNull.Value);
        }
        /// <summary>
        /// Adds a return parameter to the associated DbCommand object.
        /// </summary>
        public void AddReturnParameter(string name, DbType paramType)
        {
            AddParameter(name, paramType, ParameterDirection.ReturnValue, null);
        }
        /// <summary>
        /// Adds a parameter to the associated DbCommand object.
        /// </summary>
        public void AddParameter(string name, DbType paramType, ParameterDirection direction, object value)
        {
            var param = _providerFactory.CreateParameter();
            param.DbType = paramType;
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
                _mainCommand.Parameters.RemoveAt(index);
            else
                throw new DataAccessException(string.Format("Parameter {0} does not exists", name));
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
        public void ExecuteNonQuery()
        {
            this.OpenConnection();
            _isCommandPending = false;
            _rowsAffected = _mainCommand.ExecuteNonQuery();
        }
        /// <summary>
        /// Executes the DbCommand.
        /// </summary>
        public DbDataReader ExecuteReader()
        {
            this.OpenConnection();
            _isCommandPending = false;
            return _mainCommand.ExecuteReader();
        }
        /// <summary>
        /// Executes the DbCommand.
        /// </summary>
        public DbDataReader ExecuteReader(CommandBehavior behavior)
        {
            this.OpenConnection();
            _isCommandPending = false;
            return _mainCommand.ExecuteReader(behavior);
        }
        /// <summary>
        /// Executes scalar-valued procedures.
        /// </summary>
        public T ExecuteScalar<T>()
        {
            this.OpenConnection();
            _isCommandPending = false;
            object value = _mainCommand.ExecuteScalar();
            if (value != null)
                return (T)value;
            return default(T);
        }
        /// <summary>
        /// Extracts the parameter object from DbCommand object.
        /// </summary>
        public T GetParameterValue<T>(string name)
        {
            var param = _mainCommand.Parameters[name];
            if (param != null &&
                param.Value != DBNull.Value)
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
        public void ResetPendingCommand()
        {
            if (_isCommandPending)
                throw new DataAccessException("Cannot reset the current command without executing it first.");
            _isCommandPending = false;
            _rowsAffected = 0;
            _mainCommand.Parameters.Clear();
            _mainCommand.CommandText = string.Empty;
        }
        /// <summary>
        /// Manages connection object state.
        /// </summary>
        protected void OpenConnection()
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
        protected virtual void Dispose(bool disposing)
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
                        _providerFactory = null;
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
        public int RowsAffected
        {
            get
            {
                return _rowsAffected;
            }
        }
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
        public bool IsCommandPending
        {
            get
            {
                return _isCommandPending;
            }
        }
        public IDbConnectionResolver DatabaseResolver
        {
            get
            {
                return _databaseResolver;
            }
        }
        public DbProviderFactory DbProviderFactory
        {
            get
            {
                return _providerFactory;
            }
        }

        #endregion
    }
}

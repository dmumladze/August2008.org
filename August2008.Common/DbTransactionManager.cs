using System;
using System.Data;
using System.Data.Common;
using August2008.Common.Interfaces;

namespace August2008.Common
{
    /// <summary>
    /// Provides transaction to IDataProvider objects. This class cannot be inherited.
    /// </summary>
    public sealed class DbTransactionManager : IDisposable
    {
        #region [ class member declarations ]

        private DbConnection _connection;
        private DbTransaction _currentTransaction;
        private bool _isTransactionPending;
        private bool _isDisposed;
        private IsolationLevel _isolationLevel;
        private DbProviderFactory _providerFactory;
        private IDbConnectionResolver _resolver;

        #endregion

        /// <summary>
        /// Initializes a new instance of DbTransactionManager with a default IDatabaseResolver instance.
        /// </summary>
        public DbTransactionManager() : this(DbConnectionResolver.Instance)
        {
        }
        /// <summary>
        /// Initializes a new instance of DbTransactionManager with an external IDatabaseResolver instance.
        /// </summary>
        public DbTransactionManager(IDbConnectionResolver resolver)
        {
            this.Initialize(resolver);
        }
        private void Initialize(IDbConnectionResolver resolver)
        {
            try
            {
                _resolver = resolver;
                _providerFactory = DbProviderFactories.GetFactory(resolver.ProviderName);
                _connection = _providerFactory.CreateConnection();
                _connection.ConnectionString = resolver.ConnectionString;
            }
            catch 
            {
                throw new DbTransactionException("Failed to initialize DbTransactionManager.");
            }
        }
        /// <summary>
        /// Starts the transaction.
        /// </summary>
        public void BeginTransaction()
        {
            if (_isTransactionPending)
            {
                throw new DbTransactionException("Transaction already pending. Nesting not allowed.");
            }
            if (_connection == null)
            {
                throw new DbTransactionException("DbConnection is required to start the transaction.");
            }
            try
            {
                _connection.Open();
                _currentTransaction = _connection.BeginTransaction(IsolationLevel.ReadCommitted);
                _isTransactionPending = true;
            }
            catch (Exception ex)
            {
                throw new DbTransactionException("DbTransactionManager failed to begin the transaction.", ex);
            }
        }
        /// <summary>
        /// Commits the current transaction.
        /// </summary>
        public void Commit()
        {
            if (!_isTransactionPending)
            {
                throw new DbTransactionException("No transaction is pending at the moment.");
            }
            try
            {                
                _currentTransaction.Commit();
                _isTransactionPending = false;
                _currentTransaction.Dispose();
                _currentTransaction = null;
            }
            catch (Exception ex)
            {
                throw new DbTransactionException("DbTransactionManager failed to commit the transaction.", ex);
            }
        }
        /// <summary>
        /// Rolls back the transaction, if any.
        /// </summary>
        public void Rollback()
        {
            if (_isTransactionPending)
            {
                try
                {                    
                    _currentTransaction.Rollback();
                    _isTransactionPending = false;
                }
                catch (Exception ex)
                {
                    throw new DbTransactionException("DbTransactionManager failed to rollback the transaction.", ex);
                }
            }
        }
        /// <summary>
        /// Releases resources held by the DbTransactionManager object.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// Releases resources held by the DbTransactionManager object.
        /// </summary>
        private void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    if (_currentTransaction != null)
                    {
                        _currentTransaction.Dispose();
                        _currentTransaction = null;
                    }
                    if (_connection != null)
                    {
                        if (_connection.State == ConnectionState.Open)
                        {
                            _connection.Close();
                        }
                        _connection.Dispose();
                        _connection = null;
                        _providerFactory = null;
                    }
                }
            }
            _isDisposed = true;
        }

        #region [ class property declarations ]

        /// <summary>
        /// Gets the value to see if a transaction is pending.
        /// </summary>
        public bool IsTransactionPending
        {
            get
            {
                return _isTransactionPending;
            }
        }
        /// <summary>
        /// Gets the DbTransaction object assiciated with this transaction.
        /// </summary>
        public DbTransaction CurrentTransaction
        {
            get
            {
                return _currentTransaction;
            }
        }
        /// <summary>
        /// Gets the isolation level (locking behavior) for the current transaction.
        /// </summary>
        internal IDbConnectionResolver DatabaseResolver
        {
            get
            {
                return _resolver;
            }
        }

        #endregion
    }
}

using System;

namespace August2008.Common
{
    public class DbTransactionException : Exception
    { 
        public DbTransactionException()
        {
        }
        public DbTransactionException(string message)
            : base(message)
        {
        } 
        public DbTransactionException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}

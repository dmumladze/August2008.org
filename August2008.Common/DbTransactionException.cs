using System;

namespace MelaniArt.Core.Exceptions
{
    public class DbTransactionException : MelaniArtException
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

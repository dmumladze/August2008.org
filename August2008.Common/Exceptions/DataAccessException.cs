using System;

namespace August2008.Common
{
    public class DataAccessException : Exception
    {
        public DataAccessException()
        {
        }
        public DataAccessException(string message)
            : base(message)
        {
        }
        public DataAccessException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}

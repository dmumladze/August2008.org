using System;

namespace MelaniArt.Core.Exceptions
{
    public class DataAccessException : MelaniArtException
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

using System;

namespace August2008.Common
{ 
    public class ManagerException : Exception
    {
        public ManagerException(string message)
            : base(message)
        {
        }
        public ManagerException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}

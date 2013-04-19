using System;

namespace MelaniArt.Core.Exceptions
{ 
    public class ManagerException : MelaniArtException
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

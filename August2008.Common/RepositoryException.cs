using System;
using System.Data.SqlClient;

namespace August2008.Common
{
    public class RepositoryException : Exception 
    {
        public RepositoryException(string message)
            : base(message)
        {
        }
        public RepositoryException(string message, Exception exception)
            : base(message, exception)
        {           
        }
        public override string Message
        {
            get
            {
                var error = base.InnerException as SqlException;
                if (error != null)
                {
                    if (error.Number > 50000)
                    {
                        return error.Message;
                    }
                    else
                    {
                        return "Oops! Something went wrong... :(";
                    }
                }
                return base.Message;
            }
        }
    }
}

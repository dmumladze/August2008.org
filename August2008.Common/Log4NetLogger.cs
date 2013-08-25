using System;
using August2008.Common.Interfaces;
using log4net;

namespace August2008.Common
{
    public class Log4NetLogger : ILogger
    {
        private ILog logger = LogManager.GetLogger(typeof(Log4NetLogger));

        public void Debug(object message)
        {
            logger.Debug(message);
        }
        public void Debug(object message, Exception exception)
        {
            logger.Debug(message, exception);
        }
        public void DebugFormat(string format, params object[] args)
        {
            logger.DebugFormat(format, args);
        }
        public void Error(object message)
        {
            logger.Error(message);
        }
        public void Error(object message, Exception exception)
        {
            logger.Error(message, exception);
        }
        public void ErrorFormat(string format, params object[] args)
        {
            logger.ErrorFormat(format, args);
        }
        public void Fatal(object message)
        {
            logger.Fatal(message);
        }
        public void Fatal(object message, Exception exception)
        {
            logger.Fatal(message, exception);
        }
        public void FatalFormat(string format, params object[] args)
        {
            logger.FatalFormat(format, args);
        }
        public void Info(object message)
        {
            logger.Info(message);
        }
        public void Info(object message, Exception exception)
        {
            logger.Info(message, exception);
        }
        public void InfoFormat(string format, params object[] args)
        {
            logger.InfoFormat(format, args);
        }
        public void Warn(object message)
        {
            logger.Warn(message);
        }
        public void Warn(object message, Exception exception)
        {
            logger.Warn(message, exception);
        }
        public void WarnFormat(string format, params object[] args)
        {
            logger.WarnFormat(format, args);
        }
    }
}

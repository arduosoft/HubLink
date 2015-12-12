using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Wlog.Web.Code.Helpers;

namespace Wlog.Web.Code.Classes
{
    public class LogQueue
    {
        private Queue<LogMessage> queque = new Queue<LogMessage>();

        public long Count
        {
            get { return queque.Count; }
        }

        public void Enqueue(LogMessage le)
        {
            queque.Enqueue(le);
        }

        public List<LogMessage> Dequeue(int count)
        {
            List<LogMessage> result = new List<LogMessage>();
            LogMessage newElem;
            int i = 0;
            while (i < count)
            {
                newElem = queque.Dequeue();
                if (newElem != null)
                {
                    result.Add(newElem);
                }
                else
                {
                    break;
                }
                i++;
            }
            return result;
        }

        public void Run()
        {



            if (LogQueue.Current.Count > 0)
            {
                using (UnitOfWork uow = new UnitOfWork())
                {
                    for (int i = 0; i < Math.Min(LogQueue.Current.Count, 10000); i++)
                    {

                        LogMessage log = LogQueue.Current.Dequeue();
                        
                        LogEntity entToSave = ConversionHelper.ConvertLog(uow,log);
                        LogHelper.AppendLog(uow, entToSave);


                    }
                    uow.Commit();
                }
            }
        }

        public LogMessage Dequeue()
        {
            return queque.Dequeue();
        }

        private static LogQueue _current = null;
        public static LogQueue Current
        {
            get
            {
                if (_current == null) _current = new LogQueue();
                return _current;
            }
        }
    }
}
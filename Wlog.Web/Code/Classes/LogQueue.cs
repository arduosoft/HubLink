using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wlog.Web.Code.Classes
{
    public class LogQueue
    {
        private Queue<LogEntry> queque = new Queue<LogEntry>();

        public long Count
        {
            get { return queque.Count; }
        }

        public void Enqueue(LogEntry le)
        {
            queque.Enqueue(le);
        }

        public List<LogEntry> Dequeue(int count)
        {
            List<LogEntry> result = new List<LogEntry>();
            LogEntry newElem;
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
                    LogEntry log = LogQueue.Current.Dequeue();



                    using (ISession session = WebApiApplication.CurrentSessionFactory.OpenSession())
                    {
                        using (ITransaction transaction = session.BeginTransaction())
                        {
                            session.Save(log);
                            transaction.Commit();
                        }
                    }

                }
            
        }

        public LogEntry Dequeue()
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
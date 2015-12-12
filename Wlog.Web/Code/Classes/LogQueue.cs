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
        private Queue<LogEntity> queque = new Queue<LogEntity>();

        public long Count
        {
            get { return queque.Count; }
        }

        public void Enqueue(LogEntity le)
        {
            queque.Enqueue(le);
        }

        public List<LogEntity> Dequeue(int count)
        {
            List<LogEntity> result = new List<LogEntity>();
            LogEntity newElem;
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

                        LogEntity log = LogQueue.Current.Dequeue();
                        log.Uid = Guid.NewGuid();
                        LogHelper.AppendLog(uow, log);


                    }
                    uow.Commit();
                }
            }
        }

        public LogEntity Dequeue()
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
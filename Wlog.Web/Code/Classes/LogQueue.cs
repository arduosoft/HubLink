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
        public List<QueueLoad> QueueLoad { get; set; }

        public int MaxProcessedItems { get; set; }
        public int MaxQueueSize { get; set; }

        public long Count
        {
            get { return queque.Count; }
        }

        public void Enqueue(LogMessage le)
        {
            queque.Enqueue(le);
        }

        public LogQueue()
        {
            MaxProcessedItems = 10000;
            MaxQueueSize = 100000;
            QueueLoad = new List<Classes.QueueLoad>();
            AppendLoadValue(0, MaxQueueSize);
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

            LogQueue.Current.AppendLoadValue(LogQueue.Current.Count, LogQueue.Current.MaxQueueSize);

            if (LogQueue.Current.Count > 0)
            {
                using (UnitOfWork uow = new UnitOfWork())
                {
                    uow.BeginTransaction();

                    for (int i = 0; i < Math.Min(LogQueue.Current.Count, LogQueue.Current.MaxProcessedItems); i++)
                    {

                        LogMessage log = LogQueue.Current.Dequeue();
                        
                        LogEntity entToSave = ConversionHelper.ConvertLog(uow,log);
                        LogHelper.AppendLog(uow, entToSave);

                       
                    }
                    uow.Commit();
                }
                LogQueue.Current.AppendLoadValue(LogQueue.Current.Count, LogQueue.Current.MaxQueueSize);
            }
        }

        private void AppendLoadValue(long count, int maxQueueSize)
        {
            if (this.QueueLoad.Count > 100)
            {
                this.QueueLoad.RemoveAt(0);
            }

            this.QueueLoad.Add(new Classes.QueueLoad() {
                MaxSize = maxQueueSize,
                QueueSize = (int)count,
                Time = DateTime.Now
            });
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
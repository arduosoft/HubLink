using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Wlog.TestApp.Test
{
    public class TestIterator
    {
        Logger l = LogManager.GetCurrentClassLogger();
        public int RepeatCount { get; set; }

        public List<TestInstance> Instances { get; set; }

        public TestIterator()
        {
            RepeatCount = 10;
            Instances = new List<TestInstance>();
        }

        public void DoTest()
        { 
            Stopwatch sw= new Stopwatch();
            l.Info("TEST START");
            foreach(TestInstance t in Instances)
            {
                l.Info("TEST START"+t.ToString());
                sw.Reset();
                sw.Start();
                for(int i=0; i< RepeatCount;i++)
                {

                    
                    t.Execute();
                   
                }
                sw.Stop();
                 double ms =   1000.0 * (double)sw.ElapsedTicks / Stopwatch.Frequency;
                 double med=ms/RepeatCount;
                 l.Info("TEST TIME" + ms+" avg:"+med);
                 Thread.Sleep(10000);
            }
        }
    }
}

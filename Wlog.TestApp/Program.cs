using Newtonsoft.Json;
using NLog;
using NLog.Config;
using NLog.WebLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Wlog.TestApp.Test;

namespace Wlog.TestApp
{
    class Program
    {
        static Logger logger = LogManager.GetCurrentClassLogger();
        static void Main(string[] args)
        {


            Console.WriteLine("Making a single call to service to check availability...");
            //Manual call to log service to test plain performance
            DateTime d1 = DateTime.Now;
            WebTarget.DoRequest("Http://localhost:55044/api/log", JsonConvert.SerializeObject(new NLog.WebLog.WebTarget.LogMessage() 
            { 
                Message="TEST MANUAL MESSAGE" + DateTime.Now,
                SourceDate=DateTime.Now,
                Level="Error",
                ApplicationKey = "{FF99EE02-1E88-44FF-B0A4-8DF8D2F3B742}"
                
            }));

            double ms = DateTime.Now.Subtract(d1).TotalMilliseconds;
            Console.WriteLine("Service call done in ms:"+ms);

            TestIterator it = new TestIterator();
            it.Instances.Add(new WlogTest());
            it.Instances.Add(new WLogBulkTest());
            it.Instances.Add(new FileTest());

            int[] iterationSize = new int[] { 1, 10, 100, 1000, 10000 };
            Console.WriteLine("#;Wlog;Wlog (bulk);File;");
            for (int i = 0; i < iterationSize.Length; i++)
            {                
                it.RepeatCount = iterationSize[i];
                //Console.WriteLine("=> Starting benchmark with iterationSize " + it.RepeatCount);

                it.DoTest();

                Console.WriteLine("{0};{1};{2};{3}", it.RepeatCount, it.Instances[0].Avg,  it.Instances[1].Avg, it.Instances[2].Avg);
            }


            Console.ReadKey();
        }
    }
}

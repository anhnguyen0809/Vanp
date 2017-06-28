using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Web;

namespace Vanp.DAL.Utils
{
    public class BackgroundThread
    {
        public static void StartCheckingLog()
        {
            var thread = new Thread(new ThreadStart(StartJob));
            thread.IsBackground = true;
            thread.Name = "BackgroundChecker";
            thread.Start();
        }

        private static void StartJob()
        {
            var logChecker = new LogChecker();
            var timer = new System.Timers.Timer();
            timer.Interval = 60000;
            timer.Elapsed += new System.Timers.ElapsedEventHandler(logChecker.SearchLogForIntrusion);
            timer.Enabled = true;
            // If AutoReset=false then the timer will only tick once
            timer.AutoReset = true;
            timer.Start();
        }

        private class LogChecker
        {
            internal void SearchLogForIntrusion(object sender, System.Timers.ElapsedEventArgs e)
            {
                //Cập nhập thường xuyên tình trạng kết thúc của sản phẩm
                UnitOfWork unitOfWork = new UnitOfWork();
                unitOfWork.ProductRepository.ProductBiEndByTime();

                Trace.TraceError("Found intrusion in the log...");
            }
        }
    }
}
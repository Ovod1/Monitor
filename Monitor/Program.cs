using System;
using System.Diagnostics;
using System.Threading;
using NLog;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/* 
 * вводный комментарий ???
 * 
*/
namespace Monitor
{
    static class Log
    {
        internal static Logger logger = LogManager.GetCurrentClassLogger();
    }

    static class Monitor
    {
        private static int lifeTime;

        static public void CheckProcess(object array)
        {
            var parameters = (string[])array;
            var processName = parameters[0];
            if (Int32.TryParse(parameters[1], out lifeTime))
            {
                var maxTime = DateTime.Now.AddHours(lifeTime) - DateTime.Now;
                Process[] localByName = Process.GetProcessesByName(processName);
                if (localByName.Length == 0)
                    return;
                else
                    foreach (Process p in localByName)
                    {
                        var workTime = DateTime.Now - p.StartTime;
                        if (workTime > maxTime)
                        {
                            Killer.KillProcess(processName);
                            return;
                        }
                    }
            }
            else
                Log.logger.Error("Error in type conversion. The second argument does not match a numeric value.");
        }

    }
    static class Killer
    {
        static public void KillProcess(string processName)
        {
            Process[] localByName = Process.GetProcessesByName(processName);
            foreach (Process p in localByName)
            {
                p.Kill();
            }
            Log.logger.Debug($"Process {processName} has been completed");
        }
    }
    static class Clock
    {
        private static int periodicity;
        static public void CallMonitor(string[] args)
        {
            var array = (object)args;
            if (Int32.TryParse(args[2], out periodicity))
            {
                var miliSeconds = periodicity * 60000;
                TimerCallback tm = new TimerCallback(Monitor.CheckProcess);
                Timer timer = new Timer(tm, array, 0, miliSeconds);
            }
            else
                Log.logger.Error("Error in type conversion. The third argument does not match a numeric value.");
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 3)
                Clock.CallMonitor(args);
            else
                Log.logger.Error("Invalid input: number of array elements other than 3");
        }
    }
}

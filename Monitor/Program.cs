using System;
using System.Diagnostics;
using System.Threading;
using NLog;

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
            Log.logger.Debug("Launched method CheckProcess");
            var parameters = (string[])array;
            var processName = parameters[0];
            if (Int32.TryParse(parameters[1], out lifeTime) && lifeTime >= 0)
            {
                var maxTime = DateTime.Now.AddMinutes(lifeTime) - DateTime.Now;
                Process[] localByName = Process.GetProcessesByName(processName);
                if (localByName.Length == 0)
                {
                    Log.logger.Debug("Process not found");
                }
                else
                    foreach (Process p in localByName)
                    {
                        var workTime = DateTime.Now - p.StartTime;
                        if (workTime > maxTime)
                        {
                            Killer.KillProcess(processName);
                            break;
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
            if (Int32.TryParse(args[2], out periodicity)&& periodicity >= 0)
            {
                var miliSeconds = periodicity * 60000;
                TimerCallback tm = new TimerCallback(Monitor.CheckProcess);
                Timer timer = new Timer(tm, array, 0, miliSeconds);
            }
            else
                Log.logger.Error("Error in type conversion. The third argument does not match a numeric value.");
        }
    }
    static class Program
    {
        static public void Main(string[] args)
        {
            Log.logger.Debug("Program was started");
            if (args.Length == 3)
                Clock.CallMonitor(args);
            else
                Log.logger.Error("Invalid input: number of array elements other than three");
        }
    }
}

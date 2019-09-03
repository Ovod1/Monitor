using System;
using System.Diagnostics;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitor
{
    static class Log
    {
        static public void WriteError()
        {

        }
        static public void WriteKill()
        {

        }

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
                Process[] localByName = Process.GetProcessesByName(processName);
                if (localByName.Length == 0)
                    return;
                else
                    foreach (Process p in localByName)
                    {
                        var workTime = DateTime.Now - p.StartTime;
                        if (workTime > lifeTime)
                        {
                            Killer.KillProcess(processName);
                            return;
                        }
                    }
            }
            else
                Log.WriteError();

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
            Log.WriteKill();

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
                Log.WriteError();
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 3)
                Clock.CallMonitor(args);
            else
                Log.WriteError();
        }
    }
}

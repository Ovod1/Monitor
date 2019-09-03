using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.Threading;

namespace MonitorTests
{
    [TestClass]
    public class MonitorTest
    {
        [TestMethod]
        public void ExecutionTask()
        {
            //C:\Program Files (x86)\Adobe\Acrobat Reader DC\Reader
            Process.Start("C://Program Files (x86)//Adobe//Acrobat Reader DC//Reader//AcroRd32.exe");
            //Thread.Sleep(20000);
            Process.Start("C://Users//1ovod1//source//repos//Monitor//Monitor//bin//Debug//Monitor.exe AcroRd32 0 0");
            //Monitor.
        }
    }
}

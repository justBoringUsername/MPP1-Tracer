using System.Collections.Generic;
using System.Diagnostics;

namespace TracerLib
{
    class MethodTracer
    {
        public MethodTracer()
        {
            Stopwatch = new Stopwatch();
            ChildMethods = new List<MethodInfo>();
        }

        private readonly Stopwatch Stopwatch;

        private readonly List<MethodInfo> ChildMethods;

        public void AddChildMethod(MethodInfo childMethod)
        {
            ChildMethods.Add(childMethod);
        }

        public List<MethodInfo> GetChildMethods()
        {
            return ChildMethods;
        }

        public double GetExecutionTime()
        {
            return Stopwatch.ElapsedMilliseconds;
        }

        public void StartTrace()
        {
            Stopwatch.Start();
        }

        public void StopTrace()
        {
            Stopwatch.Stop();
        }
    }
}

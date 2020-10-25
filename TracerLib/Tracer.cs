using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace TracerLib
{
    public class Tracer : ITracer
    {
        public Tracer()
        {
            ThreadTracers = new ConcurrentDictionary<int, ThreadTracer>();
        }

        private readonly ConcurrentDictionary<int, ThreadTracer> ThreadTracers;

        public TraceResult GetTraceResult()
        {
            var threadsInfo = new List<ThreadInfo>();
            foreach (var thread in ThreadTracers)
            {
                threadsInfo.Add(new ThreadInfo(thread.Key, thread.Value.GetThreadMethodList()));
            }
            return new TraceResult(threadsInfo);
        }

        private ThreadTracer GetCurrentThreadTracer()
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;         
            ThreadTracer threadTracer;
            ThreadTracers.TryGetValue(threadId, out threadTracer);
            return threadTracer;
        }

        public void StartTrace()
        {
            ThreadTracer threadTracer = GetCurrentThreadTracer();
            if (threadTracer == null)
            {
                int currentThreadId = Thread.CurrentThread.ManagedThreadId;
                threadTracer = new ThreadTracer();       
                ThreadTracers.TryAdd(currentThreadId, threadTracer);
                
            }
            threadTracer.StartTrace();
        }

        public void StopTrace()
        {
            ThreadTracer threadTracer = GetCurrentThreadTracer();
            threadTracer.StopTrace();
        }
    }
}

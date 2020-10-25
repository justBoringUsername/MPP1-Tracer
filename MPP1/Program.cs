using System;
using System.IO;
using TracerLibrary;
using System.Threading;

namespace MPP1
{
    public class Foo
    {
        private Bar _bar;
        private Tracer _tracer;

        internal Foo(Tracer tracer)
        {
            _tracer = tracer;
            _bar = new Bar(_tracer);
        }

        public void MyMethod()
        {
            _tracer.StartTrace();
            Thread.Sleep(2000);
            _bar.InnerMethod();
            Thread.Sleep(1000);
            _bar.Async();
            _tracer.StopTrace();
        }
    }

    public class Bar
    {
        private Tracer _tracer;

        internal Bar(Tracer tracer)
        {
            _tracer = tracer;
        }

        public void InnerMethod()
        {
            _tracer.StartTrace();
            int a = 0;
            for (int i = 0; i < 10000000; i++)
                a++;
            _tracer.StopTrace();
        }

        public void Async()
        {
            _tracer.StartTrace();
            ThreadStart threadStart = new ThreadStart(InnerMethod);
            Thread thread = new Thread(threadStart);
            thread.Start();
            thread.Join();
            _tracer.StopTrace();
        }
    }

    class Program
    { 
        static void Main(string[] args)
        {
            Tracer tracer = new Tracer();
            Foo foo = new Foo(tracer);
            foo.MyMethod();

            TraceResult traceResult = tracer.GetTraceResult();

            ISerialize serialize1, serialize2;
            serialize1 = new XMLSerialize();

            byte[] data1 = serialize1.Serialize(traceResult);
            Output.WriteToStream(data1, Console.OpenStandardOutput());
            using (FileStream fs = new FileStream("output.txt", FileMode.OpenOrCreate, FileAccess.Write))
                Output.WriteToStream(data1, fs);

           serialize2 = new JSONSerialize();
            byte[] data2 = serialize2.Serialize(traceResult);
            Output.WriteToStream(data2, Console.OpenStandardOutput());
            using (FileStream fs = new FileStream("output.txt", FileMode.Open, FileAccess.Write))
                Output.WriteToStream(data2, fs);

            Console.ReadLine();
        }
        class Output
        {
            public static void WriteToStream(byte[] bytes, Stream outStream)
            {
                outStream.Write(bytes, 0, bytes.Length);
            }
        }

    }
}

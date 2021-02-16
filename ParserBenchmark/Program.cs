using System;
using BenchmarkDotNet.Running;

namespace ParserBenchmark
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("start");
            var summary = BenchmarkRunner.Run<TokenizerBenchmark>();
            //var summary1 = BenchmarkRunner.Run<ObjectDictionaryBenchmark>();
            //var summary = BenchmarkRunner.Run<TeeConnectorEndsDictionaryBenchmark>();
            //var summary = BenchmarkRunner.Run<BothSideCollectionBenchmark>();
            //var summary = BenchmarkRunner.Run<MathBenchmark>();
            // var summary = BenchmarkRunner.Run<PropertyGetterBenchmark>();

            Console.WriteLine("stop");
        }
    }
}
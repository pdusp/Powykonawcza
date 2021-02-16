using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;

namespace ParserBenchmark

{
    [SimpleJob(RunStrategy.ColdStart, targetCount: 20, invocationCount: 1000_000)]
    public class TokenizerBenchmark
    {
        [Benchmark]
        public double Sinus()
        {
            return Math.Sin(123);
        }
    }
}
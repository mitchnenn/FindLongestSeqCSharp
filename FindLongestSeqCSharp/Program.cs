using System;
using System.Diagnostics;

namespace FindLongestSeqCSharp
{
    partial class Program
    {
        static void Main(string[] args)
        {
            var input = new [] {0, 8, 4, 12, 2, 10, 6, 14, 1, 9, 5, 13, 3, 11, 7, 15};
            var state = new LongestSeqAlgorithm(input);
            var stopWatch = Stopwatch.StartNew();
            var longestIncSeq = state.FindLongestIncreasingSeq();
            var longestDecSeq = state.FindLongestDecreasingSeq();
            stopWatch.Stop();
            Console.WriteLine($"Find longest seq took {stopWatch.ElapsedMilliseconds} ms");
            Console.WriteLine(string.Join(",", longestIncSeq));        
            Console.WriteLine(string.Join(",", longestDecSeq));        
        }
    }
}
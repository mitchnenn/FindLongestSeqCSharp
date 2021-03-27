using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FParsec.CSharp;
using static FParsec.CSharp.PrimitivesCS; // combinator functions
using static FParsec.CSharp.CharParsersCS; // pre-defined parsers

namespace FindLongestSeqCSharp
{
    partial class Program
    {
        private static IEnumerable<int> ParseInput()
        {
            var countParser = Int.AndL(Newline);
            var seqParser = Many(Spaces.AndRTry(Int));
            var entryParser = Tuple(countParser, seqParser);
            var allEntries = Many1(entryParser);
            var path = Path.Combine(AppContext.BaseDirectory, "data.txt");
            var result = allEntries.RunOnFile(path).GetResult();
            return result[0].Item2;
        }
        
        static void Main(string[] args)
        {
            var input = ParseInput().ToArray();
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
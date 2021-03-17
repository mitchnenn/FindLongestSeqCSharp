using System;
using System.Collections.Generic;
using System.Text;

namespace FindLongestSeqCSharp
{
    public class State
    {
        /// <summary>
        /// Denote the sequence values as X[0], X[1], etc. Then, after
        /// processing X[i], the algorithm will have stored values in
        /// two arrays M and P.
        /// </summary>
        private readonly IReadOnlyList<int> _X;
        /// <summary>
        /// M[j] — stores the index k of the smallest value X[k] such
        /// that there is an increasing subsequence of length j ending
        /// at X[k] on the range k ≤ i.  Note that j ≤ (i+1), because j ≥ 1
        /// represents the length of the increasing subsequence, and k ≥ 0
        /// represents the index of its termination.
        /// </summary>
        public int[] _M;
        /// <summary>
        /// P[k] — stores the index of the predecessor of X[k] in the longest
        /// increasing subsequence ending at X[k].
        /// </summary>
        public int[] _P;
        
        public State(IReadOnlyList<int> x)
        {
            _X = x;
            Initialize();
        }

        public int InputLength => _X.Count;
        
        public int LongestSeqIndex { get; set; }
        
        public int CurrentInputIndex { get; set; }

        private void Initialize()
        {
            _P = new int[InputLength];
            _M = new int[InputLength + 1];
            LongestSeqIndex = 0;
        }
        
        public IEnumerable<int> FindLongestIncSeq()
        {
            for (var currentInputIndex = 0; currentInputIndex < InputLength; currentInputIndex++)
            {
                CurrentInputIndex = currentInputIndex;
                var newLongestSeqIndex = SearchLargestPossibleValue();
                _P[currentInputIndex] = _M[newLongestSeqIndex - 1];
                _M[newLongestSeqIndex] = currentInputIndex;
                if (newLongestSeqIndex > LongestSeqIndex)
                    LongestSeqIndex = newLongestSeqIndex;
                Console.WriteLine(this);
            }
            return ReconstructLongestSeq();
        }

        private int SearchLargestPossibleValue()
        {
            var lo = 1;
            var hi = LongestSeqIndex;
            while (lo <= hi)
            {
                var mid = (int) Math.Ceiling((lo + hi) / 2.0);
                if (_X[_M[mid]] < _X[CurrentInputIndex])
                    lo = mid + 1;
                else
                    hi = mid - 1;
            }
            return lo;
        }

        private IEnumerable<int> ReconstructLongestSeq()
        {
            var result = new int[LongestSeqIndex];
            var k = _M[LongestSeqIndex];
            for (var index = LongestSeqIndex - 1; index >= 0; index--)
            {
                result[index] = _X[k];
                k = _P[k];
            }
            return result;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"{nameof(CurrentInputIndex)}:{CurrentInputIndex}");
            sb.AppendLine($"{nameof(LongestSeqIndex)}:{LongestSeqIndex}");
            sb.AppendLine($"Input:\t\t\t\t[{string.Join(",", _X)}]");
            sb.AppendLine($"CurrentLongestSeqIndexes:\t[{string.Join(",", _P)}]");
            sb.AppendLine($"IntermediateIndexes:\t\t[{string.Join(",", _M)}]");
            sb.AppendLine($"Longest Seq:\t\t\t[{string.Join(",", ReconstructLongestSeq())}]");
            return sb.ToString();
        }
    }
}
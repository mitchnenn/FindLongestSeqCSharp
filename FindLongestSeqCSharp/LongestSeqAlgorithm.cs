using System;
using System.Collections.Generic;
using System.Text;

namespace FindLongestSeqCSharp
{
    public class LongestSeqAlgorithm
    {
        private delegate bool CompareFunc(int x, int y);

        private static bool Increasing(int x, int y) { return y > x; }
        private static bool Decreasing(int x, int y) { return x > y; }
        
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
        private int[] _M;
        /// <summary>
        /// P[k] — stores the index of the predecessor of X[k] in the longest
        /// increasing subsequence ending at X[k].
        /// </summary>
        private int[] _P;

        private int _longestSeqIndex;
        private int _currentInputIndex;

        public LongestSeqAlgorithm(IReadOnlyList<int> x)
        {
            _X = x;
        }

        private void Initialize()
        {
            _P = new int[_X.Count];
            _M = new int[_X.Count + 1];
            _longestSeqIndex = 0;
        }

        public IEnumerable<int> FindLongestIncreasingSeq()
        {
            return FindLongestSeq(Increasing);
        }

        public IEnumerable<int> FindLongestDecreasingSeq()
        {
            return FindLongestSeq(Decreasing);
        }
        
        private IEnumerable<int> FindLongestSeq(CompareFunc compareFunc)
        {
            Initialize();
            for (var currentInputIndex = 0; currentInputIndex < _X.Count; currentInputIndex++)
            {
                _currentInputIndex = currentInputIndex;
                var newLongestSeqIndex = SearchLargestPossibleValue(compareFunc);
                _P[currentInputIndex] = _M[newLongestSeqIndex - 1];
                _M[newLongestSeqIndex] = currentInputIndex;
                if (newLongestSeqIndex > _longestSeqIndex)
                    _longestSeqIndex = newLongestSeqIndex;
            }
            return ReconstructLongestSeq();
        }

        private int SearchLargestPossibleValue(CompareFunc compareFunc)
        {
            var lo = 1;
            var hi = _longestSeqIndex;
            while (lo <= hi)
            {
                var mid = (int) Math.Ceiling((lo + hi) / 2.0);
                if (compareFunc(_X[_M[mid]], _X[_currentInputIndex]))
                    lo = mid + 1;
                else
                    hi = mid - 1;
            }
            return lo;
        }

        private IEnumerable<int> ReconstructLongestSeq()
        {
            var result = new int[_longestSeqIndex];
            var k = _M[_longestSeqIndex];
            for (var index = _longestSeqIndex - 1; index >= 0; index--)
            {
                result[index] = _X[k];
                k = _P[k];
            }
            return result;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"{nameof(_currentInputIndex)}:{_currentInputIndex}");
            sb.AppendLine($"{nameof(_longestSeqIndex)}:{_longestSeqIndex}");
            sb.AppendLine($"Input:\t\t\t\t[{string.Join(",", _X)}]");
            sb.AppendLine($"CurrentLongestSeqIndexes:\t[{string.Join(",", _P)}]");
            sb.AppendLine($"IntermediateIndexes:\t\t[{string.Join(",", _M)}]");
            sb.AppendLine($"Longest Seq:\t\t\t[{string.Join(",", ReconstructLongestSeq())}]");
            return sb.ToString();
        }
    }
}
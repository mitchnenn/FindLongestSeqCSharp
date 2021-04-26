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

        private int _L;

        public LongestSeqAlgorithm(IReadOnlyList<int> x)
        {
            _X = x;
        }

        private void Initialize()
        {
            _P = new int[_X.Count];
            _M = new int[_X.Count + 1];
            _L = 0;
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
                // After searching, newL is 1 greater than the
                // length of the longest prefix of X[i]
                var newL = BinarySearchLargestValue(compareFunc, currentInputIndex);
                
                // The predecessor of X[i] is the last index of 
                // the subsequence of length newL-1
                _P[currentInputIndex] = _M[newL - 1];
                _M[newL] = currentInputIndex;
                
                // If we found a subsequence longer than any we've
                // found yet, update L
                if (newL > _L)
                    _L = newL;
            }
            
            return ReconstructLongestSeq();
        }

        /// <summary>
        /// Binary search for the largest positive j ≤ L
            /// such that X[M[j]] <= X[i] or X[M[j]] >= X[i] 
        /// </summary>
        /// <param name="compareFunc"></param>
        /// <returns></returns>
        private int BinarySearchLargestValue(CompareFunc compareFunc, int currentInputIndex)
        {
            var lo = 1;
            var hi = _L;
            while (lo <= hi)
            {
                var mid = (int) Math.Ceiling((lo + hi) / 2.0);
                if (compareFunc(_X[_M[mid]], _X[currentInputIndex]))
                    lo = mid + 1;
                else
                    hi = mid - 1;
            }
            return lo;
        }

        /// <summary>
        /// Reconstruct the longest increasing subsequence
        /// </summary>
        /// <returns></returns>
        private IEnumerable<int> ReconstructLongestSeq()
        {
            var S = new int[_L];
            var k = _M[_L];
            for (var index = _L - 1; index >= 0; index--)
            {
                S[index] = _X[k];
                k = _P[k];
            }
            return S;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"_L:{_L}");
            sb.AppendLine($"X:\t[{string.Join(",", _X)}]");
            sb.AppendLine($"P:\t[{string.Join(",", _P)}]");
            sb.AppendLine($"M:\t[{string.Join(",", _M)}]");
            sb.AppendLine($"S:\t[{string.Join(",", ReconstructLongestSeq())}]");
            return sb.ToString();
        }
    }
}
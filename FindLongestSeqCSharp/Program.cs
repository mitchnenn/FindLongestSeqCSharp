using System;
using System.Collections.Generic;

namespace FindLongestSeqCSharp
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = new [] {0, 8, 4, 12, 2, 10, 6, 14, 1, 9, 5, 13, 3, 11, 7, 15};
            var longestSeqIndexes = FindLongestSeq(input, out var M, out var L);
            var longestSeq = ReconstructLongestSeq(L, M, input, longestSeqIndexes);
            Console.WriteLine(string.Join(",", longestSeq));        
        }

        private static int[] FindLongestSeq(int[] input, out int[] M, out int currentLongestSeqLen)
        {
            var inputLength = input.Length;
            var currentLongestSeqIndexes = new int [inputLength];
            M = new int [inputLength + 1];
            currentLongestSeqLen = 0;
            for (var currentInputIndex = 0; currentInputIndex <= inputLength - 1; currentInputIndex++)
            {
                Console.Write($"{nameof(currentInputIndex)}:{currentInputIndex} ", currentInputIndex);

                var lo = SearchLargestPossibleValue(input, M, currentLongestSeqLen, currentInputIndex);
                Console.WriteLine($"{nameof(lo)}:{lo} ", lo);

                // After searching, lo is 1 greater than the
                // length of the longest prefix of X[i]
                var newLongestSeqLen = lo;

                // The predecessor of X[i] is the last index of 
                // the subsequence of length newL-1
                currentLongestSeqIndexes[currentInputIndex] = M[newLongestSeqLen - 1];
                M[newLongestSeqLen] = currentInputIndex;

                if (newLongestSeqLen > currentLongestSeqLen)
                {
                    // If we found a subsequence longer than any we've
                    // found yet, update L
                    currentLongestSeqLen = newLongestSeqLen;
                }
                
                Console.WriteLine($"{nameof(newLongestSeqLen)}:{newLongestSeqLen} ", newLongestSeqLen);
                
                var longestSeq = ReconstructLongestSeq(currentLongestSeqLen, M, input, currentLongestSeqIndexes);
                Console.WriteLine(string.Join(",", longestSeq));        
                
            }

            return currentLongestSeqIndexes;
        }

        private static string WriteInputValues(IReadOnlyList<int> input, IEnumerable<int> indices)
        {
            var valueArray = new int [input.Count];
            foreach (var index in  indices)
            {
                valueArray[index] = input[index];
            }
            return string.Join(",", valueArray);
        }

        private static int SearchLargestPossibleValue(
            IReadOnlyList<int> input, 
            IReadOnlyList<int> M, 
            int L, 
            int currentInputIndex)
        {
            // Binary search for the largest positive j ≤ L
            // such that X[M[j]] <= X[i]
            var lo = 1;
            var hi = L;
            while (lo <= hi)
            {
                var mid = (int) Math.Ceiling((lo + hi) / 2.0);
                if (input[M[mid]] < input[currentInputIndex])
                {
                    lo = mid + 1;
                }
                else
                {
                    hi = mid - 1;
                }
            }

            return lo;
        }

        private static IEnumerable<int> ReconstructLongestSeq(
            int longestSeqLen, 
            int[] M, 
            int[] input, 
            int[] currentLongestIndexes)
        {
            Console.WriteLine($"{nameof(longestSeqLen)}:{longestSeqLen}");
            Console.WriteLine("P:[{0}]", string.Join(",", currentLongestIndexes));
            Console.WriteLine("M:[{0}] ", string.Join(",", M));
            Console.WriteLine("X:[{0}]", string.Join(",", input));
            
            // Reconstruct the longest increasing subsequence
            var result = new int[longestSeqLen];
            var k = M[longestSeqLen];
            for (var index = longestSeqLen-1; index >= 0; index--)
            {
                Console.Write($"{nameof(k)}:{k}");
                Console.WriteLine($" {input[k]}");
                result[index] = input[k];
                k = currentLongestIndexes[k];
            }

            return result;
        }
    }
}
using System;

namespace rm.Extensions
{
    /// <summary>
    /// Int extensions.
    /// </summary>
    public static class IntExtension
    {
        /// <summary>
        /// n!
        /// </summary>
        public static int Factorial(this int n)
        {
            checked
            {
                var product = 1;
                for (int i = 1; i <= n; i++)
                {
                    product *= i;
                }
                return product;
            }
        }
        /// <summary>
        /// nPr
        /// </summary>
        public static int Permutation(this int n, int r)
        {
            checked
            {
                return n.Factorial() / (n - r).Factorial();
            }
        }
        /// <summary>
        /// nCr
        /// </summary>
        public static int Combination(this int n, int r)
        {
            checked
            {
                return n.Factorial() / ((n - r).Factorial() * r.Factorial());
            }
        }
        /// <summary>
        /// Scrabble count for n. 
        /// </summary>
        /// <remarks>nP1 + nP2 + ... + nPn</remarks>
        public static int ScrabbleCount(this int n)
        {
            checked
            {
                var sum = 0;
                for (int i = 1; i <= n; i++)
                {
                    sum += n.Permutation(i);
                }
                return sum;
            }
        }
    }
}

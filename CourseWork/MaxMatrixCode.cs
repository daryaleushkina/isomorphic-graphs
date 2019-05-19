using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWork
{
    class MaxMatrixCode
    {
        int[,] adjacencyMatrix;
        public MaxMatrixCode(int[,] a)
        {
            adjacencyMatrix = a;
        }
        public static int factorNum(int n)
        {
            int result = 1;
            for (int i = 2; i <= n; i++)
            {
                result *= i;
            }
            return result;
        }
        public int GetMaxMatrixCode
        {
            get
            {
                int n = Convert.ToInt32(Math.Sqrt(adjacencyMatrix.Length)) ;
                int[] mapping = new int[n];
                int[] perputation = new int[n];
                Permutation perm = new Permutation();
                for (int i = 0; i < n; i++)
                {
                    perputation[i] = i;
                }
                int factorial = factorNum(n);
                int ansver = 0;
                for (int i = 0; i < factorial; i++)
                {
                    String s = "";
                    for (int j = 0; j < n; j++)
                    {
                        mapping[perputation[j]] = j;
                    }
                    for (int j = 0; j < n; j++)
                        for (int k = j + 1; k < n; k++)
                        {
                            s += adjacencyMatrix[mapping[j], mapping[k]].ToString();
                        }
                    ansver = Math.Max(ansver, Convert.ToInt32(s, 2));
                    perm.nextPermutation(perputation);
                }
                return ansver;
            }
        }

    }
}

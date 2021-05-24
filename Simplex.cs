using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK_LB_2_SIMPLEX
{
    class Simplex
    {
        private double[,] table; //simplex table
        private Dictionary<int,int> N = new Dictionary<int, int>(); // non base variables
        private Dictionary<int, int> B = new Dictionary<int, int>(); // base variables
        private int n, m;

        public Simplex(double[] c, double[,] A, double[] b)
        {
            n = A.GetLength(1); m = A.GetLength(0);

           

            if (m != b.Length)
            {
                throw new Exception("Number of constraints in A doesn't match number in b.");
            }

            this.table = new double[m + 1, n - m + 1]; 
            table[0, 0] = c[0];
            for (int i = 1; i < n - m + 1; i++)
            {
              
                table[0, i] = c[i];
            }


            bool basis;
            int basisI = 0;
            for (int i = 0; i < n; i++)
            {
                if (basisI == m + 1)
                    break;
                basis = false;
                int j = 0;
                while (j < m && A[j, i] != 1)
                    j++;
                if (j == m)
                    continue;
                else
                {
                    basis = true;
                    j++;
                    while (j < m)
                    {
                        if(A[j,i] != 0)
                        {
                            basis = false;
                            break;
                        }
                        j++;
                    }

                    if (basis)
                    {
                        B.Add(basisI, i);
                        basisI++;
                    }
                }
            }

            for (int i = 1; i < m + 1; i++)
            {
                table[i, 0] = b[i - 1];
                int colI = 1;
                for (int j = 0; j < n; j++)
                {
                    if (B.ContainsValue(j))
                        continue;
                    table[i, colI] = A[i - 1, j];
                    colI++;
                }
            }

            //log();

            for (int i = 0; i < n; i++) 
            {
                if (B.ContainsValue(i))
                    continue;
                N.Add(i,i);
            }            
        }
        public void log()
        {
            for (int j = 0; j < table.GetLength(0); j++)
            {
                for (int k = 0; k < table.GetLength(1); k++)
                {
                    Console.Write(String.Format("{0,4:F1} ", table[j, k]));
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        public Tuple<double, double[]> maximize()
        {
            while (true)
            {

                int e = -1;
                for (int i = 1; i < table.GetLength(1); i++)
                {
                    if (table[0, i] > 0)
                    {
                        e = i;
                        break;
                    }
               }

                // If no coefficient > 0, there's no more maximizing to do, and we're almost done
                if (e == -1) break;

                
                int l = 1;
                while (l< table.GetLength(0) && table[l, e] <= 0)
                    l++;

                double ratio = table[0, e] / table[l, e];
                
                for(int i = l + 1; i < table.GetLength(0); i++)
                    if(table[i,e]!=0 && table[0, e] / table[i, e] < ratio)
                    {
                        ratio = table[0, e] / table[i, e];
                        l = i;
                    }

                pivot(e, l);
            }

            double[] x = new double[n];
            
            foreach(var key in B.Keys)
            {
                x[B[key]] = table[key+1, 0]; 
            }

            // Return max and variables
            return Tuple.Create<double, double[]>(table[0, 0], x);
        }
        private void pivot(int l, int e)
        {
           
            double[,] table2 = new double[table.GetLength(0), table.GetLength(1)];
            table2[l, e] = 1 / table[l, e];

            for (int i = 0; i < table.GetLength(1); i++)
            {
                if (i == e)
                    continue;
                table2[l, i] = table[l, i] * table2[l, e];
            }

            for (int i = 0; i < table.GetLength(0); i++)
            {
                if (i == l)
                    continue;
                table2[i, e] = table[i, e] * -table2[l, e];
            }

            for(int i =0; i< table.GetLength(0); i++)
            {
                if (i == l)
                    continue;
                for(int j = 0; j < table.GetLength(1); j++)
                {
                    if (j == e)
                        continue;
                    table2[i, j] = table2[i, e] * table[l, j];
                }
            }

            for (int i = 0; i < table.GetLength(0); i++)
            {
                for (int j = 0; j < table.GetLength(1); j++)
                {
                    if (j == e || i == l)
                        table[i, j] = table2[i, j];
                    else
                    {
                        table[i, j] = table[i, j] + table2[i, j];
                    }
                }
            }
            int temp = N[e - 1];
            N[e - 1] = B[l - 1];
            B[l - 1] = temp;
        }
    }
}

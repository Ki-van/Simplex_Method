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
        private Dictionary<int, int> B = new Dictionary<int, int>(); // base variables
        private int n, m;

        public Simplex(double[] c, double[,] A, double[] b)
        {
            n = A.GetLength(1); m = A.GetLength(0);

           if(n != c.Length - 1)
            {
                throw new Exception();
            }

            if (m != b.Length)
            {
                throw new Exception("Number of constraints in A doesn't match number in b.");
            }

            table = new double[m + 1, n + m + 1]; //m + c string, n + m(artifical basis) + 1(b column)

            //Fill table with c
            table[m, 0] = c[0];
            for (int i = 1; i < n + 1; i++)
            { 
                table[m, i] = -c[i];
            }

            //Fill with A and b
            for (int i = 0; i < A.GetLength(0); i++)
            {
                for (int j = 0; j < A.GetLength(1) + 1; j++)
                {
                    if (j == 0)
                    {
                        table[i, j] = b[i];
                        continue;
                    }
                    table[i, j] = A[i, j - 1];
                }
            }

            //Fill basis 1-s
            for (int i = n + 1, j = 0; j < table.GetLength(0) - 1; j++, i++)
            {
                table[j, i] = 1;
            }
           
            log();

            for (int i = 0; i < m; i++)
            {
                B.Add(i, i + n + 1);
            }
        }
       

        public Tuple<double, double[]> maximize()
        {
            while (true)
            {

                int e = -1;
                int l = -1;
                double MaxMinRatio = -1;

                for (int i = 1; i < n + 1; i++)
                {
                    if (table[m, i] < 0)
                    {
                        int _e = i;
                        int _l = 0;
                        while (_l < table.GetLength(0) - 1 && table[_l, _e] <= 0)
                            _l++;

                        double ratio = table[_l, 0] / table[_l, _e];

                        for (int j = _l + 1; j < table.GetLength(0); j++)
                            if (table[j, _e] > 0 && table[j, 0] / table[j, _e] < ratio)
                            {
                                ratio = table[j, 0] / table[j, _e];
                                _l = j;
                            }
                        if(ratio > MaxMinRatio)
                        {
                            MaxMinRatio = ratio;
                            e = _e;
                            l = _l;
                        }
                    }
               }


                if (e == -1)
                {
                    if (B.Values.Max() < n + 1)
                        break;
                    else
                    {
                        l = B.FirstOrDefault(x => x.Value == B.Values.Max()).Key;
                        double maxValue = -1;
                        double objectiveVal;

                        for (int i = 1; i < n + 1; i++)
                        {
                            if (table[l, i] > 0)
                            {
                                objectiveVal = table[m, 0] + (table[l, 0] / table[l, i]) * (-table[m, i]);
                                
                                if(objectiveVal > maxValue)
                                {
                                    maxValue = objectiveVal;
                                    e = i;
                                }
                            }
                        }
                    }
                }

                pivot(l, e);
            }
            Console.WriteLine();
            log();

            double[] x = new double[n];
            
            foreach(int key in B.Keys)
            {
                x[B[key] - 1] = table[key, 0]; 
            }
           

            // Return max and variables
            return Tuple.Create<double, double[]>(table[m, 0], x);
        }
        private void pivot(int l, int e)
        {
            double ratio = 1 / table[l, e];
            for (int i = 0; i < table.GetLength(1); i++)
            {
                table[l, i] *= ratio;
            }

            for (int i = 0; i < table.GetLength(0); i++)
            {
                if (i == l)
                    continue;
                if (table[i, e] == 0)
                    continue;

                ratio = -table[i, e];
                for (int j = 0; j < table.GetLength(1); j++)
                {
                    table[i, j] += table[l, j] * ratio;
                }
            }
            B.Remove(l);
            B.Add(l, e);
        }

        public void log()
        {
            for (int i = 0; i < table.GetLength(1); i++)
            {
                if (i == 0)
                    Console.Write(String.Format("{0, 7}|", "bi"));
                else
                {
                    Console.Write(String.Format("{0,7}|", "x" + i));
                }
            }
            Console.WriteLine();
            for (int i = 0; i < table.GetLength(0); i++)
            {
                for (int j = 0; j < table.GetLength(1); j++)
                {
                    Console.Write(String.Format("{0, 7:F1}|", table[i, j]));
                }
                Console.WriteLine();
            }
        }
    }
}

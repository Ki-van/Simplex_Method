using System;

namespace MMK_LB_2_SIMPLEX
{
    class Program
    {
        static void Main(string[] args)
        {
            var s = new Simplex(
              new double[] { 0, 6.5, 0, -7.5, 23.5, -5, },
              new double[,] {
                { 1, 3, 1,  4,  0 },
                { 2, 0, -1, 12, -1},
                { 1, 2, 0,  3,  -1},
              },
              new double[] { 12, 14, 6 }
            );
           /* var s = new Simplex(
             new double[] { 0, 3, 0, 0, 0, 2,-5 },
             new double[,] {
                { 2, 1, 0,  0,  -3,5 },
                { 4, 0, 1, 0, 2,-4},
                { -3, 0, 0,  1,  -3,6},
             },
             new double[] { 34,28,24 }
           );*/
            var rz = s.maximize();
            Console.WriteLine("F = " + rz.Item1);
            for (int i = 0; i < rz.Item2.Length; i++)
            {
                Console.WriteLine("X{0} = {1}", i + 1, rz.Item2[i]);
            }

           
        }
    }
}

using System;

namespace MMK_LB_2_SIMPLEX
{
    class Program
    {
        static void Main(string[] args)
        {
            var s = new Simplex(
              new double[] { 36.4, -3.2, -12, },
              new double[,] {
          {1, 0, -1.0/5.0, 7, 0},
          {0, 1, 2.0/5.0, -1, 0},
          {0, 0, 3.0/5.0, 2, 1}
              },
              new double[] { 9.6, 0.8, 5.2 }
            );
            var rz = s.maximize();
            Console.WriteLine("F = " + rz.Item1);
            for (int i = 0; i < rz.Item2.Length; i++)
            {
                Console.WriteLine("X{0} = {1}", i+1, rz.Item2[i]);
            }
        }
    }
}

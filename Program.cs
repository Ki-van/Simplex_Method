using System;

namespace MMK_LB_2_SIMPLEX
{
    class Program
    {
        static void Main(string[] args)
        {
            var s = new Simplex(
              new double[] { 6.5,0,-7.5, 23.5,-5 },
              new double[,] {
          {1,3,1,4,0},
          {2,0,-1,12,-1},
          {1,2,0,3,-5}
              },
              new double[] { 12,14,6 }
            );

            
            Console.WriteLine("Максимальное значение целевой функции: ", answer.Item1);
            Console.WriteLine(string.Join(", ", answer.Item2));
        }
    }
}

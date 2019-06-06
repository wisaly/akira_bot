using System;
using System.Collections.Generic;
using System.Text;

namespace libtest
{
    class TestRand
    {
        Random rand_ = new Random();
        public TestRand()
        {

        }

        public void run()
        {
            const int range = 10;
            const int tot = 1000000;
            int[] hit = new int[range];
            for (int i = 0; i < hit.Length; i++)
                hit[i] = 0;

            for (int i = 0; i < tot; i++)
                hit[rand_.Next(range)]++;

            for (int i = 0; i < hit.Length; i++)
            {
                Console.WriteLine($"{i}: {hit[i]},  {(decimal)hit[i] / (decimal)tot * 100}%");
            }
        }
    }
}

using System;

namespace AdventOfCode2019
{
    class Program
    {
        static void Main(string[] args)
        {
            var task = new Task3();

            while (true)
            {
                task.Start2();
                Console.ReadKey();
            }

        }
    }
}

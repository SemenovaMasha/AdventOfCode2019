using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AdventOfCode2019
{
    class Task17 : TaskI
    {
        public override int TaskNumber => 17;

        public override void Start1()
        {
            var filename = GetFileName1();
            var amplifier =new Amplifier()
            {
                IntCode = GetLongList_BySeparator(filename, ","),
            };

            amplifier.WorkUntilHaltOrWaitForInput();
            var output = amplifier.Output;

            var screen = new Screen(output.Count(x => x == 10), output.IndexOf(10)+1);
            int i = 0, j = 0;
            foreach(var ch in output)
            {

                if (ch == 10)
                {
                    j = 0;
                    i++;
                    continue;
                }
                screen.Grid[i, j] = ch;

                j++;
            }

            screen.Print();

            Console.WriteLine(screen.GetIntersectionsSum());
        }

        public override void Start2()
        {
            var filename = GetFileName1();
            var amplifier = new Amplifier()
            {
                IntCode = GetLongList_BySeparator(filename, ","),
            };

            amplifier.IntCode[0] = 2;

            var list = new List<int> {'A', ',', 'B', ',', 'A', ',', 'B', ',', 'A', ',', 'C', ',', 'B', ',', 'C', ',', 'A', ',', 'C',  10
                                    , 'R', ',', '4', ',', 'L', ',', '1', '0', ',', 'L', ',', '1', '0', 10
                                    , 'L', ',', '8', ',', 'R', ',', '1', '2', ',', 'R', ',', '1', '0', ',', 'R', ',', '4', 10
                                    , 'L', ',', '8', ',', 'L', ',', '8', ',', 'R', ',', '1', '0', ',', 'R', ',', '4', 10
                                    , 'n', 10
            };

            foreach(var n in list)
            {
                amplifier.IO.AddLast(n);
            }
            amplifier.WorkUntilHaltOrWaitForInput();

            var output = amplifier.Output;
            foreach (var ch in output)
            {
                Console.Write((char)ch);
            }
                                  
        }
        
        class Screen
        {
            /// <summary>
            /// 0 is an empty tile. No game object appears in this tile.
            /// 1 is a wall tile.Walls are indestructible barriers.
            /// 2 is a block tile.Blocks can be broken by the ball.
            /// 3 is a horizontal paddle tile. The paddle is indestructible.
            /// 4 is a ball tile.The ball moves diagonally and bounces off objects.
            /// </summary>
            public long[,] Grid { get; set; }
            public Screen(int N,int M)
            {
                Grid = new long[N, M];
            }
            public void Print()
            {
                for (int i = 0; i < Grid.GetLength(0); i++)
                {
                    for (int j = 0; j < Grid.GetLength(1); j++)
                    {
                        Console.Write((char)Grid[i,j]);
                    }

                    Console.Write((char)10);
                }

                Console.WriteLine();
            }

            public int GetIntersectionsSum()
            {
                int res = 0;
                for(int i = 1; i < Grid.GetLength(0) - 1; i++)
                {
                    for(int j = 1; j < Grid.GetLength(1) - 1; j++)
                    {
                        if (Grid[i - 1, j] == '#' && Grid[i, j - 1] == '#' && Grid[i + 1, j] == '#' && Grid[i, j + 1] == '#' && Grid[i, j] == '#')
                            res+=i*j;
                    }
                }
                return res;
            }

        }
    }
}

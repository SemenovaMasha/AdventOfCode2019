using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AdventOfCode2019
{
    class Task13 : TaskI
    {
        public override int TaskNumber => 13;

        public override void Start1()
        {
            var filename = GetFileName1();
            var amplifier =new Amplifier()
            {
                IntCode = GetLongList_BySeparator(filename, ","),
            };

            while (!amplifier.IsHalt())
            {
                amplifier.WorkUntilHaltOrWaitForInput();
                if (amplifier.IsHalt())
                    break;

            }
            var output = amplifier.Output;
            var screen = new Screen(50);

            for (int i = 0; i < output.Count; i+=3)
            {
                screen.Grid[output[i], output[i + 1]] = output[i + 2];
            }

            long blockTiles = 0;
            foreach (var tile in screen.Grid)
            {
                if (tile == 2) blockTiles++;
            }

            Console.WriteLine(blockTiles);
        }

        public override void Start2()
        {
            var filename = GetFileName1();
            var amplifier = new Amplifier()
            {
                IntCode = GetLongList_BySeparator(filename, ","),
            };

            //amplifier.IO.AddFirst(2);
            amplifier.IntCode[0] = 2;
            //amplifier.IO.AddLast(0);

            while (true)
            {
                PrintScreen(amplifier);
                ConsoleKeyInfo _Key = Console.ReadKey();
                switch (_Key.Key)
                {
                    case ConsoleKey.RightArrow:
                        amplifier.IO.AddLast(1);
                        break;
                    case ConsoleKey.LeftArrow:
                        amplifier.IO.AddLast(-1);
                        break;
                    default:
                        amplifier.IO.AddLast(0);
                        break;
                }

                amplifier.WorkUntilHaltOrWaitForInput();
                if (amplifier.IsHalt())
                    break;

                PrintScreen(amplifier);
                

                
                //if(screen.GetBlockTilesCount()==0)
                //    break;

            }

            Console.Clear();
            Console.WriteLine("Game over");
        }

        private void PrintScreen(Amplifier amplifier)
        {
            var output = amplifier.Output;
            var screen = new Screen(40);

            for (int i = 0; i < output.Count; i += 3)
            {
                if (output[i] == -1 && output[i + 1] == 0)
                {
                    Console.WriteLine($"Score = {output[i + 2]}");

                }
                else
                    screen.Grid[output[i], output[i + 1]] = output[i + 2];
            }

            screen.Print();
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

            public Screen(int size)
            {
                Grid = new long[size, size];

            }

            public void Print()
            {
                for (int j = 0; j < 21; j++)
                {
                    for (int i = 0; i < Grid.GetLength(0); i++)
                    {
                        if (Grid[i, j] == 0)
                            Console.Write(" ");
                        else
                            Console.Write(Grid[i, j]);
                    }

                    Console.WriteLine();
                }

                Console.WriteLine();
            }

            public long GetBlockTilesCount()
            {
                long blockTiles = 0;
                foreach (var tile in Grid)
                {
                    if (tile == 2) blockTiles++;
                }

                return blockTiles;
            }
        }
    }
}

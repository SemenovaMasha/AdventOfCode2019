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

            amplifier.IntCode[0] = 2;

            var screen = new Screen(40);
            int nextDrawPosition = 0;
            long step = 0;
            while (true)
            {
                step++;

                if (amplifier.Output.Count > nextDrawPosition)
                {
                    screen.AddMoves(amplifier.Output.GetRange(nextDrawPosition, amplifier.Output.Count - nextDrawPosition));
                    nextDrawPosition = amplifier.Output.Count;
                }
                //ConsoleKeyInfo _Key = Console.ReadKey();
                //switch (_Key.Key)
                //{
                //    case ConsoleKey.RightArrow:
                //        amplifier.IO.AddLast(1);
                //        break;
                //    case ConsoleKey.LeftArrow:
                //        amplifier.IO.AddLast(-1);
                //        break;
                //    default:
                //        amplifier.IO.AddLast(0);
                //        break;
                //}

                long current3X = screen.Get3X();
                long current4X = screen.Get4X();
                if (current4X > current3X)
                {
                    amplifier.IO.AddLast(1);
                }else if (current4X < current3X)
                {
                    amplifier.IO.AddLast(-1);
                }
                else amplifier.IO.AddLast(0);

                //if (step % 1 == 0)
                //{
                //    screen.PrintScreen();
                //    Console.ReadKey();
                //}

                amplifier.WorkUntilHaltOrWaitForInput();
                if (amplifier.IsHalt())
                    break;

            }

            Console.Clear();
            Console.WriteLine("Game over");
            Console.WriteLine($"Score = {amplifier.Output.Last()}");
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
            public long Score { get; set; }
            public Screen(int size)
            {
                Grid = new long[size, size];

            }

            public void AddMoves(List<long> output)
            {
                for (int i = 0; i < output.Count; i += 3)
                {
                    if (output[i] == -1 && output[i + 1] == 0)
                    {
                        Score = output[i + 2];
                    }
                    else
                        Grid[output[i], output[i + 1]] = output[i + 2];
                }
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

            public long Get3X()
            {
                for (int i = 0; i < Grid.GetLength(0); i++)
                {
                    for (int j = 0; j < Grid.GetLength(1); j++)
                    {
                        if (Grid[i, j] == 3) return i;
                    }
                }
                return 0;
            }
            public long Get4X()
            {
                for (int i = 0; i < Grid.GetLength(0); i++)
                {
                    for (int j = 0; j < Grid.GetLength(1); j++)
                    {
                        if (Grid[i, j] == 4) return i;
                    }
                }
                return 0;
            }

            public void PrintScreen()
            {
                Console.WriteLine($"3 x = {Get3X()}");
                Console.WriteLine($"4 x = {Get4X()}");
                Console.WriteLine($"Score = {Score}");

                Print();
            }
        }
    }
}

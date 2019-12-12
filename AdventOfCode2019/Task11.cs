using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AdventOfCode2019
{
    class Task11 : TaskI
    {
        public override int TaskNumber => 11;

        public override void Start1()
        {
            var filename = GetFileName1();
            var amplifier =new Amplifier()
            {
                IntCode = GetLongList_BySeparator(filename, ","),
            };

            var Grid = new Grid(10_000);

            while (!amplifier.IsHalt())
            {
                amplifier.IO.AddLast(Grid.GetCurrentPanelColor());
                amplifier.WorkUntilHaltOrTwoOutput();
                if (amplifier.IsHalt())
                    break;
                Grid.Move((int)amplifier.Output[0], (int)amplifier.Output[1]);
                amplifier.Output.Clear();

            }

            Grid.PrintAroundCenterWithRadius(50);

            Console.WriteLine(Grid.GetPaintedCount());
        }

        public override void Start2()
        {
            throw new NotImplementedException();
        }

        class Grid
        {
            /// <summary>
            /// 0 - black
            /// 1 - white
            /// </summary>
            public int[,] Panels;

            public int[,] WasPainted;
            public int CurrentX { get; set; }
            public int CurrentY { get; set; }
            public Direction Direction { get; set; }
            public Grid(int size)
            {
                Panels = new int[size, size];
                WasPainted = new int[size, size];
                CurrentX = CurrentY = size / 2;
                Panels[CurrentY, CurrentX] = 1;
            }

            public void Move(int param1, int param2)
            {
                Panels[CurrentY, CurrentX] = param1;
                WasPainted[CurrentY, CurrentX] ++;

                if (param2 == 0) TurnLeft();
                else TurnRight();

                StepForward();
            }

            public int GetCurrentPanelColor()
            {
                return Panels[CurrentY, CurrentX];
            }

            public void TurnLeft()
            {
                if (Direction == Direction.Up) Direction = Direction.Left;
                else Direction--;
            }
            public void TurnRight()
            {
                if (Direction == Direction.Left) Direction = Direction.Up;
                else Direction++;
            }
            public void StepForward()
            {
                switch (Direction)
                {
                    case Direction.Down:
                        CurrentY++;
                        break;
                    case Direction.Up:
                        CurrentY--;
                        break;
                    case Direction.Right:
                        CurrentX++;
                        break;
                    case Direction.Left:
                        CurrentX--;
                        break;
                }
            }

            public int GetPaintedCount()
            {
                int count = 0;
                foreach(var panel in WasPainted)
                {
                    if (panel > 0) count++;
                }
                return count;
            }
            
            public void PrintAroundCenterWithRadius(int radius = 10)
            {
                for (int i = Panels.GetLength(0)/2 - radius; i <= Panels.GetLength(0) / 2 + radius; i++)
                {
                    for (int j = Panels.GetLength(0) / 2 - radius; j <= Panels.GetLength(0) / 2 + radius; j++)
                    {
                        Console.Write(Panels[i, j]==1?"#":" ");
                    }
                    Console.WriteLine();
                }
                Console.WriteLine();
            }
        }
        enum Direction
        {
            Up = 0,
            Right = 1,
            Down = 2,
            Left = 3,
        }
    }
}

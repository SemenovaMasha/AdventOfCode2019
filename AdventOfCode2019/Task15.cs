using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode2019
{
    class Task15 : TaskI
    {
        public override int TaskNumber => 15;

        public override void Start1()
        {
            var filename = GetFileName1();
            var amplifier = new Amplifier()
            {
                IntCode = GetLongList_BySeparator(filename, ","),
            };

            var Grid = new Grid(10_000);

            int prevMove = 1;
            while (true)
            {
                //Grid.Print(30);

                //ConsoleKeyInfo _Key = Console.ReadKey();
                //switch (_Key.Key)
                //{
                //    case ConsoleKey.UpArrow:
                //        amplifier.IO.AddLast(1);
                //        move = 1;
                //        break;
                //    case ConsoleKey.DownArrow:
                //        amplifier.IO.AddLast(2);
                //        move = 2;
                //        break;
                //    case ConsoleKey.LeftArrow:
                //        amplifier.IO.AddLast(3);
                //        move = 3;
                //        break;
                //    case ConsoleKey.RightArrow:
                //        amplifier.IO.AddLast(4);
                //        move = 4;
                //        break;
                //    default:
                //        amplifier.IO.AddLast(0);
                //        break;
                //}

                int move = Grid.GetNextMoveByPrev_RightRule(prevMove);
                amplifier.IO.AddLast(move);

                amplifier.WorkUntilHaltOrWaitForInput();
                if (amplifier.IsHalt())
                    break;

                if((int)amplifier.Output.FindLast(x => true) != 0)
                {
                    prevMove = move;
                }


                Grid.SetLocationAndMove((int)amplifier.Output.FindLast(x => true), move);
            }


        }
        static bool Back = false;
        public override void Start2()
        {

            var filename = GetFileName1();
            var amplifier = new Amplifier()
            {
                IntCode = GetLongList_BySeparator(filename, ","),
            };

            var Grid = new Grid(10_000);

            int prevMove = 1;
            while (true)
            {
                //Grid.Print(30);


                int move = Grid.GetNextMoveByPrev_RightRule(prevMove);
                amplifier.IO.AddLast(move);

                amplifier.WorkUntilHaltOrWaitForInput();
                if (amplifier.IsHalt())
                    break;

                if ((int)amplifier.Output.FindLast(x => true) != 0)
                {
                    prevMove = move;
                }

                if ((int)amplifier.Output.FindLast(x => true) == 2)
                {
                    Grid.Locations[Grid.CurrentY, Grid.CurrentX] = 2;
                }

                Grid.SetLocationAndMove((int)amplifier.Output.FindLast(x => true), move);
                if (Grid.IsAtStart())
                {
                    break;
                }
            }

            Grid.Print(30);


            Grid.FillOxygen();

        }

        class Grid
        {
            /// <summary>
            /// 0 - undefined
            /// 1 - empty
            /// 2 - wall
            /// 3 - oxygen
            /// 4 - backRoad
            /// 6 - oxygen
            /// </summary>
            public int[,] Locations;
            public int CurrentX { get; set; }
            public int CurrentY { get; set; }
            public int StartX { get; set; }
            public int StartY { get; set; }
            List<PointWithOxy> OxygenPoints = new List<PointWithOxy>();
            public Grid(int size)
            {
                Locations = new int[size, size];
                CurrentX = CurrentY = StartX = StartY = size / 2;
                Locations[CurrentY, CurrentX] = 1;
            }
            public bool IsAtStart()
            {
                return CurrentX == StartX && CurrentY == StartY;
            }
            public void Print(int radius = 10)
            {
                for (int i = StartY - radius; i < StartY + radius; i++)
                {
                    for (int j = StartX - radius; j < StartX + radius; j++)
                    {
                        //if (CurrentX == j && CurrentY == i)
                        //{
                        //    Console.Write("D");
                        //    continue;
                        //}
                        //if(StartY==j && StartX == i)
                        //{
                        //    Console.Write("S");
                        //    continue;
                        //}
                        switch (Locations[i, j])
                        {
                            case 0:
                                Console.Write("?");
                                break;
                            case 1:
                                Console.Write(".");
                                break;
                            case 2:
                                Console.Write("#");
                                break;
                            case 3:
                                Console.Write("*");
                                break;
                            case 4:
                                Console.Write("4");
                                break;
                            case 5:
                                Console.Write("5");
                                break;
                            case 6:
                                Console.Write("O");
                                break;
                        }
                    }
                    Console.WriteLine();
                }
                Console.WriteLine();
            }
            public Point GetOxygenPoint(int radius = 100)
            {
                for (int i = StartY - radius; i < StartY + radius; i++)
                {
                    for (int j = StartX - radius; j < StartX + radius; j++)
                    {
                        if (Locations[i, j]==3)
                        {
                            return new Point(j, i);
                        }
                    }
                }
                return null;
            }
            public int GetRoadCount(int radius = 100, int value = 4)
            {
                int count = 0;
                for (int i = StartY - radius; i < StartY + radius; i++)
                {
                    for (int j = StartX - radius; j < StartX + radius; j++)
                    {
                        if (Locations[i, j] == value)
                            count++;
                    }
                }
                return count;
            }
            public void Update45(int radius = 100)
            {
                for (int i = StartY - radius; i < StartY + radius; i++)
                {
                    for (int j = StartX - radius; j < StartX + radius; j++)
                    {
                        if (Locations[i, j] == 4 || Locations[i, j] == 5)
                            Locations[i, j] = 1;
                    }
                }
            }
            long steps = 0;
            public void SetLocationAndMove(int response, int prevMove)
            {
                if(CurrentX==StartX && CurrentY == StartY)
                {
                    Console.WriteLine(GetRoadCount());
                    Console.WriteLine(GetRoadCount(5));
                    steps = 0;
                }
                //Print(30);
                switch (response)
                {
                    case 0:
                        SetValue(prevMove, 2);
                        break;
                    case 1:
                        steps ++;
                        Move(prevMove);
                        if (!Back)
                        {
                        SetValue(prevMove, 1,true);

                        }
                        else
                        {
                            SetValue(prevMove, 4, true);
                        }
                        break;
                    case 2:
                        steps=0;
                        Back = true;
                        Print(30);
                        Move(prevMove);
                        SetValue(prevMove, 3, true);
                        break;
                }
            }
            private void SetValue (int move,int value ,bool alreadyMoved=false)
            {
                if (alreadyMoved)
                {
                    if (value == 4 && Locations[CurrentY , CurrentX] == 4)
                        value = 5;
                    Locations[CurrentY, CurrentX ] = value;
                    return;
                }
                switch (move)
                {
                    case 1:
                        Locations[CurrentY - 1, CurrentX] = value;
                        break;
                    case 2:
                        Locations[CurrentY + 1, CurrentX] = value;
                        break;
                    case 3:
                        Locations[CurrentY, CurrentX - 1] = value;
                        break;
                    case 4:
                        Locations[CurrentY, CurrentX + 1] = value;
                        break;
                }
            }

            private void Move(int move)
            {
                switch (move)
                {
                    case 1:
                        CurrentY--;
                        break;
                    case 2:
                        CurrentY++;
                        break;
                    case 3:
                        CurrentX--;
                        break;
                    case 4:
                        CurrentX++;
                        break;
                }
            }

            public int GetNextMoveByPrev_RightRule(int prevMove)
            {
                switch (prevMove)
                {
                    case 1:
                        if (GetValueIfMove(4) != 2) return 4;
                        if (GetValueIfMove(1) != 2) return 1;
                        if (GetValueIfMove(3) != 2) return 3;
                        return 2;
                    case 2:
                        if (GetValueIfMove(3) != 2) return 3;
                        if (GetValueIfMove(2) != 2) return 2;
                        if (GetValueIfMove(4) != 2) return 4;
                        return 1;
                    case 3:
                        if (GetValueIfMove(1) != 2) return 1;
                        if (GetValueIfMove(3) != 2) return 3;
                        if (GetValueIfMove(2) != 2) return 2;
                        return 4;
                    case 4:
                        if (GetValueIfMove(2) != 2) return 2;
                        if (GetValueIfMove(4) != 2) return 4;
                        if (GetValueIfMove(1) != 2) return 1;
                        return 3;
                }


                return 0;
            }
            public int GetRightMove(int prevMove)
            {
                if (prevMove == 1) return 4;
                if (prevMove == 2) return 3;
                if (prevMove == 3) return 1;
                if (prevMove == 4) return 2;
                return 0;
            }

            private int GetValueIfMove(int move)
            {
                switch (move)
                {
                    case 1:
                        return Locations[CurrentY - 1, CurrentX];
                    case 2:
                        return Locations[CurrentY + 1, CurrentX];
                    case 3:
                        return Locations[CurrentY, CurrentX - 1];
                    case 4:
                        return Locations[CurrentY, CurrentX + 1];
                }
                return 0;
            }

            List<PointWithOxy> newList;
            public void FillOxygen()
            {
                Update45();
                var ox = GetOxygenPoint();
                OxygenPoints.Add(new PointWithOxy(ox.X, ox.Y));
                int minutes = 0;
                Locations[ox.Y, ox.X] = 6;
                while (OxygenPoints.Count > 0)
                {
                    newList = new List<PointWithOxy>();
                    foreach(var point in OxygenPoints)
                    {
                        CheckAdjacent(point);
                    }
                    minutes++;
                    OxygenPoints = newList;
                    if (minutes % 1==0)
                    {
                        Print(30);
                    }
                }
            }
            private void CheckAdjacent(PointWithOxy point)
            {
                if (Locations[point.Y - 1, point.X] == 1)
                {
                    newList.Add(new PointWithOxy(point.X, point.Y - 1));
                    Locations[point.Y - 1, point.X] = 6;
                }
                if (Locations[point.Y , point.X- 1] == 1)
                {
                    newList.Add(new PointWithOxy(point.X - 1, point.Y));
                    Locations[point.Y , point.X- 1] = 6;
                }
                if (Locations[point.Y + 1, point.X] == 1)
                {
                    newList.Add(new PointWithOxy(point.X, point.Y + 1));
                    Locations[point.Y +1, point.X] = 6;
                }
                if (Locations[point.Y , point.X+ 1] == 1)
                {
                    newList.Add(new PointWithOxy(point.X + 1, point.Y));
                    Locations[point.Y , point.X + 1] = 6;
                }
            }
        }

        class PointWithOxy
        {
            public int X { get; set; }
            public int Y { get; set; }

            public PointWithOxy(int x, int y)
            {
                X = x;
                Y = y;
            }
        }
    }
}

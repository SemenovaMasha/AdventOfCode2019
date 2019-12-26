using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2019
{
    class Task20 : TaskI
    {
        public override int TaskNumber => 20;

        public override void Start1()
        {
            var filename = GetFileName1();
           
            int count1 = 0;

            for(int i = 0; i < 50; i++)
            {
                for (int j = 0; j <50; j++)
                {
                    var amplifier = new Amplifier()
                    {
                        IntCode = GetLongList_BySeparator(filename, ","),
                    };
                    amplifier.IO.AddLast(i);
                    amplifier.IO.AddLast(j);

                    amplifier.WorkUntilHaltOrWaitForInput();
                    if (amplifier.Output.Last() == 1)
                        count1++;
                }
            }
            Console.WriteLine(count1);
        }

        public override void Start2()
        {

            var filename = GetFileName1();

            //1250 101

            //7091148 high
            //7081149 high/
            //wrong 6991133
            int N = 1250, M = 1300;
            var grid = new Grid(N, M);

            int prevStart = 0;
            int prevEnd = 1;
            for (int i = 0; i < N; i++)
            {
                if (prevEnd < prevStart)
                    prevEnd = prevStart;
                bool started = false;
                for (int j = prevStart; j < M; j++)
                {
                    if (i < 10 && j > 10)
                        break;
                    var amplifier = new Amplifier()
                    {
                        IntCode = GetLongList_BySeparator(filename, ","),
                    };
                    if (grid.CheckPoint(amplifier, j, i) == 1)
                    {
                        if (!started)
                        {
                            prevStart = j;
                            for (int j1 = j; j1 <= prevEnd ; j1++)
                            {
                                grid.Array[i, j1] = 1;
                            }
                            j = prevEnd - 1;//???
                        }
                        started = true;
                    }
                    else
                    {
                        if (started)
                        {
                            prevEnd = j;
                            break;
                        }
                    }
                }
                //grid.Print(30);

                if(grid.GetMaxSquareFromLast()>=100)
                {
                    grid.Print(100);
                    int firstJ = grid.GetFirst1InLastFilledLine();
                    Console.WriteLine((firstJ-1)*10000 + (i- 100));
                }
                //if (i %10==0)
                //{
                //    grid.Print(50);
                //    Console.WriteLine(grid.GetMaxSquareFromLast());

                //}
            }

            grid.Print(30);
            Console.WriteLine(grid.GetLastWidth());
            Console.WriteLine(grid.GetLastHeight());
            Console.WriteLine(grid.GetMaxSquare());
        }

        class Grid
        {
            public int[,] Array;
            public Grid(int n,int m)
            {
                Array = new int[n, m];
            }

            public int CheckPoint(Amplifier amplifier, int x, int y)
            {
                amplifier.IO.AddLast(x);
                amplifier.IO.AddLast(y);

                amplifier.WorkUntilHaltOrWaitForInput();

                Array[y,x] = (int)amplifier.Output.Last();
                return Array[y, x];
            }
            public void Print(int radius = 20)
            {
                for (int i = 0; i < radius; i++)
                {
                    for (int j = 0; j < radius; j++)
                    {
                        if (Array[i, j] == 1)
                            Console.Write("#");
                        else
                            Console.Write(".");
                    }
                    Console.WriteLine();
                }
                Console.WriteLine();
            }

            public int GetLastWidth()
            {
                int res = 0;
                for (int j = 0; j < Array.GetLength(1); j++)
                    if (Array[Array.GetLength(0) - 1, j] == 1)
                        res++;
                return res;
            }
            public int GetLastHeight()
            {
                int res = 0;
                for (int i = 0; i < Array.GetLength(0); i++)
                    if (Array[i, Array.GetLength(1) - 1] == 1)
                        res++;
                return res;
            }
            public int GetMaxSquare()
            {
                int firstJ = 0;
                int lastJ = 0;
                int lastI = Array.GetLength(0);
                for (int j = 0; j < Array.GetLength(1); j++)
                    if (Array[Array.GetLength(0) - 1, j] == 1)
                    {
                        firstJ = j;
                        break;
                    }
                for (int j = firstJ; j < Array.GetLength(1); j++)
                    if (Array[Array.GetLength(0) - 1, j] != 1)
                    {
                        lastJ = j-1;
                        break;
                    }
                int maxSize = lastJ - firstJ;
                int mbSize = maxSize;
                for (; mbSize > 0; mbSize--)
                {
                    if (Array[lastI - mbSize + 1, firstJ + mbSize - 1] == 1)
                        return mbSize;
                }

                return 0;
            }
            public int GetMaxSquareFromLast()
            {
                int firstJ = 0;
                int lastJ = 0;
                int lastI = -1;

                for (int i = Array.GetLength(0) - 1; i >= 0; i--)
                {
                    for (int j = 0; j < Array.GetLength(1); j++)
                    {
                        if (Array[i, j] == 1)
                        {
                            lastI = i;
                            break;
                        }
                    }
                    if (lastI != -1)
                        break;
                }

                for (int j = 0; j < Array.GetLength(1); j++)
                    if (Array[lastI , j] == 1)
                    {
                        firstJ = j;
                        break;
                    }
                for (int j = firstJ; j < Array.GetLength(1); j++)
                    if (Array[lastI, j] != 1)
                    {
                        lastJ = j - 1;
                        break;
                    }
                int maxSize = lastJ - firstJ;
                int mbSize = maxSize;
                int resMax = 0;
                for (; mbSize > 0; mbSize--)
                {
                    if (Array[lastI - mbSize + 1, firstJ + mbSize - 1] == 1)
                    {
                        resMax = mbSize;
                        break;
                    }
                }

                return resMax;
            }
            public int GetMaxSquareFromLine(int lastI)
            {
                int firstJ = 0;
                int lastJ = 0;

                for (int j = 0; j < Array.GetLength(1); j++)
                    if (Array[lastI, j] == 1)
                    {
                        firstJ = j;
                        break;
                    }
                for (int j = firstJ; j < Array.GetLength(1); j++)
                    if (Array[lastI, j] != 1)
                    {
                        lastJ = j - 1;
                        break;
                    }
                int maxSize = lastJ - firstJ;
                int mbSize = maxSize;
                for (; mbSize > 0; mbSize--)
                {
                    if (Array[lastI - mbSize + 1, firstJ + mbSize - 1] == 1)
                        return mbSize;
                }

                return 0;
            }
            public int GetFirst1InLastFilledLine()
            {
                int lastI = -1;

                for (int i = Array.GetLength(0) - 1; i >= 0; i--)
                {
                    for (int j = 0; j < Array.GetLength(1); j++)
                    {
                        if (Array[i, j] == 1)
                        {
                            lastI = i;
                            break;
                        }
                    }
                    if (lastI != -1)
                        break;
                }
                for (int j = 0; j < Array.GetLength(1); j++)
                    if (Array[lastI - 1, j] == 1)
                    {
                        return j;
                    }
                return 0;
            }
        }
    }
}

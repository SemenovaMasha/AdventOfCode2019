using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;

namespace AdventOfCode2019
{
    class Task3 : TaskI
    {
        int[,] Grid;
        int startX;
        int startY;
        int currentX;
        int currentY;
        int minDistanceToCross;
        Point startPoint;
        
        int minX = 100_000; //6408
        int minY = 100_000; //8068
        int maxX = -100_000; //21944
        int maxY = -100_000; //21334

        public override void Start1()
        {
            var filename = GetFileName1();
            var list = GetStringList_ByLine(filename);

            var firstWireMoves = GetMovesFromString(list[0]);
            var secondWireMoves = GetMovesFromString(list[1]);

            int size = 16_000;

            /*
             * 0-empty
             * 1-firstWire
             * 2-secondWire
             * 3-both
             * 4-startPoint
             */
            Grid = new int[size, size];

            startX = size / 2;
            startY = size / 2;
            startPoint = new Point(startX, startY);

            Grid[GetWithDeltaX(startY), GetWithDeltaX(startX)] = 4;

            minDistanceToCross = size * 2;

            FillGrid(firstWireMoves, 1);
            //PrintAroundStartWithRadius();

            FillGrid(secondWireMoves, 2);
            //PrintAroundStartWithRadius();

            Console.WriteLine(minDistanceToCross);
        }

        private int GetWithDeltaX(int start)
        {
            return start +600 ;
        }
        private int GetWithDeltaY(int start)
        {
            return start ;
        }

        private void PrintAroundStartWithRadius(int radius = 10)
        {
            for(int i = startY - radius; i <= startY + radius; i++)
            {
                for(int j = startX - radius; j <= startX + radius; j++)
                {
                    Console.Write(Grid[GetWithDeltaX(i), GetWithDeltaX(j)].ToString().PadLeft(3));
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        private void FillGrid(List<Move> moves, int wireNumber)
        {
            currentX = startX;
            currentY = startY;
            foreach (var move in moves)
            {
                var points = GetPointsFromMove(move);

                foreach(var point in points)
                {
                    int pointValue = Grid[GetWithDeltaX(point.Y), GetWithDeltaX(point.X)];
                    switch (pointValue)
                    {
                        case 4:
                            break;
                        case 0:
                            Grid[GetWithDeltaX(point.Y), GetWithDeltaX(point.X)] = wireNumber;
                            break;
                        case 1:
                        case 2:
                            if (pointValue != wireNumber)
                            {
                                Grid[GetWithDeltaX(point.Y), GetWithDeltaX(point.X)] = 3;
                                var dis = GetDistanceBetweenPoints(new Point(point.X, point.Y), startPoint);
                                if (dis < minDistanceToCross)
                                    minDistanceToCross = dis;
                            }
                            break;
                    }
                }
                ChangeCurrent(move);
            }
        }

        private int GetDistanceBetweenPoints(Point one , Point two)
        {
            return Math.Abs(one.X - two.X) + Math.Abs(one.Y - two.Y);
        }

        private List<Point> GetPointsFromMove(Move move)
        {
            List<Point> points = new List<Point>();
            switch (move.Direction)
            {
                case "R":
                    for (int i = currentX + 1; i <= currentX + move.Distance; i++)
                        points.Add(new Point(i, currentY));
                    break;
                case "L":
                    for (int i = currentX - 1; i >= currentX - move.Distance; i--)
                        points.Add(new Point(i, currentY));
                    break;
                case "D":
                    for (int i = currentY + 1; i <= currentY + move.Distance; i++)
                        points.Add(new Point(currentX, i));
                    break;
                case "U":
                    for (int i = currentY - 1; i >= currentY - move.Distance; i--)
                        points.Add(new Point(currentX, i));
                    break;
            }
            return points;
        }

        private void ChangeCurrent(Move move)
        {
            switch (move.Direction)
            {
                case "R":
                    currentX += move.Distance;
                    break;
                case "L":
                    currentX -= move.Distance;
                    break;
                case "D":
                    currentY += move.Distance;
                    break;
                case "U":
                    currentY -= move.Distance;
                    break;
            }

            if (currentX < minX)
                minX = currentX;
            if (currentY < minY)
                minY = currentY;
            if (currentX > maxX)
                maxX = currentX;
            if (currentY >maxY)
                maxY = currentY;
        }

        private List<Move> GetMovesFromString(string s)
        {
            var wireString = GetStringList_FromString_BySeparator(s, ",");

            return wireString.Select(x => new Move()
            {
                Direction = x.Substring(0, 1),
                Distance = int.Parse(x.Substring(1))
            }).ToList();
        }

        public override void Start2()
        {
            var filename = GetFileName1();
            var list = GetStringList_ByLine(filename);

            var firstWireMoves = GetMovesFromString(list[0]);
            var secondWireMoves = GetMovesFromString(list[1]);

            int size = 16_000;

            /*
             * 0-empty
             * +N - N step for 1 wire
             * -N - N step for 2 wire
             * 0-startPoint
             */
            Grid = new int[size, size];

            startX = size / 2;
            startY = size / 2;
            startPoint = new Point(startX, startY);

            Grid[GetWithDeltaX(startY), GetWithDeltaX(startX)] = 0;

            minStepDistanceToCross = int.MaxValue;

            FillGrid2(firstWireMoves, 1);
            PrintAroundStartWithRadius();

            FillGrid2(secondWireMoves, 2);
            PrintAroundStartWithRadius();

            Console.WriteLine(minStepDistanceToCross);
        }

        int minStepDistanceToCross = int.MaxValue;
        private void FillGrid2(List<Move> moves, int wireNumber)
        {
            currentX = startX;
            currentY = startY;
            bool positive = wireNumber == 1;
            int currentStep = 1;
            foreach (var move in moves)
            {
                var points = GetPointsFromMove(move);

                foreach (var point in points)
                {
                    int pointValue = Grid[GetWithDeltaX(point.Y), GetWithDeltaX(point.X)];
                    switch (pointValue)
                    {
                        case 4:
                            break;
                        case 0:
                            Grid[GetWithDeltaX(point.Y), GetWithDeltaX(point.X)] = positive?currentStep:-currentStep;
                            break;
                        default:
                            if((positive && pointValue < 0 )|| (!positive && pointValue> 0))
                            {
                                var bothStepsCount = Math.Abs(pointValue) + Math.Abs(currentStep);
                                if (bothStepsCount < minStepDistanceToCross)
                                    minStepDistanceToCross = bothStepsCount;
                            }
                            break;
                    }
                    currentStep++;
                }
                ChangeCurrent(move);

                if (currentStep > minStepDistanceToCross)
                    break;
            }
        }


        public override int TaskNumber => 3;

        class Move
        {
            public string Direction { get; set; }

            public int Distance { get; set; }
        }

    }
    public class Point
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}

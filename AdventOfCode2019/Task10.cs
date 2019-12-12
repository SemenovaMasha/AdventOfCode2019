using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2019
{
    class Task10 : TaskI
    {
        public override int TaskNumber => 10;

        public override void Start1()
        {
            var lines = GetStringList_ByLine(GetFileName1());
            var asteroids = new List<Asteroid>();

            for(int y=0;y<lines.Count;y++)
            {
                for(int x = 0; x < lines[0].Length; x++)
                {
                    if (lines[y][x] == '#')
                    {
                        asteroids.Add(new Asteroid(x, y));
                    }
                }
            }

            foreach(var asteroid in asteroids)
            {
                foreach (var asteroid2 in asteroids.OrderBy(x=>x.GetDistanceTo(asteroid)))
                {
                    if (asteroid != asteroid2)
                    {
                        asteroid.CheckAsteroid(asteroid2);
                    }
                }
            }
                       
            Console.WriteLine(asteroids.Max(x=>x.DetectedAsteroids.Count));
        }
        public override void Start2()
        {
            var lines = GetStringList_ByLine(GetFileName1());
            var asteroids = new List<Asteroid>();

            for (int y = 0; y < lines.Count; y++)
            {
                for (int x = 0; x < lines[0].Length; x++)
                {
                    if (lines[y][x] == '#')
                    {
                        asteroids.Add(new Asteroid(x, y));
                    }
                }
            }

            foreach (var asteroid in asteroids)
            {
                foreach (var asteroid2 in asteroids.OrderBy(x => x.GetDistanceTo(asteroid)))
                {
                    if (asteroid != asteroid2)
                    {
                        asteroid.CheckAsteroid(asteroid2);
                    }
                }
            }

            var BestStation = asteroids.OrderByDescending(x => x.DetectedAsteroids.Count).FirstOrDefault();

            //var vaporizedCount = 0;
            //var cycles = 0;

            //while (vaporizedCount < 200 && asteroids.Count > 1)
            //{
            //    foreach (var vap in BestStation.DetectedAsteroids)
            //    {
            //        asteroids.Remove(vap);
            //        vaporizedCount++;
            //    }
            //    BestStation.DetectedAsteroids.Clear();

            //    foreach (var asteroid2 in asteroids.OrderBy(x => x.GetDistanceTo(BestStation)))
            //    {
            //        if (BestStation != asteroid2)
            //        {
            //            BestStation.CheckAsteroid(asteroid2);
            //        }
            //    }
            //    cycles++;
            //}

            var listToDestroy = BestStation.DetectedAsteroids.OrderBy(x => BestStation.GetAngleFromTopTo(x)).ToList();

            Console.WriteLine(listToDestroy[199].X*100+ listToDestroy[199].Y);
        }

        public double GetAngleFromTopTo(int x1,int y1,int x2,int y2)
        {
            float xDiff = x2 - x1;
            float yDiff = y2 - y1;
            yDiff = -yDiff;
            var tmp = 90 - Math.Atan2(yDiff, xDiff)*180/Math.PI;
            if (xDiff < 0 && yDiff > 0) tmp += 360;

            return tmp;
        }

        class Asteroid
        {
            public int X { get; set; }
            public int Y { get; set; }

            public List<Asteroid> DetectedAsteroids = new List<Asteroid>();
            
            public Asteroid(int x, int y)
            {
                X = x;
                Y = y;
            }

            public int GetDistanceTo(Asteroid asteroid)
            {
                return Math.Abs(asteroid.X - this.X) + Math.Abs(asteroid.Y - this.Y);
            }

            public bool CanSee(Asteroid asteroid)
            {
                foreach(var detected in DetectedAsteroids)
                {
                    if (new Fraction(asteroid.X - this.X, asteroid.Y - this.Y) == new Fraction(detected.X - this.X, detected.Y - this.Y))
                        return false;
                }
                return true;
            }

            public void CheckAsteroid(Asteroid asteroid)
            {
                if (CanSee(asteroid))
                    DetectedAsteroids.Add(asteroid);
            }

            public override string ToString()
            {
                return X + " " + Y;
            }

            public double GetAngleFromTopTo(Asteroid asteroid)
            {
                float xDiff = asteroid.X - this.X;
                float yDiff = asteroid.Y - this.Y;
                yDiff = -yDiff;
                var tmp = 90 - Math.Atan2(yDiff, xDiff) * 180 / Math.PI;
                if (xDiff < 0 && yDiff > 0) tmp += 360;
                return tmp;
            }
        }

        class Fraction
        {
            public int numerator;
            public int denominator;
            public Fraction(int numerator, int denominator)
            {
                this.numerator = numerator;
                this.denominator = denominator;
            }
            public static bool operator ==(Fraction fraction1, Fraction fraction2)
            {
                if (object.ReferenceEquals(fraction1, null))
                {
                    return object.ReferenceEquals(fraction2, null);
                }

                if((fraction1.numerator ^ fraction2.numerator) < 0 || (fraction1.denominator ^ fraction2.denominator) < 0) return false;

                if (fraction1.numerator == fraction2.numerator && 0 == fraction2.numerator) return true;
                if (fraction1.denominator == fraction2.denominator && 0 == fraction2.denominator) return true;

                ToReduce(fraction1);
                ToReduce(fraction2);
                return fraction1.numerator == fraction2.numerator && fraction1.denominator == fraction2.denominator;
            }
            public static bool operator !=(Fraction fraction1, Fraction fraction2)
            {
                throw new Exception();
            }

            static Fraction ToReduce(Fraction fraction)
            {
                fraction.numerator = Math.Abs(fraction.numerator);
                fraction.denominator = Math.Abs(fraction.denominator);
                int nod = Nod(fraction.numerator, fraction.denominator);
                if (nod != 0)
                {
                    fraction.numerator /= nod;
                    fraction.denominator /= nod;
                }
                return fraction;
            }
            static int Nod(int n, int d)
            {
                int temp;
                n = Math.Abs(n);
                d = Math.Abs(d);
                while (d != 0 && n != 0)
                {
                    if (n % d > 0)
                    {
                        temp = n;
                        n = d;
                        d = temp % d;
                    }
                    else break;
                }
                if (d != 0 && n != 0) return d;
                else return 0;
            }
        }
    }
}

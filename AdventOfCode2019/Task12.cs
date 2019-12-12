using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2019
{
    class Task12 : TaskI
    {
        public override int TaskNumber => 12;

        public override void Start1()
        {
            var lines = GetStringList_ByLine(GetFileName1());

            var moons = new List<Moon>();
            var start = new List<Moon>();
            foreach (var line in lines)
            {
                var split1 = line.Replace("<x=", "").Replace(", y", "").Replace(", z", "").Replace(">", "").Split("=");
                moons.Add(new Moon(long.Parse(split1[0]), long.Parse(split1[1]), long.Parse(split1[2])));
                start.Add(new Moon(long.Parse(split1[0]), long.Parse(split1[1]), long.Parse(split1[2])));
            }
            for (long step = 0; step < 1000; step++)
            {
                Step(moons);
                //PrintMoons(moons);
            }

            Console.WriteLine(moons.Sum(x=>x.GetEnergy()));
        }
        private void Step(List<Moon> moons)
        {
            for (int i = 0; i < moons.Count; i++)
            {
                for (int j = i + 1; j < moons.Count; j++)
                {
                    ApplyGravity(moons[i], moons[j]);
                }
            }
            ApplyVelocity(moons);
        }
        private void ApplyGravity(Moon moon1, Moon moon2)
        {
            var xDiff = moon1.Position.X - moon2.Position.X;
            if(xDiff!=0) xDiff /= Math.Abs(xDiff);
            var yDiff = moon1.Position.Y - moon2.Position.Y;
            if (yDiff != 0) yDiff /= Math.Abs(yDiff);
            var zDiff = moon1.Position.Z - moon2.Position.Z;
            if (zDiff != 0) zDiff /= Math.Abs(zDiff);

            moon1.Velocity.X -= xDiff;
            moon2.Velocity.X += xDiff;

            moon1.Velocity.Y -= yDiff;
            moon2.Velocity.Y += yDiff;

            moon1.Velocity.Z -= zDiff;
            moon2.Velocity.Z += zDiff;
        }

        private void PrintMoons(List<Moon> moons)
        {
            foreach (var moon in moons)
            {
                Console.WriteLine(moon.ToString());
            }
            Console.WriteLine();
        }
        private void ApplyVelocity(List<Moon> moons)
        {
            foreach (var moon in moons)
                moon.ApplyVelocity();
        }
        public override void Start2()
        {
            var lines = GetStringList_ByLine(GetFileName1());

            var moons = new List<Moon>();
            var start = new List<Moon>();
            foreach (var line in lines)
            {
                var split1 = line.Replace("<x=", "").Replace(", y", "").Replace(", z", "").Replace(">", "").Split("=");
                moons.Add(new Moon(long.Parse(split1[0]), long.Parse(split1[1]), long.Parse(split1[2])));
                start.Add(new Moon(long.Parse(split1[0]), long.Parse(split1[1]), long.Parse(split1[2])));
            }

            long x_period = 0;
            long y_period = 0;
            long z_period = 0;

            long full_period = 0;
            for (long step = 0; step < long.MaxValue; step++)
            {
                Step(moons);
                if (moons[0].Position.X == start[0].Position.X && moons[1].Position.X == start[1].Position.X &&
                    moons[2].Position.X == start[2].Position.X && moons[3].Position.X == start[3].Position.X &&
                    moons[0].Velocity.X == 0 && moons[1].Velocity.X == 0 && moons[2].Velocity.X == 0 && moons[3].Velocity.X == 0)
                {
                    if (x_period == 0)
                        x_period = step + 1;
                }
                if (moons[0].Position.Y == start[0].Position.Y && moons[1].Position.Y == start[1].Position.Y &&
                    moons[2].Position.Y == start[2].Position.Y && moons[3].Position.Y == start[3].Position.Y &&
                    moons[0].Velocity.Y == 0 && moons[1].Velocity.Y == 0 && moons[2].Velocity.Y == 0 && moons[3].Velocity.Y == 0)
                {
                    if (y_period == 0)
                        y_period = step + 1;
                }
                if (moons[0].Position.Z == start[0].Position.Z && moons[1].Position.Z == start[1].Position.Z &&
                    moons[2].Position.Z == start[2].Position.Z && moons[3].Position.Z == start[3].Position.Z &&
                    moons[0].Velocity.Z == 0 && moons[1].Velocity.Z == 0 && moons[2].Velocity.Z == 0 && moons[3].Velocity.Z == 0)
                {
                    if (z_period == 0)
                        z_period = step + 1;
                }

                if (x_period != 0 && y_period != 0 && z_period != 0)
                {
                    full_period = x_period * y_period * z_period;
                    break;
                }
            }

            Console.WriteLine(NOK(NOK(x_period, y_period), z_period));
        }
        long NOD(long a, long b)
        {
            if (b < 0)
                b = -b;
            if (a < 0)
                a = -a;
            while (b > 0)
            {
                long temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }

        long NOK(long a,long b)
        {
            return a * b / NOD(a, b);
        }


        class Moon
        {
            public Vector Position { get; set; }
            public Vector Velocity { get; set; }
            public Moon(long x,long y,long z)
            {
                Position = new Vector(x, y, z);
                Velocity = new Vector(0, 0, 0);
            }
            public void ApplyVelocity()
            {
                Position.X += Velocity.X;
                Position.Y += Velocity.Y;
                Position.Z += Velocity.Z;
            }
            public override string ToString()
            {
                var space = 3;
                return $"pos=<x={Position.X.ToString().PadLeft(space)}, y={Position.Y.ToString().PadLeft(space)}, z={Position.Z.ToString().PadLeft(space)}>," +
                    $" vel=<x={Velocity.X.ToString().PadLeft(space)}, y={Velocity.Y.ToString().PadLeft(space)}, z={Velocity.Z.ToString().PadLeft(space)}>";
            }

            public long GetEnergy()
            {
                var pot = Math.Abs(Position.X) + Math.Abs(Position.Y) + Math.Abs(Position.Z);
                var kin = Math.Abs(Velocity.X) + Math.Abs(Velocity.Y) + Math.Abs(Velocity.Z);
                return pot * kin;
            }
        }
        class Vector
        {
            public long X { get; set; }
            public long Y { get; set; }
            public long Z { get; set; }
            public Vector(long x, long y, long z)
            {
                X = x;
                Y = y;
                Z = z;
            }
        }
    }
}

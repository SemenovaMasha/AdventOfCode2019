using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2019
{
    class Task16 : TaskI
    {
        public override int TaskNumber => 16;

        public  List<int> Pattern;
        int[] Signal;
        int[] NewSignal;
        public override void Start1()
        {
            var s = GetString(GetFileName1());
            s = String.Concat(Enumerable.Repeat(s, 10000));
            Signal = new int[s.Length];
            NewSignal = new int[s.Length];
            for (int i = 0; i < s.Length; i++)
            {
                Signal[i] = s[i] - '0';
            }

            for(int i = 0; i < 100; i++)
            {
                Phase();
            }

            Console.WriteLine(String.Join("",Signal.Take(8)));
        }

        private void Phase()
        {
            for (int i = 0; i < Signal.Length; i++)
            {
                Step(i);
            }

            Signal = NewSignal;
            NewSignal = new int[Signal.Length];
        }
        private void Step(int stepNum)
        {
            GeneratePattern(stepNum);

            NewSignal[stepNum] = GetDigit();
        }
        private void GeneratePattern(int stepNum)
        {
            stepNum++;
            int[] repeat = new int[] { 0, 1, 0, -1 };
            Pattern = new List<int>();

            while (Pattern.Count < Signal.Length + 1)
            {
                Pattern.AddRange(Enumerable.Repeat(repeat[0], stepNum).ToList());
                Pattern.AddRange(Enumerable.Repeat(repeat[1], stepNum).ToList());
                Pattern.AddRange(Enumerable.Repeat(repeat[2], stepNum).ToList());
                Pattern.AddRange(Enumerable.Repeat(repeat[3], stepNum).ToList());
            }
            Pattern.RemoveAt(0);
        }

        private int GetDigit()
        {
            int res = 0;
            for(int i = 0; i < Signal.Length; i++)
            {
                res += Signal[i] * Pattern[i];
            }
            return (Math.Abs(res))%10;
        }
        public override void Start2()
        {
            throw new NotImplementedException();
        }
    }
}

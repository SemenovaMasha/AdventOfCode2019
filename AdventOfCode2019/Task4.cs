using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2019
{
    class Task4 : TaskI
    {
        public override void Start1()
        {
            int beginRange = 134792;
            int endRange = 675810;

            int count = 0;

            for(int currentNumber = beginRange; currentNumber <= endRange; currentNumber++)
            {
                if (HasDouble(currentNumber) && !HasDecrease(currentNumber))
                {
                    count++;
                }
            }

            Console.WriteLine(count);
        }

        private bool HasDecrease(int number)
        {
            var digits = GetDigits(number).ToList();
            for (int i = 0; i < digits.Count - 1; i++)
            {
                if (digits[i] > digits[i + 1])
                    return true;
            }
            return false;
        }

        private bool HasDouble(int number)
        {
            var digits = GetDigits(number).ToList();
            for (int i = 0; i < digits.Count - 1; i++)
            {
                if (digits[i] == digits[i + 1])
                    return true;
            }
            return false;
        }

        private bool HasDoubleNotInLargerGroup(int number)
        {
            var digits = GetDigits(number).ToList();

            for (int i = 0; i < digits.Count - 1; i++)
            {
                if (digits[i] == digits[i + 1])
                {
                    if ((i == digits.Count - 2? true: digits[i + 2] != digits[i] )
                        &&(i == 0? true : digits[i - 1] != digits[i]))
                        return true;
                    
                }
            }
            return false;
        }

        public static IEnumerable<int> GetDigits(int source)
        {
            int individualFactor = 0;
            int tennerFactor = Convert.ToInt32(Math.Pow(10, source.ToString().Length));
            do
            {
                source -= tennerFactor * individualFactor;
                tennerFactor /= 10;
                individualFactor = source / tennerFactor;

                yield return individualFactor;
            } while (tennerFactor > 1);
        }

        public override void Start2()
        {
            int beginRange = 134792;
            int endRange = 675810;

            int count = 0;

            for (int currentNumber = beginRange; currentNumber <= endRange; currentNumber++)
            {
                if (!HasDecrease(currentNumber) && HasDoubleNotInLargerGroup(currentNumber))
                {
                    count++;
                }
            }

            Console.WriteLine(count);
        }

        public override int TaskNumber()
        {
            return 4;
        }
    }
}

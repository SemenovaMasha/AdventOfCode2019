using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode2019
{
    class Task1 : TaskI
    {
        public override void Start1()
        {
            string fileName = GetFileName1();

            var list = GetIntList_ByLine(fileName);

            var res = 0;
            foreach (var item in list)
            {
                res += (item/3) - 2;
            }
            Console.WriteLine(res);
        }

        public override void Start2()
        {
            string fileName = GetFileName1();

            var list = GetIntList_ByLine(fileName);

            var res = 0;
            while(list.Count>0)
            {
                var currentItem = list[0];
                int newVal = (currentItem / 3) - 2;

                if (newVal > 0)
                {
                    res += newVal;
                    list.Add(newVal);
                }
                list.RemoveAt(0);
            }
            Console.WriteLine(res);
        }

        public override int TaskNumber => 1;
    }
}

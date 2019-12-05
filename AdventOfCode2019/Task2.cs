using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace AdventOfCode2019
{
    class Task2 : TaskI
    {
        public override void Start1()
        {
            var filename = GetFileName1();
            var list = GetIntList_BySeparator(filename,",");

            list[1] = 12;
            list[2] = 2;

            var currentPosition = 0;

            while (list[currentPosition] != 99)
            {
                switch (list[currentPosition])
                {
                    case 1:
                        list[list[currentPosition + 3]] = list[list[currentPosition + 1]] + list[list[currentPosition + 2]];
                        break;
                    case 2:
                        list[list[currentPosition + 3]] = list[list[currentPosition + 1]] * list[list[currentPosition + 2]];
                        break;
                }

                //Console.WriteLine(String.Join(",",list));

                currentPosition += 4;
            }
            Console.WriteLine(list[0]);
        }

        public override void Start2()
        {
            var filename = GetFileName1();

            for (int noun = 0; noun < 100; noun++)
            {
                for (int verb = 0; verb < 100; verb++)
                {
                    var list = GetIntList_BySeparator(filename, ",");

                    list[1] = noun;
                    list[2] = verb;

                    var currentPosition = 0;

                    while (list[currentPosition] != 99)
                    {
                        switch (list[currentPosition])
                        {
                            case 1:
                                list[list[currentPosition + 3]] = list[list[currentPosition + 1]] + list[list[currentPosition + 2]];
                                break;
                            case 2:
                                list[list[currentPosition + 3]] = list[list[currentPosition + 1]] * list[list[currentPosition + 2]];
                                break;
                        }
                        

                        currentPosition += 4;
                    }

                    if (list[0] == 19690720)
                    {
                        Console.WriteLine(100*noun+verb);
                        return;

                    }
                }
            }
            Console.WriteLine("end");
        }

        public override int TaskNumber()
        {
            return 2;
        }
    }
}

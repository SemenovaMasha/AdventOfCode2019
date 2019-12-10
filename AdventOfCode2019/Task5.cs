using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace AdventOfCode2019
{
    class Task5 : TaskI
    {
        public override void Start1()
        {
            var filename = GetFileName1();
            var list = GetIntList_BySeparator(filename,",");

            //list[1] = 12;
            //list[2] = 2;

            var currentPosition = 0;
            var Input = new Queue<int>();
            var Output = new List<int>();
            var IO = new Queue<int>();
            IO.Enqueue(1);

            while (list[currentPosition] != 99)
            {
                int opcode = GetOpcodeFromInstruction(list[currentPosition].ToString());
                var modes = GetModes(list[currentPosition].ToString());
                int firstParam;
                int secondParam;
                switch (opcode)
                {
                    case 1:
                        firstParam = modes[0] == 0 ? list[list[currentPosition + 1]] : list[currentPosition + 1];
                        secondParam = modes[1] == 0 ? list[list[currentPosition + 2]] : list[currentPosition + 2];
                        list[list[currentPosition + 3]] = firstParam + secondParam;
                        currentPosition += 4;
                        break;
                    case 2:
                        firstParam = modes[0] == 0 ? list[list[currentPosition + 1]] : list[currentPosition + 1];
                        secondParam = modes[1] == 0 ? list[list[currentPosition + 2]] : list[currentPosition + 2];
                        list[list[currentPosition + 3]] = firstParam * secondParam;
                        currentPosition += 4;
                        break;
                    case 3: //save input
                        list[list[currentPosition + 1]] = IO.Dequeue();
                        currentPosition += 2;
                        break;
                    case 4: //output
                        IO.Enqueue(list[list[currentPosition + 1]]);
                        Output.Add(list[list[currentPosition + 1]]);
                        currentPosition += 2;
                        break;
                }

                //Console.WriteLine(String.Join(",",list));

            }
            Console.WriteLine(list[0]);
        }

        private int GetOpcodeFromInstruction(string instruction)
        {
            instruction = instruction.PadLeft(2, '0');
            return int.Parse(instruction.Substring(instruction.Length - 2));            
        }

        private List<int> GetModes(string instruction)
        {
            instruction = instruction.PadLeft(2, '0');
            instruction = instruction.Substring(0, instruction.Length - 2).PadLeft(5,'0');
            var list = new List<int>();

            for (int i = instruction.Length - 1; i >= 0; i--)
                list.Add(instruction[i] - '0');

            return list;
            
        }
        public override void Start2()
        {

            var filename = GetFileName1();
            var list = GetIntList_BySeparator(filename, ",");
            
            var currentPosition = 0;
            var Output = new List<int>();
            var IO = new Queue<int>();
            IO.Enqueue(5);

            while (list[currentPosition] != 99)
            {
                int opcode = GetOpcodeFromInstruction(list[currentPosition].ToString());
                var modes = GetModes(list[currentPosition].ToString());
                int firstParam;
                int secondParam;
                switch (opcode)
                {
                    case 1:
                        firstParam = modes[0] == 0 ? list[list[currentPosition + 1]] : list[currentPosition + 1];
                        secondParam = modes[1] == 0 ? list[list[currentPosition + 2]] : list[currentPosition + 2];
                        list[list[currentPosition + 3]] = firstParam + secondParam;
                        currentPosition += 4;
                        break;
                    case 2:
                        firstParam = modes[0] == 0 ? list[list[currentPosition + 1]] : list[currentPosition + 1];
                        secondParam = modes[1] == 0 ? list[list[currentPosition + 2]] : list[currentPosition + 2];
                        list[list[currentPosition + 3]] = firstParam * secondParam;
                        currentPosition += 4;
                        break;
                    case 3: //save input
                        list[list[currentPosition + 1]] = IO.Dequeue();
                        currentPosition += 2;
                        break;
                    case 4: //output
                        IO.Enqueue(list[list[currentPosition + 1]]);
                        Output.Add(list[list[currentPosition + 1]]);
                        currentPosition += 2;
                        break;
                    case 5:
                        firstParam = modes[0] == 0 ? list[list[currentPosition + 1]] : list[currentPosition + 1];
                        secondParam = modes[1] == 0 ? list[list[currentPosition + 2]] : list[currentPosition + 2];
                        if (firstParam != 0)
                        {
                            currentPosition = secondParam;
                        }
                        else
                        {
                            currentPosition += 3;
                        }
                        break;
                    case 6:
                        firstParam = modes[0] == 0 ? list[list[currentPosition + 1]] : list[currentPosition + 1];
                        secondParam = modes[1] == 0 ? list[list[currentPosition + 2]] : list[currentPosition + 2];
                        if (firstParam == 0)
                        {
                            currentPosition = secondParam;
                        }
                        else
                        {
                            currentPosition += 3;
                        }
                        break;
                    case 7:
                        firstParam = modes[0] == 0 ? list[list[currentPosition + 1]] : list[currentPosition + 1];
                        secondParam = modes[1] == 0 ? list[list[currentPosition + 2]] : list[currentPosition + 2];
                        if (firstParam < secondParam)
                        {
                            list[list[currentPosition + 3]] = 1;
                        }
                        else
                        {
                            list[list[currentPosition + 3]] = 0;
                        }
                        currentPosition += 4;
                        break;
                    case 8:
                        firstParam = modes[0] == 0 ? list[list[currentPosition + 1]] : list[currentPosition + 1];
                        secondParam = modes[1] == 0 ? list[list[currentPosition + 2]] : list[currentPosition + 2];
                        if (firstParam == secondParam)
                        {
                            list[list[currentPosition + 3]] = 1;
                        }
                        else
                        {
                            list[list[currentPosition + 3]] = 0;
                        }
                        currentPosition += 4;
                        break;
                }

                //Console.WriteLine(String.Join(",",list));

            }
            Console.WriteLine(list[0]);
        }

        public override int TaskNumber => 5;
    }
}

using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;

namespace AdventOfCode2019
{
    class Task7 : TaskI
    {
        private int GetOpcodeFromInstruction(string instruction)
        {
            instruction = instruction.PadLeft(2, '0');
            return int.Parse(instruction.Substring(instruction.Length - 2));
        }

        private List<int> GetModes(string instruction)
        {
            instruction = instruction.PadLeft(2, '0');
            instruction = instruction.Substring(0, instruction.Length - 2).PadLeft(5, '0');
            var list = new List<int>();

            for (int i = instruction.Length - 1; i >= 0; i--)
                list.Add(instruction[i] - '0');

            return list;

        }
        public override void Start1()
        {
            var filename = GetFileName1();
            var list = GetIntList_BySeparator(filename, ",");

            var maxOutput = 0;
            foreach(var permute in Permutations(new List<int>() { 0, 1, 2, 3, 4 }))
            {
                var Phases = permute;
                var currentPosition = 0;
                var Output = new List<int>();
                var IO = new LinkedList<int>();

                //var Phases = new List<int>() { 0, 1, 2, 3, 4 };
                IO.AddLast(0);

                foreach (var phase in Phases)
                {
                    IO.AddFirst(phase);
                    currentPosition = 0;

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
                                list[list[currentPosition + 1]] = IO.First.Value;
                                IO.RemoveFirst();
                                currentPosition += 2;
                                break;
                            case 4: //output
                                IO.AddLast(list[list[currentPosition + 1]]);
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
                }
                if(Output[Output.Count - 1] > maxOutput)
                {
                    maxOutput = Output[Output.Count - 1];
                }
            }
                Console.WriteLine(maxOutput);

        }
        private ICollection<ICollection<T>> Permutations<T>(ICollection<T> list)
        {
            var result = new List<ICollection<T>>();
            if (list.Count == 1)
            { // If only one possible permutation
                result.Add(list); // Add it and return it
                return result;
            }
            foreach (var element in list)
            { // For each element in that list
                var remainingList = new List<T>(list);
                remainingList.Remove(element); // Get a list containing everything except of chosen element
                foreach (var permutation in Permutations<T>(remainingList))
                { // Get all possible sub-permutations
                    permutation.Add(element); // Add that element
                    result.Add(permutation);
                }
            }
            return result;
        }

        public override void Start2()
        {
            var filename = GetFileName1();

            var maxOutput = 0;
            foreach (var permute in Permutations(new List<int>() { 5,6,7,8,9 }))
            {
                var phases = permute.ToList();

               // phases =new List<int> { 9,8,7,6,5 };

                var Amplifiers = new List<Amplifier>();
                
                foreach(var phase in phases)
                {
                    Amplifiers.Add(new Amplifier(phase)
                    {
                        IntCode = GetIntList_BySeparator(filename, ","),
                    });
                }

                Amplifiers[0].IO.AddLast(0);

                Amplifiers[0].Next = Amplifiers[1];
                Amplifiers[1].Next = Amplifiers[2];
                Amplifiers[2].Next = Amplifiers[3];
                Amplifiers[3].Next = Amplifiers[4];
                Amplifiers[4].Next = Amplifiers[0];

                int currentAmplifier = 0;
                while (!Amplifiers[4].IsHalt())
                {
                    Amplifiers[currentAmplifier].WorkUntilHaltOrWaitForInput();

                    currentAmplifier = NextPhase(currentAmplifier);
                }
                
                if (Amplifiers[4].Output[Amplifiers[4].Output.Count - 1] > maxOutput)
                {
                    maxOutput = Amplifiers[4].Output[Amplifiers[4].Output.Count - 1];
                }
            }
            Console.WriteLine(maxOutput);
        }

        private int NextPhase(int phase)
        {
            if (phase == 4) return 0;
            return ++phase;
        }

        public override int TaskNumber()
        {
            return 7;
        }

        class Amplifier
        {
            private int currentPosition { get; set; } = 0;
            public List<int> IntCode { get; set; } = new List<int>();
            public List<int> Output { get; set; } = new List<int>();
            public Amplifier Next { get; set; }

            public int Phase { get; set; }

            public LinkedList<int> IO { get; set; } = new LinkedList<int>();
            public Amplifier(int phase)
            {
                this.Phase = phase;
                IO.AddFirst(phase);
            }
            public void WorkUntilHaltOrWaitForInput()
            {
                while (IntCode[currentPosition] != 99)
                {
                    int opcode = GetOpcodeFromInstruction(IntCode[currentPosition].ToString());
                    var modes = GetModes(IntCode[currentPosition].ToString());
                    int firstParam;
                    int secondParam;
                    switch (opcode)
                    {
                        case 1:
                            firstParam = modes[0] == 0 ? IntCode[IntCode[currentPosition + 1]] : IntCode[currentPosition + 1];
                            secondParam = modes[1] == 0 ? IntCode[IntCode[currentPosition + 2]] : IntCode[currentPosition + 2];
                            IntCode[IntCode[currentPosition + 3]] = firstParam + secondParam;
                            currentPosition += 4;
                            break;
                        case 2:
                            firstParam = modes[0] == 0 ? IntCode[IntCode[currentPosition + 1]] : IntCode[currentPosition + 1];
                            secondParam = modes[1] == 0 ? IntCode[IntCode[currentPosition + 2]] : IntCode[currentPosition + 2];
                            IntCode[IntCode[currentPosition + 3]] = firstParam * secondParam;
                            currentPosition += 4;
                            break;
                        case 3: //save input
                            if (IO.First == null)
                                return;
                            IntCode[IntCode[currentPosition + 1]] = IO.First.Value;
                            IO.RemoveFirst();
                            currentPosition += 2;
                            break;
                        case 4: //output
                            Next.IO.AddLast(IntCode[IntCode[currentPosition + 1]]);
                            Output.Add(IntCode[IntCode[currentPosition + 1]]);
                            currentPosition += 2;
                            break;
                        case 5:
                            firstParam = modes[0] == 0 ? IntCode[IntCode[currentPosition + 1]] : IntCode[currentPosition + 1];
                            secondParam = modes[1] == 0 ? IntCode[IntCode[currentPosition + 2]] : IntCode[currentPosition + 2];
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
                            firstParam = modes[0] == 0 ? IntCode[IntCode[currentPosition + 1]] : IntCode[currentPosition + 1];
                            secondParam = modes[1] == 0 ? IntCode[IntCode[currentPosition + 2]] : IntCode[currentPosition + 2];
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
                            firstParam = modes[0] == 0 ? IntCode[IntCode[currentPosition + 1]] : IntCode[currentPosition + 1];
                            secondParam = modes[1] == 0 ? IntCode[IntCode[currentPosition + 2]] : IntCode[currentPosition + 2];
                            if (firstParam < secondParam)
                            {
                                IntCode[IntCode[currentPosition + 3]] = 1;
                            }
                            else
                            {
                                IntCode[IntCode[currentPosition + 3]] = 0;
                            }
                            currentPosition += 4;
                            break;
                        case 8:
                            firstParam = modes[0] == 0 ? IntCode[IntCode[currentPosition + 1]] : IntCode[currentPosition + 1];
                            secondParam = modes[1] == 0 ? IntCode[IntCode[currentPosition + 2]] : IntCode[currentPosition + 2];
                            if (firstParam == secondParam)
                            {
                                IntCode[IntCode[currentPosition + 3]] = 1;
                            }
                            else
                            {
                                IntCode[IntCode[currentPosition + 3]] = 0;
                            }
                            currentPosition += 4;
                            break;


                            //Console.WriteLine(String.Join(",",list));

                    }
                }

            }
                public bool IsHalt()
                {
                    return IntCode[currentPosition] == 99;
                }
            
            private int GetOpcodeFromInstruction(string instruction)
            {
                instruction = instruction.PadLeft(2, '0');
                return int.Parse(instruction.Substring(instruction.Length - 2));
            }

            private List<int> GetModes(string instruction)
            {
                instruction = instruction.PadLeft(2, '0');
                instruction = instruction.Substring(0, instruction.Length - 2).PadLeft(5, '0');
                var list = new List<int>();

                for (int i = instruction.Length - 1; i >= 0; i--)
                    list.Add(instruction[i] - '0');

                return list;

            }
        }
    }
}

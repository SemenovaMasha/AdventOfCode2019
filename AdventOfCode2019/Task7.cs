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
            var list = GetLongList_BySeparator(filename, ",");

            long maxOutput = 0;
            foreach(var permute in Permutations(new List<int>() { 0, 1, 2, 3, 4 }))
            {
                var Phases = permute;
                var currentPosition = 0;
                var Output = new List<long>();
                var IO = new LinkedList<long>();

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
                        long firstParam;
                        long secondParam;
                        switch (opcode)
                        {
                            case 1:
                                firstParam = modes[0] == 0 ? list[(int)(list[currentPosition + 1])] : list[currentPosition + 1];
                                secondParam = modes[1] == 0 ? list[(int)list[currentPosition + 2]] : list[currentPosition + 2];
                                list[(int)list[currentPosition + 3]] = firstParam + secondParam;
                                currentPosition += 4;
                                break;
                            case 2:
                                firstParam = modes[0] == 0 ? list[(int)list[currentPosition + 1]] : list[currentPosition + 1];
                                secondParam = modes[1] == 0 ? list[(int)list[currentPosition + 2]] : list[currentPosition + 2];
                                list[(int)list[currentPosition + 3]] = firstParam * secondParam;
                                currentPosition += 4;
                                break;
                            case 3: //save input
                                list[(int)list[currentPosition + 1]] = IO.First.Value;
                                IO.RemoveFirst();
                                currentPosition += 2;
                                break;
                            case 4: //output
                                IO.AddLast(list[(int)list[currentPosition + 1]]);
                                Output.Add(list[(int)list[currentPosition + 1]]);
                                currentPosition += 2;
                                break;
                            case 5:
                                firstParam = modes[0] == 0 ? list[(int)list[currentPosition + 1]] : list[currentPosition + 1];
                                secondParam = modes[1] == 0 ? list[(int)list[currentPosition + 2]] : list[currentPosition + 2];
                                if (firstParam != 0)
                                {
                                    currentPosition = (int)secondParam;
                                }
                                else
                                {
                                    currentPosition += 3;
                                }
                                break;
                            case 6:
                                firstParam = modes[0] == 0 ? list[(int)list[currentPosition + 1]] : list[currentPosition + 1];
                                secondParam = modes[1] == 0 ? list[(int)list[currentPosition + 2]] : list[currentPosition + 2];
                                if (firstParam == 0)
                                {
                                    currentPosition = (int)secondParam;
                                }
                                else
                                {
                                    currentPosition += 3;
                                }
                                break;
                            case 7:
                                firstParam = modes[0] == 0 ? list[(int)list[currentPosition + 1]] : list[currentPosition + 1];
                                secondParam = modes[1] == 0 ? list[(int)list[currentPosition + 2]] : list[currentPosition + 2];
                                if (firstParam < secondParam)
                                {
                                    list[(int)list[currentPosition + 3]] = 1;
                                }
                                else
                                {
                                    list[(int)list[currentPosition + 3]] = 0;
                                }
                                currentPosition += 4;
                                break;
                            case 8:
                                firstParam = modes[0] == 0 ? list[(int)list[currentPosition + 1]] : list[currentPosition + 1];
                                secondParam = modes[1] == 0 ? list[(int)list[currentPosition + 2]] : list[currentPosition + 2];
                                if (firstParam == secondParam)
                                {
                                    list[(int)list[currentPosition + 3]] = 1;
                                }
                                else
                                {
                                    list[(int)list[currentPosition + 3]] = 0;
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

            long maxOutput = 0;
            foreach (var permute in Permutations(new List<int>() { 5,6,7,8,9 }))
            {
                var phases = permute.ToList();

               // phases =new List<int> { 9,8,7,6,5 };

                var Amplifiers = new List<Amplifier>();
                
                foreach(var phase in phases)
                {
                    Amplifiers.Add(new Amplifier(phase)
                    {
                        IntCode = GetLongList_BySeparator(filename, ","),
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

        public override int TaskNumber => 7;

     }
    public class Amplifier
    {
        private int currentPosition { get; set; } = 0;
        public List<long> IntCode { get; set; } = new List<long>();
        public List<long> Output { get; set; } = new List<long>();
        public Amplifier Next { get; set; }

        public int Phase { get; set; }

        public int RelativeBase { get; set; } = 0;

        public LinkedList<long> IO { get; set; } = new LinkedList<long>();
        public Amplifier(int phase)
        {
            this.Phase = phase;
            IO.AddFirst(phase);
        }
        private int StepNumber { get; set; }
        public Amplifier()
        {
        }

        private void OneWork()
        {
                StepNumber++;
                long opcode = GetOpcodeFromInstruction(IntCode[currentPosition].ToString());
                var modes = GetModes(IntCode[currentPosition].ToString());
                long firstParam;
                long secondParam;
            switch (opcode)
            {
                case 1:
                    firstParam = GetValue(modes[0], 1);
                    secondParam = GetValue(modes[1], 2);
                    SetValue(modes[2], firstParam + secondParam, 3);
                    currentPosition += 4;
                    break;
                case 2:
                    firstParam = GetValue(modes[0], 1);
                    secondParam = GetValue(modes[1], 2);
                    SetValue(modes[2], firstParam * secondParam, 3);
                    currentPosition += 4;
                    break;
                case 3: //save input
                    if (IO.First == null)
                        return ;
                    SetValue(modes[0], IO.First.Value, 1);
                    IO.RemoveFirst();
                    currentPosition += 2;
                    break;
                case 4: //output
                        //IO.AddLast( GetValue(modes[0], 1));
                    Output.Add(GetValue(modes[0], 1));
                    currentPosition += 2;
                    break;
                case 5:
                    firstParam = GetValue(modes[0], 1);
                    secondParam = GetValue(modes[1], 2);
                    if (firstParam != 0)
                    {
                        currentPosition = (int)secondParam;
                    }
                    else
                    {
                        currentPosition += 3;
                    }
                    break;
                case 6:
                    firstParam = GetValue(modes[0], 1);
                    secondParam = GetValue(modes[1], 2);
                    if (firstParam == 0)
                    {
                        currentPosition = (int)secondParam;
                    }
                    else
                    {
                        currentPosition += 3;
                    }
                    break;
                case 7:
                    firstParam = GetValue(modes[0], 1);
                    secondParam = GetValue(modes[1], 2);
                    if (firstParam < secondParam)
                    {
                        SetValue(modes[2], 1, 3);
                    }
                    else
                    {
                        SetValue(modes[2], 0, 3);
                    }
                    currentPosition += 4;
                    break;
                case 8:
                    firstParam = GetValue(modes[0], 1);
                    secondParam = GetValue(modes[1], 2);
                    if (firstParam == secondParam)
                    {
                        SetValue(modes[2], 1, 3);
                    }
                    else
                    {
                        SetValue(modes[2], 0, 3);
                    }
                    currentPosition += 4;
                    break;
                case 9:
                    firstParam = GetValue(modes[0], 1);
                    RelativeBase += (int)firstParam;
                    currentPosition += 2;
                    break;

            }
        }

        public void WorkUntilHaltOrWaitForInput()
        {
            while (IntCode[currentPosition] != 99&& (IO.Count>0 || IntCode[currentPosition] != 3))
            //while (true)
            {
                OneWork();
            }

        }
        public void WorkUntilHaltOrTwoOutput()
        {
            while (IntCode[currentPosition] != 99 && Output.Count<2)
            {
                OneWork();
            }

        }

        public string GetLastOutput(int count =9)
        {
            string s = "";
            if (Output.Count >= count)
            for(int i = 0; i < count; i++)
            {
                s += Output[Output.Count - count + i]+" ";
            }
            return s;
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

        private long GetValue(int mode, int paramNumber = 1)
        {
            ExtentIfNeed(currentPosition + paramNumber);
            switch (mode){
                case 0:
                    ExtentIfNeed(IntCode[currentPosition + paramNumber]);
                    return IntCode[(int)IntCode[currentPosition + paramNumber]];
                case 1:
                    return IntCode[currentPosition + paramNumber];
                case 2:
                    ExtentIfNeed(RelativeBase + IntCode[currentPosition + paramNumber]);
                    return IntCode[(int)(RelativeBase + IntCode[currentPosition + paramNumber])];
            }
            throw new Exception();
        }


        private void SetValue(int mode, long value, int paramNumber = 1)
        {
            ExtentIfNeed(currentPosition + paramNumber);
            switch (mode)
            {
                case 0:
                    ExtentIfNeed(IntCode[currentPosition + paramNumber]);
                    IntCode[(int)IntCode[currentPosition + paramNumber]] = value;
                    return;
                case 2:
                    ExtentIfNeed(RelativeBase + IntCode[currentPosition + paramNumber]);
                    IntCode[(int)(RelativeBase + IntCode[currentPosition + paramNumber])] = value;
                    return;
            }
            throw new Exception();
        }

        private void ExtentIfNeed(long position)
        {
            if(IntCode.Count<= position)
            {
                IntCode.AddRange(Enumerable.Repeat((long)0, (int)position - IntCode.Count + 1).ToList());
            }
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

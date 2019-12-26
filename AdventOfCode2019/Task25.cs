using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AdventOfCode2019
{
    class Task25 : TaskI
    {
        public override int TaskNumber => 25;
        Amplifier amplifier;
        List<string> items;
        public override void Start1()
        {
            var filename = GetFileName1();
            amplifier =new Amplifier()
            {
                IntCode = GetLongList_BySeparator(filename, ","),
            };

            var commands = new string[]{"south","south","take fixed point","north","west","west","west","take hologram",
                "east","east","east","north","west","take antenna","west","take shell","east","south","take whirled peas",
                "north","east","take candy cane", "north","north","take polygon","south","west","take fuel cell","west" };
            var list = (string.Join((char)10,commands)+(char)10).ToCharArray();
            foreach (var n in list)
            {
                amplifier.IO.AddLast(n);
            }
            items = new List<string>()
            {
                "fixed point",
                "hologram",
                "antenna",
                "shell",
                "whirled peas",
                "candy cane",
                "polygon",
                "fuel cell",
            };
            var allPermutes = new List<IEnumerable<string>>();
            for(int i = 1; i <= items.Count; i++)
            {
                allPermutes.AddRange(PermuteUtils.Permute<string>(items, i));
            }
            Console.WriteLine(allPermutes.Count);
            //while (true)
            //{
            //    var output = amplifier.Output;
            //    Print(output);
            //    amplifier.Output.Clear();

            //    string command = Console.ReadLine();
            //    AddCommand(command, amplifier.IO);
            //}

            foreach(var perm in allPermutes)
            {
                Print(amplifier.Output);
                DropAll();
                Take(perm);
                RunCommand("west");
                if (!ContainsWord(amplifier.Output, "lighter") && !ContainsWord(amplifier.Output, "heavier"))
                {

                }
                Print(amplifier.Output);
            }

        }
        private void RunCommands(IEnumerable<string> commands)
        {
            var list = (string.Join((char)10, commands) + (char)10).ToCharArray();
            foreach (var n in list)
            {
                amplifier.IO.AddLast(n);
            }
            amplifier.WorkUntilHaltOrWaitForInput();
        }
        private void RunCommand(string cmd)
        {
            var list = (cmd + (char)10).ToCharArray();
            foreach (var n in list)
            {
                amplifier.IO.AddLast(n);
            }
            amplifier.WorkUntilHaltOrWaitForInput();
        }

        private void DropAll()
        {
            foreach(var itm in items)
            {
                RunCommands(new List<string>() { "drop " + itm });
            }
        }
        private void Take(IEnumerable<string> taken)
        {
            foreach (var itm in taken)
            {
                RunCommands(new List<string>() { "take " + itm });
            }
        }
        private void AddCommand(string command, LinkedList<long> IO)
        {
            var list = (command + (char)10).ToCharArray();
            foreach (var n in list)
            {
                IO.AddLast(n);
            }
        }

        private void Print(List<long> output)
        {
            foreach (var ch in output)
                Console.Write((char)ch);
            output.Clear();
        }
        public override void Start2()
        {
            var filename = GetFileName1();
            var amplifier = new Amplifier()
            {
                IntCode = GetLongList_BySeparator(filename, ","),
            };

            amplifier.IntCode[0] = 2;

            var list = new List<int> {'A', ',', 'B', ',', 'A', ',', 'B', ',', 'A', ',', 'C', ',', 'B', ',', 'C', ',', 'A', ',', 'C',  10
                                    , 'R', ',', '4', ',', 'L', ',', '1', '0', ',', 'L', ',', '1', '0', 10
                                    , 'L', ',', '8', ',', 'R', ',', '1', '2', ',', 'R', ',', '1', '0', ',', 'R', ',', '4', 10
                                    , 'L', ',', '8', ',', 'L', ',', '8', ',', 'R', ',', '1', '0', ',', 'R', ',', '4', 10
                                    , 'n', 10
            };

            foreach(var n in list)
            {
                amplifier.IO.AddLast(n);
            }
            amplifier.WorkUntilHaltOrWaitForInput();

            var output = amplifier.Output;
            foreach (var ch in output)
            {
                Console.Write((char)ch);
            }
                                  
        }
        
        public bool ContainsWord(List<long> output, string word)
        {
            string str = new string(GetCharArray(output));
            return str.Contains(word);
        }
        private char[] GetCharArray(List<long> longArray)
        {
            var res = new char[longArray.Count];
            for (int i = 0; i < longArray.Count; i++)
            {
                res[i] = (char)longArray[i];
            }
            return res;
        }
    }
    public class PermuteUtils
    {
        // Returns an enumeration of enumerators, one for each permutation
        // of the input.
        public static IEnumerable<IEnumerable<T>> Permute<T>(IEnumerable<T> list, int count)
        {
            if (count == 0)
            {
                yield return new T[0];
            }
            else
            {
                int startingElementIndex = 0;
                foreach (T startingElement in list)
                {
                    IEnumerable<T> remainingItems = AllExcept(list, startingElementIndex);

                    foreach (IEnumerable<T> permutationOfRemainder in Permute(remainingItems, count - 1))
                    {
                        yield return Concat<T>(
                            new T[] { startingElement },
                            permutationOfRemainder);
                    }
                    startingElementIndex += 1;
                }
            }
        }

        // Enumerates over contents of both lists.
        public static IEnumerable<T> Concat<T>(IEnumerable<T> a, IEnumerable<T> b)
        {
            foreach (T item in a) { yield return item; }
            foreach (T item in b) { yield return item; }
        }

        // Enumerates over all items in the input, skipping over the item
        // with the specified offset.
        public static IEnumerable<T> AllExcept<T>(IEnumerable<T> input, int indexToSkip)
        {
            int index = 0;
            foreach (T item in input)
            {
                if (index != indexToSkip) yield return item;
                index += 1;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2019
{
    class Task14 : TaskI
    {
        public override int TaskNumber => 14;
        //private List<Ingridient> AllIngridients;
        //private List<Ingridient> AllReady;
        private Dictionary<string,Ingridient> AllIngridients2;
        private Dictionary<string, Ingridient> AllReady2;
        private Dictionary<string, List<long>> DeltaTimes;
        private Dictionary<string, long> LastZeroTimes;

        public override void Start1()
        {
            //OreCount = 0;
            //var lines = GetStringList_ByLine(GetFileName1());
            //AllIngridients = new List<Ingridient>();
            //AllReady = new List<Ingridient>();
            //AllIngridients.Add(AddIngridient("ORE", 1));
            //AllReady.Add(AddIngridient("ORE", 0));

            //foreach (var line in lines)
            //{
            //    var result = line.Split(" => ")[1];
            //    var resultIngr = AddIngridient(result.Split(" ")[1], int.Parse(result.Split(" ")[0]));
            //    var useIngr = line.Split(" => ")[0].Split(", ");
            //    AllIngridients.Add(resultIngr);
            //    AllReady.Add(new Ingridient(result.Split(" ")[1], 0));
            //    foreach (var use in useIngr)
            //    {
            //        resultIngr.Ingridients.Add(AddIngridient(use.Split(" ")[1], int.Parse(use.Split(" ")[0])));
            //    }
            //}
            //AllIngridients.FirstOrDefault(x => x.Name == "FUEL").Produce(AllIngridients, AllReady);

            //Console.WriteLine(OreCount);
        }

        private Ingridient AddIngridient(string name, long count)
        {
            //if (AllIngridients.Any(x => x.Name == name))
            //{
            //    return AllIngridients.Where(x => x.Name == name).FirstOrDefault();
            //}
            var res = new Ingridient(name, count);
            //AllIngridients.Add(res);

            return res;
        }

        public override void Start2()
        {
            var lines = GetStringList_ByLine(GetFileName1());
            AllIngridients2 = new Dictionary<string, Ingridient>();
            AllReady2 = new Dictionary<string, Ingridient>();
            DeltaTimes = new Dictionary<string, List<long>>();
            LastZeroTimes = new Dictionary<string, long>();
            AllIngridients2.Add("ORE",AddIngridient("ORE", 1));
            AllReady2.Add("ORE", AddIngridient("ORE", 1000000000000));

            foreach (var line in lines)
            {
                var result = line.Split(" => ")[1];
                var resultIngr = AddIngridient(result.Split(" ")[1], int.Parse(result.Split(" ")[0]));
                var useIngr = line.Split(" => ")[0].Split(", ");
                AllIngridients2.Add(resultIngr.Name, resultIngr);
                DeltaTimes.Add(resultIngr.Name, new List<long>());
                LastZeroTimes.Add(resultIngr.Name, 0);
                AllReady2.Add(result.Split(" ")[1],new Ingridient(result.Split(" ")[1], 0));
                foreach (var use in useIngr)
                {
                    resultIngr.Ingridients.Add(use.Split(" ")[1], AddIngridient(use.Split(" ")[1], int.Parse(use.Split(" ")[0])));
                }
            }
            long fuel = 0;

            Dictionary<string, List<long>> RepetableArrays = new Dictionary<string, List<long>>();
            while (true)
            {
                AllIngridients2["FUEL"].Produce(AllIngridients2, AllReady2);
                fuel++;

                if(!AllReady2.Values.Any(x=>x.Count != 0))
                {
                    Console.WriteLine(fuel);
                }
                foreach(var ing in AllReady2)
                {
                    if (ing.Value.Count == 0)
                    {
                        DeltaTimes[ing.Value.Name].Add(fuel - LastZeroTimes[ing.Value.Name]);
                        LastZeroTimes[ing.Value.Name] = fuel;
                    }
                }
                if (fuel == 100)
                {
                    foreach (var deltaEl in DeltaTimes)
                    {
                        RepetableArrays.Add(deltaEl.Key, GetRepeatableSubArray(deltaEl.Value));
                    }
                    PrintArray(RepetableArrays);
                }
            }
        }

        private void PrintArray(Dictionary<string, List<long>> RepetableArrays)
        {
            foreach(var repeat in RepetableArrays)
            {
                Console.WriteLine($"{repeat.Key} - {String.Join(", ", repeat.Value)}");
            }
            Console.WriteLine();
        }

        private List<long> GetRepeatableSubArray(List<long> array)
        {
            for(int repeatSize = 1; repeatSize < array.Count; repeatSize++)
            {
                bool match = true;
                for(int i = 0; i < array.Count - repeatSize; i++)
                {
                    if (array[i] != array[i + repeatSize])
                    {
                        match = false;
                    }
                }
                if (match)
                    return array.GetRange(0, repeatSize);
            }
            return array.GetRange(0,array.Count);
        }

        private void PrintReady()
        {
            foreach(var ing in AllReady2.Values)
            {
                Console.WriteLine($"{ing.Name} : {ing.Count}");
            }
            Console.WriteLine();
        }
        public static long OreCount = 0;
        class Ingridient
        {
            public string Name { get; set; }
            public long Count { get; set; }
            public Dictionary<string, Ingridient> Ingridients { get; set; }

            public Ingridient(string name, long count)
            {
                Name = name;
                Count = count;
                Ingridients=new Dictionary<string, Ingridient>();
            }

            public void Produce(Dictionary<string, Ingridient> recepts, Dictionary<string, Ingridient> ready)
            {
                if (Name == "ORE")
                {
                    OreCount++;
                }

                foreach (var ingridient in recepts[ Name].Ingridients.Values)
                {
                    while(ready[ingridient.Name].Count < ingridient.Count)
                    {
                        ingridient.Produce(recepts,ready);
                    }
                    //int leftToProduce = TakeIfExists(ingridient.Count, ingridient, ready);
                    //ingridient.Produce(ingridient.Count,recepts,ready);
                    //ready.Add();

                    ready[ingridient.Name].Count -= ingridient.Count;
                }

                ready[Name].Count+= recepts[Name].Count;

            }

        }
    }
}

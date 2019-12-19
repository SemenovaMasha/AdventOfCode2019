using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2019
{
    class Task14 : TaskI
    {
        public override int TaskNumber => 14;
        private List<Ingridient> AllIngridients;
        private List<Ingridient> AllReady;
        public override void Start1()
        {
            var lines = GetStringList_ByLine(GetFileName1());
            AllIngridients = new List<Ingridient>();
            AllIngridients.Add(AddIngridient("ORE", 1));

            foreach (var line in lines)
            {
                var result = line.Split(" => ")[1];
                var resultIngr = AddIngridient(result.Split(" ")[1], int.Parse(result.Split(" ")[0]));
                var useIngr = line.Split(" => ")[0].Split(", ");
                AllIngridients.Add(resultIngr);
                AllReady.Add(new Ingridient(result.Split(" ")[1], 0));
                foreach (var use in useIngr)
                {
                    resultIngr.Ingridients.Add(AddIngridient(use.Split(" ")[1], int.Parse(use.Split(" ")[0])));
                }
            }

            Console.WriteLine(AllIngridients.FirstOrDefault(x=>x.Name=="FUEL").GetOreCount(AllIngridients));
        }

        private Ingridient AddIngridient(string name, int count)
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
            throw new NotImplementedException();
        }

        class Ingridient
        {
            public string Name { get; set; }
            public int Count { get; set; }
            public List<Ingridient> Ingridients { get; set; }

            public Ingridient(string name, int count)
            {
                Name = name;
                Count = count;
                Ingridients=new List<Ingridient>();
            }

            public int GetOreCount(List<Ingridient> ingridients)
            {
                if (Name == "ORE") return 1;
                int count = 0;
                foreach (var ingridient in Ingridients)
                {
                    count += ingridient.Count * ingridients.FirstOrDefault(x => x.Name == ingridient.Name).GetOreCount(ingridients);
                }

                return count;
            }

            public void Produce(int count, List<Ingridient> recepts,List<Ingridient> ready)
            {
                if (Name == "ORE")
                {
                    Count++;
                    return;
                }

                foreach (var ingridient in Ingridients)
                {
                    int leftToProduce = TakeIfExists(ingridient.Count, ingridient, ready);
                    ingridient.Produce(ingridient.Count,recepts,ready);
                    ready.Add();
                }

                return count;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="ingridient"></param>
            /// <param name="ready"></param>
            /// <returns>Left to produce</returns>
            private int TakeIfExists(int count, Ingridient ingridient, List<Ingridient> ready)
            {
                var minToTake = Math.Min(ready.Count(x => x.Name == ingridient.Name), count);
                ready.FirstOrDefault(x => x.Name == ingridient.Name).Count -= minToTake;
                return count - minToTake;
            }
        }
    }
}

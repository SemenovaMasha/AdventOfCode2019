using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2019
{
    class Task6 : TaskI
    {
        public override void Start1()
        {
            var records = GetStringList_ByLine(GetFileName1());

            List<OrbitObject> orbits = new List<OrbitObject>();
            foreach(var record in records)
            {
                string name1 = record.Split(")")[0];
                string name2 = record.Split(")")[1];

                if (!orbits.Any(x => x.Name == name1))
                {
                    orbits.Add(new OrbitObject(name1));
                }
                if (!orbits.Any(x => x.Name == name2))
                {
                    orbits.Add(new OrbitObject(name2));
                }

                orbits.First(x => x.Name == name2).Parent = orbits.First(x => x.Name == name1);
            }
            int sum = 0;

            foreach(var orbit in orbits) {
                sum += orbit.GetNumberOfOrbit();
            }

            Console.WriteLine(sum); ;
        }

        public override void Start2()
        {

            var records = GetStringList_ByLine(GetFileName1());

            List<OrbitObject> orbits = new List<OrbitObject>();
            foreach (var record in records)
            {
                string name1 = record.Split(")")[0];
                string name2 = record.Split(")")[1];

                if (!orbits.Any(x => x.Name == name1))
                {
                    orbits.Add(new OrbitObject(name1));
                }
                if (!orbits.Any(x => x.Name == name2))
                {
                    orbits.Add(new OrbitObject(name2));
                }

                orbits.First(x => x.Name == name2).Parent = orbits.First(x => x.Name == name1);
                orbits.First(x => x.Name == name1).DirectChilds.Add(orbits.First(x => x.Name == name2));
            }

            foreach (var orbit in orbits)
            {
                orbit.GetChilds();
            }

            var YOU = orbits.First(x => x.Name == "YOU");
            var SAN = orbits.First(x => x.Name == "SAN");

            var lastBasis = orbits.Where(x => x.GetChilds().Contains(YOU) && x.GetChilds().Contains(SAN)).OrderBy(x => x.GetChilds().Count).First();
            var path = lastBasis.GetChilds().Where(x => x.GetChilds().Contains(YOU) || x.GetChilds().Contains(SAN));
            Console.WriteLine(path.Count()); 
        }

        public override int TaskNumber()
        {
            return 6;
        }
    }

    class OrbitObject
    {
        public string Name { get; set; }

        public OrbitObject Parent { get; set; }

        private bool OrbitsCalculated { get; set; }

        private int NumberOfOrbits { get; set; }

        public int GetNumberOfOrbit()
        {
            if (OrbitsCalculated)
                return NumberOfOrbits;

            var curr = Parent;
            while (curr != null)
            {
                NumberOfOrbits++;
                curr = curr.Parent;
            }

            OrbitsCalculated = true;
            return NumberOfOrbits;
        }

        public List<OrbitObject> DirectChilds { get; set; } = new List<OrbitObject>();
        private bool ChildsCalculated { get; set; }

        private List<OrbitObject> Childs { get; set; }

        public List<OrbitObject> GetChilds()
        {
            if (ChildsCalculated)
                return Childs;

            Childs = new List<OrbitObject>();

            foreach(var child in DirectChilds)
            {
                Childs.Add(child);
                Childs.AddRange(child.GetChilds());
            }
            ChildsCalculated = true;
            return Childs;
        }

        public OrbitObject(string name)
        {
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}

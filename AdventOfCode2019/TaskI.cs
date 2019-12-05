using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2019
{
    public abstract class TaskI
    {
        public virtual int taskNumber { get; set; }
        public string GetFileName1()
        {
            return inputPath + taskNumber + "_1.txt";
        }
        public string GetFileName2()
        {
            return inputPath + taskNumber + "_2.txt";
        }
        public string GetFileName(string file)
        {
            return inputPath  + file +".txt";
        }
        private string inputPath = "../../../inputs/";
        public abstract void Start1();
        public abstract void Start2();

        public List<int> GetIntList_ByLine(string fileName)
        {
            return System.IO.File
                     .ReadAllLines(fileName)
                     .Select(x => int.Parse(x))
                     .ToList();
        }
    }
}

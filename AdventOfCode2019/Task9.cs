using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode2019
{
    class Task9 : TaskI
    {
        public override int TaskNumber => 9;

        public override void Start1()
        {
            var filename = GetFileName1();
            var amplifier =new Amplifier()
            {
                IntCode = GetLongList_BySeparator(filename, ","),
            };
            amplifier.IO.AddLast(2);
            amplifier.WorkUntilHaltOrWaitForInput();

            Console.WriteLine(amplifier.Output.FindLast(x=>true));
        }

        public override void Start2()
        {
            throw new NotImplementedException();
        }
    }
}

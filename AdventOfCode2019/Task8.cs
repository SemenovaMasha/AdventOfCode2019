using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2019
{
    class Task8 : TaskI
    {
        public override void Start1()
        {
            string input = GetString(GetFileName1());

            int N = 25;
            int M = 6;

            int layersNumber = input.Length / N / M;
            var Layers = new List<Layer>();

            var layerStrings = Split(input, N * M).ToList();

            var max0Layer = layerStrings.OrderBy(i => i.Count(f => f == '0')).First();

            var multi = max0Layer.Count(f => f == '1') * max0Layer.Count(f => f == '2');

            Console.WriteLine(multi);

        }
        private IEnumerable<string> Split(string str, int chunkSize)
        {
            return Enumerable.Range(0, str.Length / chunkSize)
                .Select(i => str.Substring(i * chunkSize, chunkSize));
        }
        public override void Start2()
        {
            string input = GetString(GetFileName1());

            int M = 25;
            int N = 6;

            int layersNumber = input.Length / N / M;
            var layerStrings = Split(input, N * M).ToList();
            StringBuilder resultString = new StringBuilder(new string('0', N * M));

            for (int i = 0; i < N * M; i++)
            {
                foreach (var currentLayer in layerStrings)
                {
                    if (currentLayer[i] != '2')
                    {
                        resultString[i] = currentLayer[i];
                        break;
                    }
                }
            }

            var str = resultString.ToString();
            for (int n = 0; n < N; n++)
            {
                Console.WriteLine(str.Substring(n * M,M).Replace("0"," "));
            }
        }

        public override int TaskNumber => 8;

        class Layer
        {
            public int [,] Pixels { get; set; }

            public Layer(int N, int M)
            {
                Pixels = new int[N, M];
            }
        }
    }
}

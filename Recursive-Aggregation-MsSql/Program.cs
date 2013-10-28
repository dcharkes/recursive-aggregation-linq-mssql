using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Recursive_Aggregation_MsSql.Model;
using Recursive_Aggregation_MsSql.DataGenerator;
using System.Diagnostics;

namespace Recursive_Aggregation_MsSql
{
    class Program
    {
        static void Main(string[] args)
        {
            GradesDataContext context = new GradesDataContext();

            DataGenerator.DataGenerator.generateDataStructure(context);

            Stopwatch st = new Stopwatch();
            st.Start();
            var i = 0;
            foreach (var student in from stud in context.Students select stud)
            {
                i++;
            }
            st.Stop();
            Console.WriteLine("Read {0} students: {1}ms", i, st.ElapsedMilliseconds);


            Console.ReadKey();
        }
    }
}

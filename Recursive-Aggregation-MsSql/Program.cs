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


            //st.Restart();
            //foreach (var student in from stud in context.Students select stud)
            //{
            //    student.name = student.id%2==0?"Jack":"Jesse";
            //}
            //st.Stop();
            //Console.WriteLine("Update 1000 students (memory): {0}ms", st.ElapsedMilliseconds);

            //st.Restart();
            //context.SubmitChanges();
            //st.Stop();
            //Console.WriteLine("Update 1000 students (db): {0}ms", st.ElapsedMilliseconds);

            //st.Restart();
            //DataGenerator.DataGenerator.deleteAllStudents(context);
            //st.Stop();
            //Console.WriteLine("Delete 1000 students: {0}ms", st.ElapsedMilliseconds);

            Console.WriteLine("\nPress Any Key");

            Console.ReadKey();
        }
    }
}

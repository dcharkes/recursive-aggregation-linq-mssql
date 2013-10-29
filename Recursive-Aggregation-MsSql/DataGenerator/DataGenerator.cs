using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Recursive_Aggregation_MsSql.Model;
using System.Diagnostics;

namespace Recursive_Aggregation_MsSql.DataGenerator
{
    class DataGenerator
    {
        public static void generateDataStructure(GradesDataContext context)
        {
            Stopwatch st = new Stopwatch();
            st.Start();
            deleteAllStudents(context);
            st.Stop();
            Console.WriteLine("Truncating database: {0}ms", st.ElapsedMilliseconds);

            generateStudents(context);
        }

        private static void generateStudents(GradesDataContext context)
        {
            Stopwatch st = new Stopwatch();
            st.Start();
            for (int a = 0; a < 1000; a++)
            {
                var s = new Student();
                context.Students.InsertOnSubmit(s);
            }
            st.Stop();
            Console.WriteLine("Create 1000 students (memory): {0}ms", st.ElapsedMilliseconds);

            st.Reset();
            st.Start();
            context.SubmitChanges();
            st.Stop();
            Console.WriteLine("Create 1000 students (db): {0}ms", st.ElapsedMilliseconds);
        }

        public static void deleteAllStudents(GradesDataContext context)
        {
            context.Students.DeleteAllOnSubmit(context.Students);
            context.SubmitChanges();
        }
    }
}

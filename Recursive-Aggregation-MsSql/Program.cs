using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Recursive_Aggregation_MsSql.Model;

namespace Recursive_Aggregation_MsSql
{
    class Program
    {
        static void Main(string[] args)
        {
            GradesDataContext gradesDb = new GradesDataContext();

            var s = new Student();
            gradesDb.Students.InsertOnSubmit(s);
            gradesDb.SubmitChanges();
            Console.WriteLine(s.id);

            foreach (var student in from stud in gradesDb.Students select stud)
            {
                Console.WriteLine("Found a student, id = {0}.", student.id);
            }

            Console.ReadKey();
        }
    }
}

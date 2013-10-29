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
            deleteAllSubmissions(context);
            deleteAllAssignments(context);
            deleteAllUnits(context);
            deleteAllStudents(context);
            st.Stop();
            Console.WriteLine("Truncating database: {0}ms", st.ElapsedMilliseconds);

            generateStudents(context);

            st.Restart();
            generateOther(context);
            st.Stop();
            Console.WriteLine("Create 100 assignments and 100.000 submissions: {0}ms", st.ElapsedMilliseconds);
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

            st.Restart();
            context.SubmitChanges();
            st.Stop();
            Console.WriteLine("Create 1000 students (db): {0}ms", st.ElapsedMilliseconds);
        }

        // This doesn't create a batch insert, and is still slow...
        private static void generateStudentsBatch(GradesDataContext context)
        {
            Stopwatch st = new Stopwatch();
            st.Start();
            var students = new List<Student>();
            for (int a = 0; a < 1000; a++)
            {
                students.Add(new Student());
            }
            context.Students.InsertAllOnSubmit(students);
            st.Stop();
            Console.WriteLine("Create 1000 students (memory): {0}ms", st.ElapsedMilliseconds);

            st.Restart();
            context.SubmitChanges();
            st.Stop();
            Console.WriteLine("Create 1000 students (db): {0}ms", st.ElapsedMilliseconds);
        }

        public static void deleteAllSubmissions(GradesDataContext context)
        {
            context.Submissions.DeleteAllOnSubmit(context.Submissions);
            context.SubmitChanges();
        }

        public static void deleteAllStudents(GradesDataContext context)
        {
            context.Students.DeleteAllOnSubmit(context.Students);
            context.SubmitChanges();
        }

        public static void deleteAllAssignments(GradesDataContext context)
        {
            context.Assignments.DeleteAllOnSubmit(context.Assignments);
            context.SubmitChanges();
        }

        public static void deleteAllUnits(GradesDataContext context)
        {
            context.Units.DeleteAllOnSubmit(context.Units);
            context.SubmitChanges();
        }

        public static Unit generateOther(GradesDataContext context)
        {
            Unit root = new Unit();
            context.Units.InsertOnSubmit(root);
            context.SubmitChanges();

            for (int a = 0; a < 4; a++)
            {
                Unit level1 = new Unit { parentId = root.id };
                context.Units.InsertOnSubmit(level1);
                context.SubmitChanges();
                for (int b = 0; b < 5; b++)
                {
                    Unit level2 = new Unit { parentId = level1.id };
                    context.Units.InsertOnSubmit(level2);
                    context.SubmitChanges();
                    for (int c = 0; c < 5; c++)
                    {
                        Assignment x = new Assignment { Unit=level2};
                        context.Assignments.InsertOnSubmit(x);
                        context.SubmitChanges();

                        foreach (Student s in from stud in context.Students select stud)
                        {
                            var su = new Submission() { Assignment = x, Student = s };
                            context.Submissions.InsertOnSubmit(su);
                            context.SubmitChanges();
                            su.data = "submission" + (a + b + c ^ 2 + su.id * 37);
                            context.SubmitChanges();
                        }
                    }
                }
            }

            return root;
        }
    }
}

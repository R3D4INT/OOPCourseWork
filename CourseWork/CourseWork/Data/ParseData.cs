using CourseWork.Models;
using CourseWork.Objects;

namespace CourseWork.Data
{
    internal class ParseData
    {
        public List<StudentGroup> StudentGroups { get; set; }
        public List<Student> Students { get; set; }
        public List<Teacher> Teachers { get; set; }
        public List<Discipline> Disciplines { get; set; }

        public void AnalyzeParseData()
        {
            foreach (var student in Students)
            {
                foreach (var group in StudentGroups)
                {
                    if (student.StudentGroup.Id == group.Id)
                    {
                        if (group.Students == null)
                        {
                            group.Students = new List<Student>();
                        }
                        group.Students.Add(student);
                    }
                }
            }

            foreach (var teacher in Teachers)
            {
                foreach ( var discipline in Disciplines)
                {
                    if (discipline.Lector.Id == teacher.Id) {
                        teacher.Discipline = discipline;
                    }
                }
            }
        }
    }
}

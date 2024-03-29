using CourseWork.Models;

namespace CourseWork.Objects
{
    internal class Student
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public StudentGroup StudentGroup { get; set; }
        public List<Discipline> Disciplines { get; set; }
    }
}

using CourseWork.Models;
using CourseWork.Objects;

namespace CourseWork.Data
{
    internal class GroupsData
    {
        public List<StudentGroup> StudentGroups { get; set; }
        public List<Teacher> Teachers { get; set; }
        public List<Student> Students { get; set; }
        public List<Discipline> Disciplines { get; set; }
    }
}

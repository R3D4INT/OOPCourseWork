using CourseWork.Objects;

namespace CourseWork.Models
{
    internal class StudentGroup
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<Student> Students { get; set; }
        public AccessibilityGrid Accessibility { get; set; }
    }
}

using CourseWork.Models;

namespace CourseWork.Objects
{
    internal class Teacher
    {
        public string Id {  get; set; }
        public string Name {  get; set; }
        public string Surname { get; set; }
        public AccessibilityGrid Accessibility {  get; set; }
        public Discipline Discipline {  get; set; }
    }
}

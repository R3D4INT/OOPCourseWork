using CourseWork.Models;

namespace CourseWork.Objects
{
    internal class Cell
    {
        public StudentGroup StudentGroup {  get; set; }
        public Teacher Teacher { get; set; }
        public Discipline Discipline { get; set; }
    }
}

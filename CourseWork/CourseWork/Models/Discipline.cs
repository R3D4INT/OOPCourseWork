namespace CourseWork.Objects
{
    internal class Discipline
    {
        public string Id {  get; set; }
        public string Name { get; set; }
        public Teacher Lector { get; set; }
        public int TotalPerWeek { get; set; }
    }
}

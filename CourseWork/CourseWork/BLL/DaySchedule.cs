using CourseWork.Models;
using CourseWork.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWork.BLL
{
    internal class DaySchedule
    {
        public List<Cell> DayLessons { get; set; }

        public DaySchedule()
        {
            DayLessons = new List<Cell>();
            for (int i = 0; i < 5; i++)
            {
                DayLessons.Add(new Cell() {Discipline = new Discipline(), StudentGroup = new StudentGroup()});
            }
        }
    }
}

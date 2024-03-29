using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CourseWork.Models;

namespace CourseWork.BLL
{
    internal class ConsoleSchedulePresenter
    {
        public void OutputTheSchedule(Schedule schedule, bool isForLector)
        {
            Console.WriteLine("          Monday          Tuesday          Wednesday          Thursday          Friday     ");
            Console.WriteLine("----------------------------------------------------------------------------------------------------------");
            var lessonCounter = 1;
            foreach (var dayLessons in schedule.WeekSchedule)
            {
                Console.Write($"{lessonCounter} Lesson");
                foreach (var lesson in dayLessons.DayLessons)
                {
                    string disciplineName;
                    disciplineName = isForLector ? lesson.StudentGroup.Name : lesson.Discipline.Name;
                    if (disciplineName == null)
                    {
                        disciplineName = "Nothing";
                    }
                    Console.Write($"   {disciplineName,-13}");
                }
                lessonCounter++;
                Console.WriteLine();
            }
        }
    }
}

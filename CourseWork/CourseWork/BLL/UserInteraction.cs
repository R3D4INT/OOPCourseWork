using CourseWork.Data;
using CourseWork.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWork.BLL
{
    internal class UserInteraction
    {
        private readonly ParseData data;

        public UserInteraction(ParseData data)
        {
            this.data = data;
        }

        public void StartInteraction()
        {
            Console.WriteLine("Who Are You? \nLector - write 1\nGroup - write 2\nStudent - write 3");
            var input = int.Parse(Console.ReadLine());
            switch (input)
            {
                case 1:
                    {
                        Console.WriteLine("Okey, what is your name?");
                        for (var i = 0; i < data.Teachers.Count; i++)
                        {
                            Console.WriteLine($"{data.Teachers[i].Name} {data.Teachers[i].Surname} - write {i + 1} to get your schedule");
                        }
                        var index = int.Parse(Console.ReadLine());
                        var schedule = new Schedule();
                        schedule = schedule.CreateScheduleForTeacher(data.Teachers[index - 1], data.StudentGroups);
                        var ConsoleSchedulePresenter = new ConsoleSchedulePresenter();
                        ConsoleSchedulePresenter.OutputTheSchedule(schedule, true);
                        break;
                    }
                case 2:
                    {
                        Console.WriteLine("Okey, what is the name of group?");
                        for (var i = 0; i < data.StudentGroups.Count; i++)
                        {
                            Console.WriteLine($"{data.StudentGroups[i].Name} - write {i + 1} to get your schedule");
                        }
                        var index = int.Parse(Console.ReadLine());
                        var schedule = new Schedule();
                        schedule = schedule.GetScheduleForGroup(schedule.GetScheduleForAllTeachers(data.Teachers, data.StudentGroups), data.StudentGroups[index - 1]);
                        var presenter = new ConsoleSchedulePresenter();
                        presenter.OutputTheSchedule(schedule, false);
                        break;
                    }
                case 3:
                {
                    Console.WriteLine("Okey, what is your name?");
                    for (var i = 0; i < data.Students.Count; i++)
                    {
                        Console.WriteLine($"{data.Students[i].Name} {data.Students[i].Surname} - write " +
                                          $"{i + 1} to get your schedule");
                    }
                    var index = int.Parse(Console.ReadLine());
                    var schedule = new Schedule();
                    schedule = schedule.GetScheduleForGroup(schedule.GetScheduleForAllTeachers(data.Teachers, data.StudentGroups), data.Students[index - 1].StudentGroup);
                    var presenter = new ConsoleSchedulePresenter();
                    presenter.OutputTheSchedule(schedule, false);
                        break;
                }
            }
        }
    }
}

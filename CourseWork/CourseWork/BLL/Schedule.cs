using CourseWork.Data;
using CourseWork.Models;
using CourseWork.Objects;

namespace CourseWork.BLL
{
    internal class Schedule
    {
        public List<DaySchedule> WeekSchedule { get; set; }

        public Schedule CreateScheduleForTeacher(Teacher teacher, List<StudentGroup> studentGroups)
        {
            var ScheduleForTeacher = new Schedule();
            ScheduleForTeacher.WeekSchedule = new List<DaySchedule>();

            ScheduleForTeacher.FillTheScheduleWithEmptyFields(ScheduleForTeacher);

            var quantity = 0;
            var currentGroup = 0;

            if (isTeacherOverworked(GetFreeLessonsOfTeacher(teacher), teacher.Discipline.TotalPerWeek, studentGroups.Count))
            {
                throw new Exception("AHHHHHHHHHHHHH MAN IT IS IMPOSSIBLE");
            }

            for (int day = 0; day < ScheduleForTeacher.WeekSchedule.Count; day++)
            {
                for (int time = 0; time < ScheduleForTeacher.WeekSchedule[day].DayLessons.Count; time++)
                {
                    if (IsTeacherFree(teacher.Accessibility.accessibility[day, time]) && IsStudentFree(studentGroups[currentGroup].Accessibility.accessibility[day, time]))
                    {
                        Cell lesson = ScheduleForTeacher.WeekSchedule[day].DayLessons[time];
                        lesson.Teacher = teacher;
                        lesson.Discipline = teacher.Discipline;
                        lesson.StudentGroup = studentGroups[currentGroup];
                        quantity++;
                        teacher.Accessibility.accessibility[day, time] = false;
                        studentGroups[currentGroup].Accessibility.accessibility[day, time] = false;
                        ScheduleForTeacher.WeekSchedule[day].DayLessons[time] = lesson;
                    }
                    if (isFullLessonsForGroup(quantity, teacher.Discipline.TotalPerWeek))
                    {
                        quantity = 0;
                        currentGroup++;
                        if (currentGroup == studentGroups.Count)
                        {
                            return ScheduleForTeacher;
                        }
                    }
                }
            }
            return ScheduleForTeacher;
        }
        private bool isTeacherOverworked(int freeLessonsOfTeacher, int disciplinePerWeek, int studentGroupsCount) 
        {
            return freeLessonsOfTeacher < disciplinePerWeek * studentGroupsCount;
        }
        private bool IsTeacherFree(bool time)
        {
            return time;
        }
        private bool IsStudentFree(bool time)
        {
            return time;
        }
        private bool isFullLessonsForGroup(int quantity, int totalPerWeek)
        {
            return quantity == totalPerWeek;
        }
        public List<Schedule> GetScheduleForAllTeachers(List<Teacher> teachers, List<StudentGroup> studentGroups)
        {
            var SchedulesForTeachers = new List<Schedule>();
            for (int i = 0; i < teachers.Count; i++)
            {
                SchedulesForTeachers.Add(new Schedule().CreateScheduleForTeacher(teachers[i], studentGroups));
            }
            return SchedulesForTeachers;
        }
        public Schedule GetScheduleForGroup(List<Schedule> SchedulesForTeachers, StudentGroup studentGroup)
        {
            var ScheduleForGroup = new Schedule();
            ScheduleForGroup.WeekSchedule = new List<DaySchedule>();

            ScheduleForGroup.FillTheScheduleWithEmptyFields(ScheduleForGroup);

            foreach (var scheduleForTeacher in SchedulesForTeachers)
            {
                for (var day = 0; day < scheduleForTeacher.WeekSchedule.Count; day++)
                {
                    for (var time = 0; time < scheduleForTeacher.WeekSchedule[day].DayLessons.Count; time++)
                    {
                        if (isIdEqual(scheduleForTeacher.WeekSchedule[day].DayLessons[time].StudentGroup.Id, studentGroup.Id))
                        {
                            ScheduleForGroup.WeekSchedule[day].DayLessons[time] = scheduleForTeacher.WeekSchedule[day].DayLessons[time];
                        }
                    }
                }
            }
            return ScheduleForGroup;
        }
        private bool isIdEqual(string studentGroupIdFromSchedule, string StudentGroupId)
        {
            return studentGroupIdFromSchedule == StudentGroupId;
        }
        private Schedule FillTheScheduleWithEmptyFields(Schedule Schedule)
        {
            for (var i = 0; i < 5; i++)
            {
                var daySchedule = new DaySchedule();
                WeekSchedule.Add((daySchedule));
            }
            return Schedule;
        }
        private int GetFreeLessonsOfTeacher(Teacher teacher)
        {
            var counter = 0;
            foreach (var lesson in teacher.Accessibility.accessibility)
            {
                if (lesson)
                {
                    counter++;
                }
            }
            return counter;
        }
    }
}

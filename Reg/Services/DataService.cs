using System.Text.Json;
using Reg.Model;
using Reg.ViewModel;

namespace Reg.Services;

public static class DataService
{
    public static List<Student> Students { get; private set; } = new();
    public static List<CourseEnrolled> AllCourses { get; private set; } = new();
    public static Student CurrentStudent { get; set; }

    public static async Task LoadInitialData()
    {
        // โหลดข้อมูลนักศึกษา
        using var studentStream = await FileSystem.OpenAppPackageFileAsync("student.json");
        using var studentReader = new StreamReader(studentStream);
        var studentJson = await studentReader.ReadToEndAsync();
        Students = JsonSerializer.Deserialize<List<Student>>(studentJson) ?? new();

        // โหลดและแปลงข้อมูลรายวิชาเป็น CourseEnrolled
        using var subjectStream = await FileSystem.OpenAppPackageFileAsync("subjects.json");
        using var subjectReader = new StreamReader(subjectStream);
        var subjectJson = await subjectReader.ReadToEndAsync();
        var subjects = JsonSerializer.Deserialize<SubjectsData>(subjectJson);

        AllCourses = subjects?.courses.Select(c => new CourseEnrolled
        {
            course_id = c.course_id,
            course_name = c.course_name,
            year = c.year,
            term = c.term,
            credits = c.credits,
            instructor = c.instructor,
            grade = "-"
        }).ToList() ?? new();
    }

    public static List<CourseEnrolled> GetEnrolledCoursesForTerm(int term)
    {
        return CurrentStudent?.courses_enrolled?
            .Where(c => c.term == term)
            .ToList() ?? new List<CourseEnrolled>();
    }

    public static List<CourseEnrolled> GetAvailableCoursesForTerm(int term)
    {
        var enrolledIds = CurrentStudent?.courses_enrolled?
            .Select(c => c.course_id.Trim())
            .ToList() ?? new List<string>();

        // เปลี่ยนเงื่อนไขให้แสดงเฉพาะวิชาในเทอม 3
        return AllCourses
            .Where(c => c.term == 3 && !enrolledIds.Contains(c.course_id.Trim()))
            .ToList();
    }

    public static void RegisterCourse(CourseEnrolled course)
    {
        if (CurrentStudent != null)
        {
            CurrentStudent.courses_enrolled ??= new List<CourseEnrolled>();
            CurrentStudent.courses_enrolled.Add(course);
        }
    }

    public static void AddToAvailableCourses(CourseEnrolled course)
    {
        // เพิ่มรายวิชาที่ถูกถอนกลับเข้าไปใน AllCourses
        if (!AllCourses.Any(c => c.course_id.Trim() == course.course_id.Trim()))
        {
            AllCourses.Add(course);
        }
    }

    private class SubjectsData
    {
        public List<CourseEnrolled> courses { get; set; }
    }
}

namespace Reg.Model;

public class Student
{
    public string student_id { get; set; }
    public string email { get; set; }
    public string password { get; set; }
    public string first_name { get; set; }
    public string last_name { get; set; }
    public string img_url { get; set; }
    public string faculty { get; set; }
    public string major { get; set; }
    public int year { get; set; }
    public double gpa { get; set; }
    public List<CourseEnrolled> courses_enrolled { get; set; }
}

public class CourseEnrolled
{
    public string course_id { get; set; }
    public string course_name { get; set; }
    public int year { get; set; }
    public int term { get; set; }
    public int credits { get; set; }
    public string grade { get; set; }
    public string instructor { get; set; }
}

public class Course
{
    public string course_id { get; set; }
    public string course_name { get; set; }
    public int year { get; set; }
    public int term { get; set; }
    public int credits { get; set; }
    public string instructor { get; set; }
}

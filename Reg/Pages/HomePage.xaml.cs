using Reg.Model;
using Reg.Services;
using Reg.ViewModel;

namespace Reg.Pages;

[QueryProperty(nameof(Student), "student")]
public partial class HomePage : ContentPage
{
    public Student Student { get; set; }
    
    public HomePage(HomepageVM vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        if (BindingContext is HomepageVM vm)
        {
            if (Student != null)
            {
                // อัพเดทข้อมูลจาก DataService
                var updatedStudent = DataService.Students.FirstOrDefault(s => s.student_id == Student.student_id);
                if (updatedStudent != null)
                {
                    Student = updatedStudent;
                }
            }
            vm.CurrentStudent = Student;
        }
    }
}
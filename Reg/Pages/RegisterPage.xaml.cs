using Reg.ViewModel;

namespace Reg.Pages;

[QueryProperty(nameof(StudentId), "studentId")]
public partial class RegisterPage : ContentPage
{
    public string StudentId
    {
        set
        {
            if (BindingContext is RegisterVM vm)
            {
                vm.StudentId = value;
            }
        }
    }

    public RegisterPage(RegisterVM vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
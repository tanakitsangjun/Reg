using System;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using Reg.Model;
using CommunityToolkit.Mvvm.Input;
using Reg.Services;

namespace Reg.ViewModel;

public partial class RegisterVM : ObservableObject, IQueryAttributable
{
    [ObservableProperty]
    string studentId;

    [ObservableProperty]
    ObservableCollection<CourseEnrolled> availableCourses;  // Changed from Course to CourseEnrolled

    private async Task LoadAvailableCoursesAsync()
    {
        try
        {
            var availableCourses = DataService.GetAvailableCoursesForTerm(1); // Default to term 1
            AvailableCourses = new ObservableCollection<CourseEnrolled>(availableCourses);
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
        }
    }

    [RelayCommand]
    async Task RegisterCourse(CourseEnrolled course)
    {
        try
        {
            var result = await Shell.Current.DisplayAlert("Confirm Registration", 
                $"Register for {course.course_name}?", "Yes", "No");

            if (!result) return;

            DataService.RegisterCourse(course);
            AvailableCourses.Remove(course);

            await Shell.Current.DisplayAlert("Success", 
                $"Successfully registered for {course.course_name}", "OK");
            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", $"Failed to register: {ex.Message}", "OK");
        }
    }

    public async void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        StudentId = query["studentId"].ToString();
        await LoadAvailableCoursesAsync();
    }
}

public class SubjectsData
{
    public List<Course> courses { get; set; }
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

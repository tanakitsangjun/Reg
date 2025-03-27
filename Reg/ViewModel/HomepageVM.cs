using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.Json;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Reg.Model;
using Reg.Pages;
using Reg.Services;

namespace Reg.ViewModel;

public partial class HomepageVM : ObservableObject
{
    [ObservableProperty]
    Student? currentStudent;

    [ObservableProperty]
    ObservableCollection<CourseEnrolled> currentTermCourses = new();

    [ObservableProperty]
    ObservableCollection<CourseEnrolled> availableCourses = new();

    [ObservableProperty]
    int selectedTerm = 1;

    public List<int> AvailableTerms => 
        CurrentStudent?.courses_enrolled?
            .Select(c => c.term)
            .Distinct()
            .OrderBy(t => t)
            .ToList() ?? new List<int> { 1, 2, 3 };

    private void LoadCurrentTermCourses()
    {
        // โหลดวิชาที่ลงทะเบียนแล้ว
        var enrolledCourses = CurrentStudent?.courses_enrolled?
            .Where(c => c.term == SelectedTerm)
            .ToList() ?? new List<CourseEnrolled>();
        CurrentTermCourses = new ObservableCollection<CourseEnrolled>(enrolledCourses);

        // โหลดวิชาที่ยังไม่ได้ลงทะเบียน
        var availableCourses = DataService.GetAvailableCoursesForTerm(SelectedTerm);
        AvailableCourses = new ObservableCollection<CourseEnrolled>(availableCourses);

        OnPropertyChanged(nameof(AvailableTerms));
    }

    [RelayCommand]
    void SelectTerm(int term)
    {
        SelectedTerm = term;
        LoadCurrentTermCourses();
    }

    [RelayCommand]
    async Task RegisterCourse(CourseEnrolled course)
    {
        try
        {
            var result = await Shell.Current.DisplayAlert("Confirm Registration", 
                $"Register for {course.course_name}?", "Yes", "No");

            if (!result) return;

            // เพิ่มวิชาลงในรายการลงทะเบียน
            CurrentStudent.courses_enrolled.Add(course);
            
            // รีโหลดข้อมูลการแสดงผล
            LoadCurrentTermCourses();

            await Shell.Current.DisplayAlert("Success", 
                $"Successfully registered for {course.course_name}", "OK");
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
        }
    }

    [RelayCommand]
    async Task WithdrawCourse(CourseEnrolled course)
    {
        try
        {
            if (course.term != 3)
            {
                await Shell.Current.DisplayAlert("Error", 
                    "Can only withdraw courses from Term 3", "OK");
                return;
            }

            var result = await Shell.Current.DisplayAlert("Confirm Withdrawal", 
                $"Withdraw from {course.course_name}?", "Yes", "No");

            if (!result) return;

            // นำรายวิชาออกจากรายการที่ลงทะเบียน
            CurrentStudent.courses_enrolled.Remove(course);
            
            // เพิ่มรายวิชากลับไปใน AllCourses
            DataService.AddToAvailableCourses(course);
            
            // รีโหลดข้อมูลการแสดงผล
            LoadCurrentTermCourses();

            await Shell.Current.DisplayAlert("Success", 
                $"Successfully withdrew from {course.course_name}", "OK");
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
        }
    }

    partial void OnSelectedTermChanged(int value)
    {
        LoadCurrentTermCourses();
    }

    partial void OnCurrentStudentChanged(Student? value)
    {
        if (value != null)
        {
            DataService.CurrentStudent = value;
            LoadCurrentTermCourses();
        }
    }
}

public partial class TermGroup : ObservableObject
{
    public int Term { get; }
    public ObservableCollection<CourseEnrolled> Courses { get; }
    
    [ObservableProperty]
    bool isExpanded;

    public TermGroup(int term, ObservableCollection<CourseEnrolled> courses)
    {
        Term = term;
        Courses = courses;
    }
}

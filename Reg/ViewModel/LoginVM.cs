using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Text.Json;
using Reg.Model;
using Microsoft.Maui.Controls;
using Reg.Pages;
using Reg.Services;

namespace Reg.ViewModel;

public partial class LoginVM : ObservableObject
{
    [ObservableProperty]
    string email = "";
    [ObservableProperty]
    string password = "";

    private async Task<List<Student>> LoadStudentsAsync()
    {
        try
        {
            return await StudentDataService.LoadStudentsAsync();
        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert("Error", "Failed to load student data: " + ex.Message, "OK");
            return new List<Student>();
        }
    }

    [RelayCommand]
    async Task Login()
    {
        if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password))
        {
            await Application.Current.MainPage.DisplayAlert("Error", "Please enter both Email and Password", "OK");
            return;
        }

        await DataService.LoadInitialData();
        var student = DataService.Students.FirstOrDefault(s => s.email.ToLower() == Email.ToLower() && s.password == Password);
        
        if (student != null)
        {
            DataService.CurrentStudent = student;
            var queryParams = new Dictionary<string, object>
            {
                { "student", student }
            };
            await Shell.Current.GoToAsync($"HomePage", queryParams);
        }
        else
        {
            await Application.Current.MainPage.DisplayAlert("Error", "Invalid email or password", "OK");
        }
    }
}

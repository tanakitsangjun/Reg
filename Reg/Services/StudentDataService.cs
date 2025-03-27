using System.Text.Json;
using Reg.Model;

namespace Reg.Services;

public static class StudentDataService
{
    private static readonly string dataFile = "studentData.json";
    private static readonly string dataPath = Path.Combine(FileSystem.AppDataDirectory, dataFile);

    public static async Task InitializeDataAsync()
    {
        if (!File.Exists(dataPath))
        {
            using var stream = await FileSystem.OpenAppPackageFileAsync("student.json");
            using var reader = new StreamReader(stream);
            var initialData = await reader.ReadToEndAsync();
            await File.WriteAllTextAsync(dataPath, initialData);
        }
    }

    public static async Task<List<Student>> LoadStudentsAsync()
    {
        await InitializeDataAsync();
        var json = await File.ReadAllTextAsync(dataPath);
        return JsonSerializer.Deserialize<List<Student>>(json);
    }

    public static async Task SaveStudentsAsync(List<Student> students)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        var json = JsonSerializer.Serialize(students, options);
        await File.WriteAllTextAsync(dataPath, json);
    }
}

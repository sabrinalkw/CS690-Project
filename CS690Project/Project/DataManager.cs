using System.Reflection.Metadata;

namespace Project;
// this class is for reading files and adding to them
public class DataManager 
{
    FileSaver fileSaver;
    public List<Category> Categories { get; }
    public List<Label> Labels { get; }
    public List<User> Users { get; }
    public List<TaskData> TaskData { get; }
// reads from files
    public DataManager() // load
    {
        fileSaver = new FileSaver("task-data.txt");

        Categories = new List<Category>();
        var categoriesFileContent = File.ReadAllLines("categories.txt");
        foreach (var categoryName in categoriesFileContent)
        {
            Categories.Add(new Category(categoryName));
        }

        Labels = new List<Label>();
        var labelsFileContent = File.ReadAllLines("labels.txt");
        foreach (var line in labelsFileContent)
        {
            var parts = line.Split('|');
            var labelName = parts[0];
            var categoryName = parts[1];

            var category = Categories.First(c => c.Name == categoryName);
            var label = new Label(labelName);
            Labels.Add(label);
            category.Labels.Add(label);
        }

        Users = new List<User>();
        var userFileContent = File.ReadAllLines("users.txt");
        foreach (var userName in userFileContent)
        {
            Users.Add(new User(userName));
        }

        TaskData = new List<TaskData>();
        if (File.Exists("task-data.txt"))
        {
            var taskFileContent = File.ReadAllLines("task-data.txt");
            foreach (var line in taskFileContent)
            {
                var parts = line.Split('-', StringSplitOptions.RemoveEmptyEntries);
                var user = Users.First(u => u.Name == parts[0]);
                var category = Categories.First(c => c.Name == parts[1]);
                var label = category.Labels.First(l => l.Name == parts[2]);
                var dueDate = DateTime.Parse(parts[3]);
                var status = new Status(parts[4].Trim().ToLower() == "complete");

                TaskData.Add(new TaskData(dueDate, user, category, label, status));
            }
        }
    }
}

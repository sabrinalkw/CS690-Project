using System.Reflection.Metadata;

namespace Project;

public class DataManager
{
    FileSaver fileSaver;
    public List<Category> Categories { get; }
    public List<Label> Labels { get; } 
    public List<User> Users { get; }
    public List<TaskData> TaskData { get; }
    public DataManager()
    {
         fileSaver = new FileSaver("task-data.txt");

            Categories = new List<Category>(); 
            var categoriesFileContent = File.ReadAllLines("categories.txt");

            foreach(var categoryName in categoriesFileContent){
                Categories.Add(new Category(categoryName));
            }

            Labels = new List<Label>();
            var labelsFileContent = File.ReadAllLines("labels.txt");

            foreach(var labelName in labelsFileContent){
                Labels.Add(new Label(labelName));
        }

            // add different Labels to different Categories 
            Categories[0].Labels.Add(Labels[0]);
            Categories[0].Labels.Add(Labels[1]);
            Categories[1].Labels.Add(Labels[2]);
            Categories[1].Labels.Add(Labels[3]);
            Categories[1].Labels.Add(Labels[4]);

            Users = new List<User>();
            Users.Add(new User("Jane"));
            Users.Add(new User("John"));

            TaskData = new List<TaskData>();

        if (File.Exists("task-data.txt"))
        {
            var taskFileContent = File.ReadAllLines("task-data.txt");
            foreach(var line in taskFileContent)
            {
                var splitted = line.Split("-", StringSplitOptions.RemoveEmptyEntries);
                var userName = splitted[0];
                var user = new User(userName);

                var categoryName = splitted[1];
                var category = new Category(categoryName);

                var taskName = splitted[2];
                var label = new Label(taskName);

                var dueDate = DateTime.Parse(splitted[3]);

                bool Complete = splitted[4].Trim().ToLower() == "complete";
                Status status = new Status(Complete);
           

                TaskData.Add(new TaskData(dueDate, user, category, label, status));
            }; 
        }
        
    }
    public void AddNewTaskData(TaskData data)
    {
        this.TaskData.Add(data);
        this.fileSaver.AppendData(data);
    }

    public void SynchronizeLabels()
    {
        File.Delete("labels.txt");
        foreach(var label in Labels)
        {
            File.AppendAllText("labels.txt", label.Name+Environment.NewLine);
        }
    }

     public void SynchronizeCategories()
    {
        File.Delete("categories.txt");
        foreach(var category in Categories)
        {
            File.AppendAllText("categories.txt", category.Name+Environment.NewLine);
        }
    }


public void SaveAllTasks()
{
    File.Delete("task-data.txt");

    foreach (var task in TaskData)
    {
        fileSaver.AppendData(task);
    }
}
}
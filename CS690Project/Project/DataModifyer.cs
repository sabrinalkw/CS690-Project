namespace Project;

// class used to modifyfiles from input from ConsoleUi
public class DataModifyer
{
    FileSaver fileSaver;
    DataManager dataManager;
    DataModifyer dataModifyer;

    public DataModifyer(DataManager dataManager)
    {
        this.dataManager = dataManager;
        this.fileSaver = new FileSaver("task-data.txt");
    }

    public List<User> Users => dataManager.Users;

    public void SaveAllTasks()
    {
        File.Delete("task-data.txt");

        foreach (var task in dataManager.TaskData)
        {
            fileSaver.AppendData(task);
        }
    }

    public void AddNewTaskData(TaskData data)
    {
        this.dataManager.TaskData.Add(data);
        this.fileSaver.AppendData(data);
    }

    public void SynchronizeUsers()
    {
        File.Delete("users.txt");
        foreach (var user in dataManager.Users)
        {
            File.AppendAllText("users.txt", user.Name + Environment.NewLine);
        }
    }

    public void AddUser(User user) // Tested in DataModifyerTests.cs
    {
        dataManager.Users.Add(user);
        SynchronizeUsers();
    }

    public void SynchronizeLabels()
    {
        File.Delete("labels.txt");
        foreach (var category in dataManager.Categories)
        {
            foreach (var label in category.Labels)
            {
                File.AppendAllText(
                    "labels.txt",
                    $"{label.Name}|{category.Name}{Environment.NewLine}"
                );
            }
        }
    }

    public void AddLabel(Label label, Category category)
    {
        dataManager.Labels.Add(label);
        category.Labels.Add(label);
        SynchronizeLabels();
        SynchronizeCategories();
    }

    public void SynchronizeCategories()
    {
        File.Delete("categories.txt");
        foreach (var category in dataManager.Categories)
        {
            File.AppendAllText("categories.txt", category.Name + Environment.NewLine);
        }
    }

    public void AddCategory(Category category)
    {
        dataManager.Categories.Add(category);
        SynchronizeCategories();
    }
}

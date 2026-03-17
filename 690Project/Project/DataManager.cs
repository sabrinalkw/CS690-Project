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
            Categories.Add(new Category("Food"));
            Categories.Add(new Category("Vet"));
            Categories.Add(new Category("3"));

            Labels = new List<Label>();
            Labels.Add(new Label("treats"));
            Labels.Add(new Label("kibble"));
            Labels.Add(new Label("vaccines"));
            Labels.Add(new Label("check up"));

            // add different Labels to different Categories 
            Categories[0].Labels.Add(Labels[0]);
            Categories[0].Labels.Add(Labels[1]);
            Categories[1].Labels.Add(Labels[2]);
            Categories[1].Labels.Add(Labels[3]);

            Users = new List<User>();
            Users.Add(new User("Jane"));
            Users.Add(new User("John"));

            TaskData = new List<TaskData>();
    }
    public void AddNewTaskData(TaskData data)
    {
        this.TaskData.Add(data);
        this.fileSaver.AppendData(data);
    }
}
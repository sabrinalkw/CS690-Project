using Spectre.Console;

namespace Project;

// this class has peices from the ConsoleQuestions.cs, that are put together to create combo code pieces for ConsoleQuestions, as for parts of Console UI there is repeating code
public class ConsoleComponets
{
    DataManager dataManager;
    DataModifyer dataModifyer;
    ConsoleQuestions consoleQuestions;

    public ConsoleComponets(
        DataManager dataManager,
        DataModifyer dataModifyer,
        ConsoleQuestions consoleQuestions
    )
    {
        this.dataManager = dataManager;
        this.dataModifyer = dataModifyer;
        this.consoleQuestions = consoleQuestions;
    }


    public string AddOrUpdateTask(string command, User selectedUser, Status taskStatus)
    {
        string listUpdate = consoleQuestions.UpdateTaskList();

        if (listUpdate == "choose from existing list")
        {
            command = SelectTaskFromExistingList(selectedUser, taskStatus);
        }
        if (listUpdate == "add new task or category")
        {
            string txtUpdate = consoleQuestions.NewCategoryOrExisting();

            if (txtUpdate == "add new category")
            {
                SubmitNewCategory();
            }
            if (txtUpdate == "add new task for existing category")
            {
                AddNewTaskForCateogry();
            }
        }

        return command;
    }

    public string ShowUpcomingTasks()
    {
        string command;
        var result = Reporter.ShowTasksUpcoming(dataManager.TaskData);
        ConsoleInputs.PrintTasks(result, "Your upcoming tasks are:");
        command = consoleQuestions.SubmitMethod();
        return command;
    }

    public string ShowCompletedTasks()
    {
        string command;
        var result = Reporter.ShowTasksCompleted(dataManager.TaskData);
        ConsoleInputs.PrintTasks(result, "Your completed tasks are:");

        command = consoleQuestions.SubmitMethod();
        return command;
    }

    public void UpdateEnteredTask()
    {
        var incompleteTasks = Reporter.ShowTasksUpcoming(dataManager.TaskData).ToList();

        TaskData selectedUpdate = consoleQuestions.SelectFromList(incompleteTasks);

        selectedUpdate.Status.Complete = true;
        dataModifyer.SaveAllTasks();
    }

    public void AddNewTaskForCateogry()
    {
        Category selectedCategory = consoleQuestions.CategorySelect();

        string newLabelName = consoleQuestions.NewTaskForCategory();

        dataModifyer.AddLabel(new Label(newLabelName), selectedCategory);
    }

    public void SubmitNewCategory()
    {
        string newCategoryName = consoleQuestions.PromptCategoryName();

        dataModifyer.AddCategory(new Category(newCategoryName));
        string newLabelName = consoleQuestions.NewTaskForCategory();
        var addedCategory = dataManager.Categories.Last();
        dataModifyer.AddLabel(new Label(newLabelName), addedCategory);
    }

    public string SelectTaskFromExistingList(User selectedUser, Status taskStatus)
    {
        string command;
        Category selectedCategory = consoleQuestions.CategorySelect();

        Label selectedLabel = consoleQuestions.LabelSelect(selectedCategory);

        DateTime dueDate = ConsoleInputs.GetDate();

        TaskData data = new TaskData(
            dueDate,
            selectedUser,
            selectedCategory,
            selectedLabel,
            taskStatus
        );

        dataModifyer.AddNewTaskData(data);

        command = consoleQuestions.SubmitMethod();
        return command;
    }
}

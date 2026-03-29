using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq.Expressions;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using Microsoft.VisualBasic;
using Spectre.Console;

namespace Project;

public class ConsoleUI
{
    DataManager dataManager;
    DataModifyer dataModifyer;

    public ConsoleUI()
    {
        dataManager = new DataManager();
        dataModifyer = new DataModifyer((dataManager));
    }

    public void Show()
    {
        string command = "";

        string user = ModeSelect();

        if (user == "current user")
        {
            do
            {
                User selectedUser = UserSelect();

                Console.WriteLine("Current user is: " + selectedUser.Name);

                string selectedStatus = SelectCompleteUpcoming();

                Status taskStatus = new Status(selectedStatus == "completed");

                if (selectedStatus == "completed")
                {
                    string viewEdit = SelectViewEdit();

                    if (viewEdit == "edit tasks")
                    {
                        string updateEdit = NewOrUpdate();

                        if (updateEdit == "add new task")
                        {
                            string listUpdate = UpdateTaskList();

                            if (listUpdate == "choose from existing list")
                            {
                                Category selectedCategory = CategorySelect();

                                Label selectedLabel = LabelSelect(selectedCategory);

                                DateTime dueDate = ConsoleInputs.GetDate();

                                TaskData data = new TaskData(
                                    dueDate,
                                    selectedUser,
                                    selectedCategory,
                                    selectedLabel,
                                    taskStatus
                                );

                                dataModifyer.AddNewTaskData(data);

                                command = ConsoleInputs.AskForInput("Enter submit: ");
                            }
                            if (listUpdate == "add new task or category")
                            {
                                string txtUpdate = NewCategoryOrExisting();

                                if (txtUpdate == "add new category")
                                {
                                    string newCategoryName = PromptCategoryName();

                                    dataModifyer.AddCategory(new Category(newCategoryName));
                                    string newLabelName = NewTaskForCategory();
                                    var addedCategory = dataManager.Categories.Last();
                                    dataModifyer.AddLabel(new Label(newLabelName), addedCategory);
                                }
                                if (txtUpdate == "add new task for existing category")
                                {
                                    Category selectedCategory = CategorySelect();

                                    string newLabelName = NewTaskForCategory();

                                    dataModifyer.AddLabel(
                                        new Label(newLabelName),
                                        selectedCategory
                                    );
                                }
                            }
                        }
                        if (updateEdit == "update previously entered task")
                        {
                            var incompleteTasks = Reporter
                                .ShowTasksUpcoming(dataManager.TaskData)
                                .ToList();

                            TaskData selectedUpdate = SelectFromList(incompleteTasks);

                            selectedUpdate.Status.Complete = true;
                            dataModifyer.SaveAllTasks();
                        }
                    }

                    if (viewEdit == "view tasks")
                    {
                        var result = Reporter.ShowTasksCompleted(dataManager.TaskData);
                        ConsoleInputs.PrintTasks(result, "Your completed tasks are:");

                        command = SubmitMethod();
                    }
                }
                if (selectedStatus == "upcoming")
                {
                    string viewEdit = SelectViewEdit();

                    if (viewEdit == "edit tasks")
                    {
                        string updateEdit = NewOrUpdate();

                        if (updateEdit == "add new task")
                        {
                            string listUpdate = UpdateTaskList();

                            if (listUpdate == "choose from existing list")
                            {
                                Category selectedCategory = CategorySelect();

                                Label selectedLabel = LabelSelect(selectedCategory);

                                DateTime dueDate = ConsoleInputs.GetDate();

                                TaskData data = new TaskData(
                                    dueDate,
                                    selectedUser,
                                    selectedCategory,
                                    selectedLabel,
                                    taskStatus
                                );

                                dataModifyer.AddNewTaskData(data);

                                command = SubmitMethod();
                            }
                            if (listUpdate == "add new task or category")
                            {
                                string txtUpdate = NewCategoryOrExisting();

                                if (txtUpdate == "add new category")
                                {
                                    string newCategoryName = PromptCategoryName();

                                    dataModifyer.AddCategory(new Category(newCategoryName));
                                    string newLabelName = NewTaskForCategory();
                                    var addedCategory = dataManager.Categories.Last();
                                    dataModifyer.AddLabel(new Label(newLabelName), addedCategory);
                                }
                                if (txtUpdate == "add new task for existing category")
                                {
                                    Category selectedCategory = CategorySelect();

                                    string newLabelName = NewTaskForCategory();

                                    dataModifyer.AddLabel(
                                        new Label(newLabelName),
                                        selectedCategory
                                    );
                                }
                            }
                        }
                    }

                    if (viewEdit == "view tasks")
                    {
                        var result = Reporter.ShowTasksUpcoming(dataManager.TaskData);
                        ConsoleInputs.PrintTasks(result, "Your upcoming tasks are:");
                        command = SubmitMethod();
                    }
                }
            } while (command != "submit");
        }

        if (user == "new user")
        {
            NewUserSelect();
        }
    }

    private static string SubmitMethod()
    {
        return ConsoleInputs.AskForInput("Enter submit: ");
    }

    private void NewUserSelect()
    {
        var newUserName = AnsiConsole.Prompt(new TextPrompt<string>("Enter new user's name:"));
        dataModifyer.AddUser(new User(newUserName));
    }

    private static string NewTaskForCategory()
    {
        return AnsiConsole.Prompt(new TextPrompt<string>("Enter new task for this category:"));
    }

    private static string PromptCategoryName()
    {
        return AnsiConsole.Prompt(new TextPrompt<string>("Enter new category name:"));
    }

    private static string NewCategoryOrExisting()
    {
        return ConsoleInputs.SelectFromStrings(
            "Please select: add new category or add new task for existing category",
            "add new category",
            "add new task for existing category"
        );
    }

    private static TaskData SelectFromList(List<TaskData> incompleteTasks)
    {
        return ConsoleInputs.SelectFromList(
            "Please select task to mark as complete ",
            incompleteTasks
        );
    }

    private static void NewOrEntered()
    {
        var updateEdit = ConsoleInputs.SelectFromStrings(
            "Mark previously entered task as complete or add new task",
            "update previously entered task",
            "add new task"
        );
    }

    private static Label LabelSelect(Category selectedCategory)
    {
        return ConsoleInputs.SelectFromList("Please select task label: ", selectedCategory.Labels);
    }

    private static string UpdateTaskList()
    {
        return ConsoleInputs.SelectFromStrings(
            "Please select: choose from list or add new task category",
            "add new task or category",
            "choose from existing list"
        );
    }

    private static string NewOrUpdate()
    {
        return ConsoleInputs.SelectFromStrings(
            "Mark previously entered task as complete or add new task",
            "update previously entered task",
            "add new task"
        );
    }

    private static string SelectViewEdit()
    {
        return ConsoleInputs.SelectFromStrings(
            "Please select: view tasks or edit tasks:",
            "view tasks",
            "edit tasks"
        );
    }

    private static string SelectCompleteUpcoming()
    {
        return ConsoleInputs.SelectFromStrings(
            "Please select upcoming tasks or current tasks:",
            "upcoming",
            "completed"
        );
    }

    private User UserSelect()
    {
        return ConsoleInputs.SelectFromList("Please select a user:", dataManager.Users);
    }

    private static string ModeSelect()
    {
        return ConsoleInputs.SelectFromStrings(
            "Please select mode new user or current user:",
            "new user",
            "current user"
        );
    }

    private Category CategorySelect()
    {
        return ConsoleInputs.SelectFromList(
            "Please select task category: ",
            dataManager.Categories
        );
    }
}

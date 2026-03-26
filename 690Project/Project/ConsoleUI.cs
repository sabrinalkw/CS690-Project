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
        var user = SelectFromStrings(
            "Please select mode new user or current user:",
            "new user",
            "current user"
        );

        if (user == "current user")
        {
            string command = "";

            do
            {
                var selectedUser = SelectFromList("Please select a user:", dataManager.Users);

                Console.WriteLine("Current user is: " + selectedUser.Name);

                var selectedStatus = SelectFromStrings(
                    "Please select upcoming tasks or current tasks:",
                    "upcoming",
                    "completed"
                );

                Status taskStatus = new Status(selectedStatus == "completed");

                if (selectedStatus == "completed")
                {
                    var viewEdit = SelectFromStrings(
                        "Please select: view tasks or edit tasks:",
                        "view tasks",
                        "edit tasks"
                    );

                    if (viewEdit == "edit tasks")
                    {
                        var updateEdit = SelectFromStrings(
                            "Mark previously entered task as complete or add new task",
                            "update previously entered task",
                            "add new task"
                        );

                        if (updateEdit == "add new task")
                        {
                            var listUpdate = SelectFromStrings(
                                "Please select: choose from list or add new task category",
                                "add new task or category",
                                "choose from existing list"
                            );

                            if (listUpdate == "choose from existing list")
                            {
                                var selectedCategory = SelectFromList(
                                    "Please select task category: ",
                                    dataManager.Categories
                                );
                                var selectedLabel = SelectFromList(
                                    "Please select task label: ",
                                    selectedCategory.Labels
                                );

                                var dueDate = AnsiConsole.Prompt(
                                    new TextPrompt<DateTime>(
                                        "Please enter upcoming task due date and time (in m/d/y hr:min format):"
                                    )
                                );

                                TaskData data = new TaskData(
                                    dueDate,
                                    selectedUser,
                                    selectedCategory,
                                    selectedLabel,
                                    taskStatus
                                );

                                dataModifyer.AddNewTaskData(data);

                                command = AskForInput("Enter submit: ");
                            }
                            else if (listUpdate == "add new task or category")
                            {
                                var txtUpdate = SelectFromStrings(
                                    "Please select: add new category or add new task for existing category",
                                    "add new category",
                                    "add new task for existing category"
                                );

                                if (txtUpdate == "add new category")
                                {
                                    var newCategoryName = AnsiConsole.Prompt(
                                        new TextPrompt<string>("Enter new category name:")
                                    );
                                    dataModifyer.AddCategory(new Category(newCategoryName));

                                    var newLabelName = AnsiConsole.Prompt(
                                        new TextPrompt<string>("Enter new task for this category:")
                                    );
                                    var addedCategory = dataManager.Categories.Last();
                                    dataModifyer.AddLabel(new Label(newLabelName), addedCategory);
                                }
                                else if (txtUpdate == "add new task for existing category")
                                {
                                    var selectedCategory = SelectFromList(
                                        "Please select task category: ",
                                        dataManager.Categories
                                    );

                                    var newLabelName = AnsiConsole.Prompt(
                                        new TextPrompt<string>("Enter new task for this category:")
                                    );
                                    dataModifyer.AddLabel(
                                        new Label(newLabelName),
                                        selectedCategory
                                    );
                                }
                            }
                        }
                        else if (updateEdit == "update previously entered task")
                        {
                            var incompleteTasks = Reporter
                                .ShowTasksUpcoming(dataManager.TaskData)
                                .ToList();

                            var selectedUpdate = SelectFromList(
                                "Please select task to mark as complete ",
                                incompleteTasks
                            );

                            selectedUpdate.Status.Complete = true;
                            dataModifyer.SaveAllTasks();
                        }
                    }

                    if (viewEdit == "view tasks")
                    {
                        var result = Reporter.ShowTasksCompleted(dataManager.TaskData);
                        Console.WriteLine("Your completed tasks are:");
                        foreach (var task in result)
                        {
                            Console.WriteLine(
                                $"- User: {task.User} | {task.Category} |  {task.Label} | Due: {task.DueDate}"
                            );
                        }
                        command = AskForInput("Enter submit: ");
                    }
                }
                else if (selectedStatus == "upcoming")
                {
                    var viewEdit = SelectFromStrings(
                        "Please select: view tasks or edit tasks:",
                        "view tasks",
                        "edit tasks"
                    );
                    if (viewEdit == "edit tasks")
                    {
                        var updateEdit = SelectFromStrings(
                            "Mark previously entered task as complete or add new task",
                            "update previously entered task",
                            "add new task"
                        );

                        var selectedCategory = SelectFromList(
                            "Please select task category: ",
                            dataManager.Categories
                        );

                        var selectedLabel = SelectFromList(
                            "Please select task label: ",
                            selectedCategory.Labels
                        );

                        var dueDate = AnsiConsole.Prompt(
                            new TextPrompt<DateTime>(
                                "Please enter upcoming task due date and time (in m/d/y hr:min format):"
                            )
                        );

                        TaskData data = new TaskData(
                            dueDate,
                            selectedUser,
                            selectedCategory,
                            selectedLabel,
                            taskStatus
                        );

                        dataModifyer.AddNewTaskData(data);

                        command = AskForInput("Enter submit: ");
                    }

                    if (viewEdit == "view tasks")
                    {
                        var result = Reporter.ShowTasksUpcoming(dataManager.TaskData);
                        Console.WriteLine("Your upcoming tasks are:");
                        foreach (var task in result)
                        {
                            Console.WriteLine(
                                $"- User: {task.User} | {task.Category} |  {task.Label} | Due: {task.DueDate}"
                            );
                        }
                        command = AskForInput("Enter submit: ");
                    }
                }
            } while (command != "submit");
        }
        else if (user == "new user")
        {
            var newUserName = AnsiConsole.Prompt(new TextPrompt<string>("Enter new user's name:"));
            dataModifyer.AddUser(new User(newUserName));
        }
    }

    public static string AskForInput(string message) // for asking for input (refactoring)
    {
        Console.Write(message);
        return Console.ReadLine() ?? "";
    }

    public static T SelectFromList<T>(string title, IEnumerable<T> items) // refactor to pick from list
    {
        return AnsiConsole.Prompt(new SelectionPrompt<T>().Title(title).AddChoices(items));
    }

    public static string SelectFromStrings(string title, params string[] options) // refactoring but picking from different string options
    {
        return AnsiConsole.Prompt(new SelectionPrompt<string>().Title(title).AddChoices(options));
    }
}

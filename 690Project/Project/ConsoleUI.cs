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

    public ConsoleUI()
    {
        dataManager = new DataManager();
    }

    public void Show()
    {
        var user = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Please select mode new user or current user: ")
                .AddChoices("new user", "current user")
        );

        if (user == "current user")
        {
            string command = "";

            do
            {
                var selectedUser = AnsiConsole.Prompt(
                    new SelectionPrompt<User>()
                        .Title("Pleaser select a user:")
                        .AddChoices(dataManager.Users)
                );

                Console.WriteLine("Current user is: " + selectedUser.Name);

                var selectedStatus = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("Please select upcoming tasks or current tasks:")
                        .AddChoices("upcoming", "completed")
                );

                Status taskStatus = new Status(selectedStatus == "completed");

                if (selectedStatus == "completed")
                {
                    var viewEdit = AnsiConsole.Prompt(
                        new SelectionPrompt<string>()
                            .Title("Please select: view tasks or edit tasks:")
                            .AddChoices("view tasks", "edit tasks")
                    );

                    if (viewEdit == "edit tasks")
                    {
                        var updateEdit = AnsiConsole.Prompt(
                            new SelectionPrompt<string>()
                                .Title("Mark previously entered task as complete or add new task ")
                                .AddChoices("update previously entered task", "add new task")
                        );

                        if (updateEdit == "add new task")
                        {
                            string listUpdate = AnsiConsole.Prompt(
                                new SelectionPrompt<string>()
                                    .Title(
                                        "Please select: choose from list or add new task category"
                                    )
                                    .AddChoices(
                                        "add new task or category",
                                        "choose from existing list"
                                    )
                            );

                            if (listUpdate == "choose from existing list")
                            {
                                var selectedCategory = AnsiConsole.Prompt(
                                    new SelectionPrompt<Category>()
                                        .Title("Please select task category: ")
                                        .AddChoices(dataManager.Categories)
                                );

                                var selectedLabel = AnsiConsole.Prompt(
                                    new SelectionPrompt<Label>()
                                        .Title("Please select task label: ")
                                        .AddChoices(selectedCategory.Labels)
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

                                dataManager.AddNewTaskData(data);

                                command = AskForInput("Enter submit: ");
                            }
                            else if (listUpdate == "add new task or category")
                            {
                                var txtUpdate = AnsiConsole.Prompt(
                                    new SelectionPrompt<string>()
                                        .Title(
                                            "Please select: add new category or add new task for existing category"
                                        )
                                        .AddChoices(
                                            "add new category",
                                            "add new task for existing category"
                                        )
                                );

                                if (txtUpdate == "add new category")
                                {
                                    var newCategoryName = AnsiConsole.Prompt(
                                        new TextPrompt<string>("Enter new category name:")
                                    );
                                    dataManager.AddCategory(new Category(newCategoryName));

                                    var newLabelName = AnsiConsole.Prompt(
                                        new TextPrompt<string>("Enter new task for this category:")
                                    );
                                    var addedCategory = dataManager.Categories.Last();
                                    dataManager.AddLabel(new Label(newLabelName), addedCategory);
                                }
                                else if (txtUpdate == "add new task for existing category")
                                {
                                    var selectedCategory = AnsiConsole.Prompt(
                                        new SelectionPrompt<Category>()
                                            .Title("Please select task category: ")
                                            .AddChoices(dataManager.Categories)
                                    );

                                    var newLabelName = AnsiConsole.Prompt(
                                        new TextPrompt<string>("Enter new task for this category:")
                                    );
                                    dataManager.AddLabel(new Label(newLabelName), selectedCategory);
                                }
                            }
                        }
                        else if (updateEdit == "update previously entered task")
                        {
                            var incompleteTasks = Reporter
                                .ShowTasksUpcoming(dataManager.TaskData)
                                .ToList();

                            var selectedUpdate = AnsiConsole.Prompt(
                                new SelectionPrompt<TaskData>()
                                    .Title("Please select task to mark as complete ")
                                    .AddChoices(incompleteTasks)
                            );

                            selectedUpdate.Status.Complete = true;
                            dataManager.SaveAllTasks();
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
                    var viewEdit = AnsiConsole.Prompt(
                        new SelectionPrompt<string>()
                            .Title("Please select: view tasks or edit tasks:")
                            .AddChoices("view tasks", "edit tasks")
                    );
                    if (viewEdit == "edit tasks")
                    {
                        var updateEdit = AnsiConsole.Prompt(
                            new SelectionPrompt<string>()
                                .Title("Mark previously entered task as complete or add new task ")
                                .AddChoices("update previously entered task", "add new task")
                        );
                        var selectedCategory = AnsiConsole.Prompt(
                            new SelectionPrompt<Category>()
                                .Title("Please select task category: ")
                                .AddChoices(dataManager.Categories)
                        );

                        var selectedLabel = AnsiConsole.Prompt(
                            new SelectionPrompt<Label>()
                                .Title("Please select task label: ")
                                .AddChoices(selectedCategory.Labels)
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

                        dataManager.AddNewTaskData(data);

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
            dataManager.AddUser(new User(newUserName));
        }
    }

    public static string AskForInput(string message)
    {
        Console.Write(message);
        return Console.ReadLine() ?? "";
    }
}

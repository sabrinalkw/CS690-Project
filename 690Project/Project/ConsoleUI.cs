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
        var user = ConsoleInputs.SelectFromStrings(
            "Please select mode new user or current user:",
            "new user",
            "current user"
        );

        if (user == "current user")
        {
            string command = "";

            do
            {
                var selectedUser = ConsoleInputs.SelectFromList(
                    "Please select a user:",
                    dataManager.Users
                );

                Console.WriteLine("Current user is: " + selectedUser.Name);

                var selectedStatus = ConsoleInputs.SelectFromStrings(
                    "Please select upcoming tasks or current tasks:",
                    "upcoming",
                    "completed"
                );

                Status taskStatus = new Status(selectedStatus == "completed");

                if (selectedStatus == "completed")
                {
                    var viewEdit = ConsoleInputs.SelectFromStrings(
                        "Please select: view tasks or edit tasks:",
                        "view tasks",
                        "edit tasks"
                    );

                    if (viewEdit == "edit tasks")
                    {
                        var updateEdit = ConsoleInputs.SelectFromStrings(
                            "Mark previously entered task as complete or add new task",
                            "update previously entered task",
                            "add new task"
                        );

                        if (updateEdit == "add new task")
                        {
                            var listUpdate = ConsoleInputs.SelectFromStrings(
                                "Please select: choose from list or add new task category",
                                "add new task or category",
                                "choose from existing list"
                            );

                            if (listUpdate == "choose from existing list")
                            {
                                var selectedCategory = ConsoleInputs.SelectFromList(
                                    "Please select task category: ",
                                    dataManager.Categories
                                );
                                var selectedLabel = ConsoleInputs.SelectFromList(
                                    "Please select task label: ",
                                    selectedCategory.Labels
                                );

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
                            else if (listUpdate == "add new task or category")
                            {
                                var txtUpdate = ConsoleInputs.SelectFromStrings(
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
                                    var selectedCategory = ConsoleInputs.SelectFromList(
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

                            var selectedUpdate = ConsoleInputs.SelectFromList(
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
                        ConsoleInputs.PrintTasks(result, "Your completed tasks are:");

                        command = ConsoleInputs.AskForInput("Enter submit: ");
                    }
                }
                else if (selectedStatus == "upcoming")
                {
                    var viewEdit = ConsoleInputs.SelectFromStrings(
                        "Please select: view tasks or edit tasks:",
                        "view tasks",
                        "edit tasks"
                    );
                    if (viewEdit == "edit tasks")
                    {
                        var updateEdit = ConsoleInputs.SelectFromStrings(
                            "Mark previously entered task as complete or add new task",
                            "update previously entered task",
                            "add new task"
                        );

                        var selectedCategory = ConsoleInputs.SelectFromList(
                            "Please select task category: ",
                            dataManager.Categories
                        );

                        var selectedLabel = ConsoleInputs.SelectFromList(
                            "Please select task label: ",
                            selectedCategory.Labels
                        );

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

                    if (viewEdit == "view tasks")
                    {
                        var result = Reporter.ShowTasksUpcoming(dataManager.TaskData);
                        ConsoleInputs.PrintTasks(result, "Your upcoming tasks are:");

                        command = ConsoleInputs.AskForInput("Enter submit: ");
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
}

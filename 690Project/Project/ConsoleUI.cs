using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq.Expressions;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using Microsoft.VisualBasic;
using Spectre.Console;

namespace Project;
// this class holds the info that guides the UI experince (what choices or informatoin when)
public class ConsoleUI
{
    DataManager dataManager;
    DataModifyer dataModifyer;
    ConsoleQuestions consoleQuestions;

    public ConsoleUI()
    {
        dataManager = new DataManager();
        dataModifyer = new DataModifyer((dataManager));
        consoleQuestions = new ConsoleQuestions(dataManager, dataModifyer);
    }

    public void Show()
    {
        string command = "";

        string user = consoleQuestions.ModeSelect();

        if (user == "current user")
        {
            do
            {
                User selectedUser = consoleQuestions.UserSelect();

                Console.WriteLine("Current user is: " + selectedUser.Name);

                string selectedStatus = consoleQuestions.SelectCompleteUpcoming();

                Status taskStatus = new Status(selectedStatus == "completed");

                if (selectedStatus == "completed")
                {
                    string viewEdit = consoleQuestions.SelectViewEdit();

                    if (viewEdit == "edit tasks")
                    {
                        string updateEdit = consoleQuestions.NewOrUpdate();

                        if (updateEdit == "add new task")
                        {
                            string listUpdate = consoleQuestions.UpdateTaskList();

                            if (listUpdate == "choose from existing list")
                            {
                                Category selectedCategory = consoleQuestions.CategorySelect();

                                Label selectedLabel = consoleQuestions.LabelSelect(
                                    selectedCategory
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
                            if (listUpdate == "add new task or category")
                            {
                                string txtUpdate = consoleQuestions.NewCategoryOrExisting();

                                if (txtUpdate == "add new category")
                                {
                                    string newCategoryName = consoleQuestions.PromptCategoryName();

                                    dataModifyer.AddCategory(new Category(newCategoryName));
                                    string newLabelName = consoleQuestions.NewTaskForCategory();
                                    var addedCategory = dataManager.Categories.Last();
                                    dataModifyer.AddLabel(new Label(newLabelName), addedCategory);
                                }
                                if (txtUpdate == "add new task for existing category")
                                {
                                    Category selectedCategory = consoleQuestions.CategorySelect();

                                    string newLabelName = consoleQuestions.NewTaskForCategory();

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

                            TaskData selectedUpdate = consoleQuestions.SelectFromList(
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

                        command = consoleQuestions.SubmitMethod();
                    }
                }
                if (selectedStatus == "upcoming")
                {
                    string viewEdit = consoleQuestions.SelectViewEdit();

                    if (viewEdit == "edit tasks")
                    {
                        string updateEdit = consoleQuestions.NewOrUpdate();

                        if (updateEdit == "add new task")
                        {
                            string listUpdate = consoleQuestions.UpdateTaskList();

                            if (listUpdate == "choose from existing list")
                            {
                                Category selectedCategory = consoleQuestions.CategorySelect();

                                Label selectedLabel = consoleQuestions.LabelSelect(
                                    selectedCategory
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

                                command = consoleQuestions.SubmitMethod();
                            }
                            if (listUpdate == "add new task or category")
                            {
                                string txtUpdate = consoleQuestions.NewCategoryOrExisting();

                                if (txtUpdate == "add new category")
                                {
                                    string newCategoryName = consoleQuestions.PromptCategoryName();

                                    dataModifyer.AddCategory(new Category(newCategoryName));
                                    string newLabelName = consoleQuestions.NewTaskForCategory();
                                    var addedCategory = dataManager.Categories.Last();
                                    dataModifyer.AddLabel(new Label(newLabelName), addedCategory);
                                }
                                if (txtUpdate == "add new task for existing category")
                                {
                                    Category selectedCategory = consoleQuestions.CategorySelect();

                                    string newLabelName = consoleQuestions.NewTaskForCategory();

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
                        command = consoleQuestions.SubmitMethod();
                    }
                }
            } while (command != "submit");
        }

        if (user == "new user")
        {
            consoleQuestions.NewUserSelect();
        }
    }
}

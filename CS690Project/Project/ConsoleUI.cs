using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq.Expressions;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using Microsoft.VisualBasic;
using Spectre.Console;

namespace Project;

// this class holds the info that guides the UI experince (what choices or informatoin when)
public class ConsoleUI
{
    DataManager dataManager;
    DataModifyer dataModifyer;
    ConsoleQuestions consoleQuestions;
    ConsoleComponets consoleComponets;

    public ConsoleUI()
    {
        dataManager = new DataManager();
        dataModifyer = new DataModifyer((dataManager));
        consoleQuestions = new ConsoleQuestions(dataManager, dataModifyer);
        consoleComponets = new ConsoleComponets(dataManager, dataModifyer, consoleQuestions);
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
                            command = consoleComponets.AddOrUpdateTask(
                                command,
                                selectedUser,
                                taskStatus
                            );
                        }
                        if (updateEdit == "update previously entered task")
                        {
                            consoleComponets.UpdateEnteredTask();
                        }
                    }

                    if (viewEdit == "view tasks")
                    {
                        command = consoleComponets.ShowCompletedTasks();
                    }
                }
                if (selectedStatus == "upcoming")
                {
                    string viewEdit = consoleQuestions.SelectViewEdit();

                    if (viewEdit == "edit tasks")
                    {
                        command = consoleComponets.AddOrUpdateTask(
                            command,
                            selectedUser,
                            taskStatus
                        );
                    }

                    if (viewEdit == "view tasks")
                    {
                        command = consoleComponets.ShowUpcomingTasks();
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

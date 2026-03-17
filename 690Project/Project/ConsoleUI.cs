using System.Data;
using System.Reflection.Emit;
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
        public void Show(){   
            var user = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Please select mode new user or current user: ")
                    .AddChoices("new user", "current user"));

            if(user == "current user"){

             string command = ""; 

                do {
                    var selectedUser = AnsiConsole.Prompt(
                    new SelectionPrompt<User>()
                        .Title("Pleaser select a user:")
                        .AddChoices(dataManager.Users));
                    Console.WriteLine("Current user is: " + selectedUser.Name);

                    var selectedInput = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("Please select upcoming tasks or current tasks:")
                        .AddChoices("incomplete", "complete"));
                    Status selectedStatus = new Status(selectedInput == "complete");

                    var viewEdit = AnsiConsole.Prompt(
                        new SelectionPrompt<string>()
                            .Title("Please select: view tasks or edit tasks:")
                            .AddChoices("view tasks", "edit tasks"));
                     
                    var selectedCategory = AnsiConsole.Prompt(
                        new SelectionPrompt<Category>()
                        .Title("Please select task category: ")
                        .AddChoices(dataManager.Categories));

                    var selectedLabel = AnsiConsole.Prompt(
                        new SelectionPrompt<Label>()
                        .Title("Please select task label: ")
                        .AddChoices(selectedCategory.Labels));

                    var dueDate = AnsiConsole.Prompt(
                        new TextPrompt<DateTime>("Please enter upcoming task due date and time:"));

                    TaskData data = new TaskData(dueDate, selectedUser, selectedCategory, selectedLabel, selectedStatus);
            
                    dataManager.AddNewTaskData(data);

                    command = AskForInput("Enter submit"); 
                } while (command != "submit");   
            }
        }
        public static string AskForInput(string message){
            Console.Write(message);
            return Console.ReadLine();
        }
        
    }

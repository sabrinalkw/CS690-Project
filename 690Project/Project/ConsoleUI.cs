using System.Reflection.Emit;
using Microsoft.VisualBasic;
using Spectre.Console;

namespace Project;
public class ConsoleUI
    {
        FileSaver fileSaver;

        List<Category> categories; 
        List<Label> labels; 

        List<User> users; 

        public ConsoleUI()
        {
            fileSaver = new FileSaver("task-data.txt");

            categories = new List<Category>(); 
            categories.Add(new Category("Food"));
            categories.Add(new Category("Vet"));
            categories.Add(new Category("3"));

            labels = new List<Label>();
            labels.Add(new Label("treats"));
            labels.Add(new Label("kibble"));
            labels.Add(new Label("vaccines"));
            labels.Add(new Label("check up"));

            // add different labels to different categories 
            categories[0].Labels.Add(labels[0]);
            categories[0].Labels.Add(labels[1]);
            categories[1].Labels.Add(labels[2]);
            categories[1].Labels.Add(labels[3]);

            users = new List<User>();
            users.Add(new User("Jane"));
            users.Add(new User("John"));
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
                        .AddChoices(users));
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
                        .AddChoices(categories));

                    var selectedLabel = AnsiConsole.Prompt(
                        new SelectionPrompt<Label>()
                        .Title("Please select task label: ")
                        .AddChoices(selectedCategory.Labels));
                    
                    string selectedDate = AskForInput("Please enter upcoming task due date and time:");
                    DateTime dueDate;
                    while (!DateTime.TryParse(selectedDate, out dueDate))
                        {
                            selectedDate = AskForInput("Invalid format. Please enter a valid date and time (e.g., 2026-03-17 14:30):");
                        }

                    TaskData data = new TaskData(dueDate, selectedUser, selectedCategory, selectedLabel, selectedStatus);
            
                    fileSaver.AppendData(data);

                    command = AskForInput("Enter submit"); 
                } while (command != "submit");   
            }
        }
        public static string AskForInput(string message){
            Console.Write(message);
            return Console.ReadLine();
        }
        
    }

namespace Project;
public class ConsoleUI
    {
        FileSaver fileSaver;
        public ConsoleUI()
        {
            fileSaver = new FileSaver("task-data.txt");
        }
        public void Show(){   
            string user = AskForInput("Please select mode new user or current user: ");

            if(user == "current user"){

             string command = ""; 

                do {
                    string task = AskForInput("Please select upcoming tasks or current tasks:");
                    
                    string viewEdit = AskForInput("Please select: view tasks or edit tasks:");
                    
                    string taskCategory = AskForInput("Please select task category:");
                    
                    string dateTime = AskForInput("Please enter upcoming task due date and time:");
            
                    fileSaver.AppendLine(dateTime + "\n");

                    command = AskForInput("Enter submit"); 
                } while (command != "submit");   
            }
        }
        public static string AskForInput(string message){
            Console.Write(message);
            return Console.ReadLine();
        }
        
    }

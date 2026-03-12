namespace Project;

using System.IO;
class Program
{
    static void Main(string[] args)
    {
        while (true){
            Console.WriteLine("Please select mode: new user or current user ");
            string user = Console.ReadLine();

            if(user == "current user"){

             string command = ""; 

                do {
                    Console.WriteLine("Please select: upcoming tasks or current tasks");
                    string task = Console.ReadLine();
                    Console.WriteLine ("Please select: view tasks or edit tasks");
                    string viewEdit = Console.ReadLine();
                    Console.WriteLine("Please select task category:");
                    string taskCategory = Console.ReadLine();
                    Console.WriteLine("Please enter upcoming task due date and time");
                    string dateTime = Console.ReadLine();
            
                    File.AppendAllText("task-Data.txt", dateTime + "\n");
                    Console.WriteLine("Enter submit"); 
                    command = Console.ReadLine();
                } while (command != "submit");
            
            }
        }
    }
}

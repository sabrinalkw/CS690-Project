using Spectre.Console;

namespace Project;

class ConsoleInputs //THis class has the blueprints for the ConsoleQuesitons Page, where it outlines methods for pikcing something form a list, or from a set for strings or for input
{
    public static DateTime GetDate()
    {
        return AnsiConsole.Prompt(
            new TextPrompt<DateTime>(
                "Please enter upcoming task due date and time (in m/d/y hr:min format):"
            )
        );
    }

    // make these into nnew classes
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

    public static void PrintTasks(IEnumerable<TaskData> tasks, string title) // refactoring for viewing tasks (completed or not)
    {
        Console.WriteLine(title);
        foreach (var task in tasks)
        {
            Console.WriteLine(
                $"- User: {task.User} | {task.Category} | {task.Label} | Due: {task.DueDate}"
            );
        }
    }
}

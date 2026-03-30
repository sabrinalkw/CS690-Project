using Spectre.Console;

namespace Project;

// This class holds the string inputs based on the emplatesd from ConsoleInputs page for refactoring and to take editing the string inputs out of the ConsoleUI
public class ConsoleQuestions
{
    DataManager dataManager;

    DataModifyer dataModifyer;

    public ConsoleQuestions(DataManager dataManager, DataModifyer dataModifyer)
    {
        this.dataManager = dataManager;
        this.dataModifyer = dataModifyer;
    }

    public string SubmitMethod()
    {
        return ConsoleInputs.AskForInput("Enter submit: ");
    }

    public void NewUserSelect()
    {
        var newUserName = AnsiConsole.Prompt(new TextPrompt<string>("Enter new user's name:"));
        dataModifyer.AddUser(new User(newUserName));
    }

    public string NewTaskForCategory()
    {
        return AnsiConsole.Prompt(new TextPrompt<string>("Enter new task for this category:"));
    }

    public string PromptCategoryName()
    {
        return AnsiConsole.Prompt(new TextPrompt<string>("Enter new category name:"));
    }

    public string NewCategoryOrExisting()
    {
        return ConsoleInputs.SelectFromStrings(
            "Please select: add new category or add new task for existing category",
            "add new category",
            "add new task for existing category"
        );
    }

    public TaskData SelectFromList(List<TaskData> incompleteTasks)
    {
        return ConsoleInputs.SelectFromList(
            "Please select task to mark as complete ",
            incompleteTasks
        );
    }

    public void NewOrEntered()
    {
        var updateEdit = ConsoleInputs.SelectFromStrings(
            "Mark previously entered task as complete or add new task",
            "update previously entered task",
            "add new task"
        );
    }

    public Label LabelSelect(Category selectedCategory)
    {
        return ConsoleInputs.SelectFromList("Please select task label: ", selectedCategory.Labels);
    }

    public string UpdateTaskList()
    {
        return ConsoleInputs.SelectFromStrings(
            "Please select: choose from list or add new task category",
            "add new task or category",
            "choose from existing list"
        );
    }

    public string NewOrUpdate()
    {
        return ConsoleInputs.SelectFromStrings(
            "Mark previously entered task as complete or add new task",
            "update previously entered task",
            "add new task"
        );
    }

    public string SelectViewEdit()
    {
        return ConsoleInputs.SelectFromStrings(
            "Please select: view tasks or edit tasks:",
            "view tasks",
            "edit tasks"
        );
    }

    public string SelectCompleteUpcoming()
    {
        return ConsoleInputs.SelectFromStrings(
            "Please select upcoming tasks or current tasks:",
            "upcoming",
            "completed"
        );
    }

    public User UserSelect()
    {
        return ConsoleInputs.SelectFromList("Please select a user:", dataManager.Users);
    }

    public string ModeSelect()
    {
        return ConsoleInputs.SelectFromStrings(
            "Please select mode new user or current user:",
            "new user",
            "current user"
        );
    }

    public Category CategorySelect()
    {
        return ConsoleInputs.SelectFromList(
            "Please select task category: ",
            dataManager.Categories
        );
    }
}

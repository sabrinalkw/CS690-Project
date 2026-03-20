
using Microsoft.VisualBasic;

namespace Project;

public class User {
    public string Name { get; }

    public User(string name) {
        this.Name = name;
    }

     public override string ToString()
    {
        return this.Name;
    }
    
}

public class Label
{
    public string Name { get; }

    public Label(string name) {
        this.Name = name;
    }

    public override string ToString()
    {
        return this.Name;
    }
    
}

public class Category
{
    public string Name { get; }
    public List<Label> Labels { get; }
    public Category (string name) {
        this.Name = name;
        this.Labels = new List<Label>();
    }
    public override string ToString()
    {
        return this.Name;
    }
}

 

public class Status
{
    public bool Complete { get; set; }
    public Status(bool complete) {
        this.Complete = complete;
    }
public bool Incomplete => !Complete;
 public override string ToString()
    {
        return Complete ? "complete" : "incomplete";
    }
}

public class TaskData
{
    public DateTime DueDate { get; }
    public User User { get; }
    public Category Category { get; }

    public Label Label { get; }
    public Status Status { get; }
    public TaskData(DateTime dueDate, User user, Category category, Label label, Status status) {
        this.DueDate = dueDate;
        this.User = user;
        this.Category = category;
        this.Label = label; 
        this.Status = status;
    }
    public override string ToString()
    {
        return $"{User.Name} | {Category.Name} | {Label.Name} | Due: {DueDate:g} | {(Status.Complete ? "✔" : "✘")}";
    }
}





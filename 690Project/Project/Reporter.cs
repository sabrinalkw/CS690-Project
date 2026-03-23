namespace Project;
 
public class Reporter
{
   public static List<TaskData> ShowTasksCompleted(List<TaskData> data)
{
    return data
        .Where(t => t.Status.Complete)
        .OrderBy(t => t.DueDate)
        .ToList();
}

public static List<TaskData> ShowTasksUpcoming(List<TaskData> data)
{
    return data
        .Where(t => t.Status.Incomplete)
        .OrderBy(t => t.DueDate)
        .ToList();
}
}
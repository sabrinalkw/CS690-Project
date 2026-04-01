namespace CS690Project.Tests;

using System.Diagnostics;
using Project;

public class ReporterTests
{
    Reporter reporter;
    List<TaskData> sampleData;
    public ReporterTests() {
        sampleData = new List<TaskData>();
    }
    
    [Fact]
    public void Test_ShowTasksUpcoming()
    {
       User sampleUser1 = new User("SampleUser1");
        Category sampleCategory1 = new Category("SampleCategory1");
        Label sampleLabel1 = new Label("SampleLabel1");
        Status sampleStatus1 = new Status(false);
        TaskData sampleTaskData1 = new TaskData(
            DateTime.Parse("1/1/2001 1:00"),
            sampleUser1,
            sampleCategory1,
            sampleLabel1,
            sampleStatus1
        );
        sampleData.Add(sampleTaskData1);

        User sampleUser2 = new User("SampleUser2");
        Category sampleCategory2 = new Category("SampleCategory2");
        Label sampleLabel2 = new Label("SampleLabel2");
        Status sampleStatus2 = new Status(false);
        TaskData sampleTaskData2 = new TaskData(
            DateTime.Parse("2/2/2002 2:00"),
            sampleUser2,
            sampleCategory2,
            sampleLabel2,
            sampleStatus2
        );
        sampleData.Add(sampleTaskData2);


         User sampleUser3 = new User("SampleUser3");
        Category sampleCategory3 = new Category("SampleCategory3");
        Label sampleLabel3 = new Label("SampleLabel3");
        Status sampleStatus3 = new Status(false);
        TaskData sampleTaskData3 = new TaskData(
            DateTime.Parse("3/3/2003 3:00"),
            sampleUser3,
            sampleCategory3,
            sampleLabel3,
            sampleStatus3
        );
        sampleData.Add(sampleTaskData3);

        User sampleUser4 = new User("SampleUser4");
        Category sampleCategory4 = new Category("SampleCategory4");
        Label sampleLabel4 = new Label("SampleLabel4");
        Status sampleStatus4 = new Status(true);
        TaskData sampleTaskData4 = new TaskData(
            DateTime.Parse("4/4/2004 4:00"),
            sampleUser4,
            sampleCategory4,
            sampleLabel4,
            sampleStatus4
        );
        sampleData.Add(sampleTaskData4);

        var result = Reporter.ShowTasksUpcoming(sampleData);
        var expected = new List<TaskData> { sampleTaskData1, sampleTaskData2, sampleTaskData3 };
        Assert.Equal(expected, result);
    }

}

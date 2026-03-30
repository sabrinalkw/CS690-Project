namespace CS690Project.Tests;

using System.Diagnostics;
using Project;

public class FileSaverTests
{
    FileSaver fileSaverOne;
    FileSaver fileSaverTwo;
    string testFileOne;

    string testFileTwo;

    public FileSaverTests()
    {
        testFileOne = "test-doc-one.txt";
        testFileTwo = "test-doc-two.txt";

        fileSaverOne = new FileSaver(testFileOne);
        fileSaverTwo = new FileSaver(testFileTwo);
    }

    [Fact]
    public void Test_FileSave_Append() // testing to make sure that test is being appended to file, and then a new line made
    {
        fileSaverOne.AppendLine("Hello test user");
        var contentFromFile = File.ReadAllText(testFileOne);
        Assert.Equal("Hello test user" + Environment.NewLine, contentFromFile);
    }

    [Fact]
    public void Test_FileSave_AppendData() // testing to make sure that data needed is being appended to file then a new line (see FileSaver in Program as well)
    {
        User sampleUser = new User("SampleUser");
        Category sampleCategory = new Category("SampleCategory");
        Label sampleLabel = new Label("SampleLabel");
        Status sampleStatus = new Status(true);
        TaskData sampleData = new TaskData(
            DateTime.Parse("1/1/2001 1:00"),
            sampleUser,
            sampleCategory,
            sampleLabel,
            sampleStatus
        );

        fileSaverTwo.AppendData(sampleData);
        var contentFromFile = File.ReadAllText(testFileTwo);
        Assert.Equal(
            "SampleUser-SampleCategory-SampleLabel-1/1/2001 1:00:00-complete" + Environment.NewLine,
            contentFromFile
        );
    }
}

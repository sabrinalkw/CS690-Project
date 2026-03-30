namespace CS690Project.Tests;

using Project;
public class FileSaverTests
{
    FileSaver fileSaver;
    string testFileName;

    public FileSaverTests()
    {
        testFileName = "test-doc.txt";
        fileSaver = new FileSaver(testFileName);

    }
    [Fact]
    public void Test_FileSave_Append()
    {
        fileSaver.AppendLine("Hello test user");
        var contentFromFile = File.ReadAllText(testFileName);
        Assert.Equal("Hello test user" + Environment.NewLine, contentFromFile);
    }
}

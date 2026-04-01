namespace CS690Project.Tests;

using System.Reflection;
using Project;

public class DataModifyerTests
{
    DataModifyer dataModifyer;
    DataManager dataManager;

    public DataModifyerTests()
    {
        File.WriteAllText(
            "users.txt",
            "TestUser1"
                + Environment.NewLine
                + "TestUser2"
                + Environment.NewLine
                + "TestUser3"
                + Environment.NewLine
                + "TestUser4"
        );
 File.WriteAllText("categories.txt", ""); 
    File.WriteAllText("labels.txt", "");  
        var dataManager = new DataManager();
    dataModifyer = new DataModifyer(dataManager);

    }

    [Fact]
    public void Test_AddUser() //testing the functions to add a user to the system 
    {
        Assert.Equal(4, dataModifyer.Users.Count);
        dataModifyer.AddUser(new User("TestUser5"));
        Assert.Equal(5, dataModifyer.Users.Count);
    }
}

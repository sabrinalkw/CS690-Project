namespace Project;

using System.IO;

// this class is for saving the data to the txt files
public class FileSaver
{
    string fileName;

    public FileSaver(string fileName)
    {
        this.fileName = fileName;
        if (!File.Exists(this.fileName))
        {
            File.Create(this.fileName).Close();
        }
    }

    public void AppendLine(string line) // see test in FileSaverTests.cs
    {
        File.AppendAllText(this.fileName, line + Environment.NewLine);
    }

    public void AppendData(TaskData data) // see test in FileSaverTests.cs
    {
        File.AppendAllText(
            this.fileName,
            data.User
                + "-"
                + data.Category
                + "-"
                + data.Label
                + "-"
                + data.DueDate.ToString("M/d/yyyy H:mm:ss")
                + "-"
                + data.Status
                + Environment.NewLine
        );
    }
}

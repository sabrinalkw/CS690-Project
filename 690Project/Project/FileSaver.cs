namespace Project;

using System.IO;

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

    public void AppendLine(string line)
    {
        File.AppendAllText(this.fileName, line + Environment.NewLine);
    }

    public void AppendData(TaskData data)
    {
        File.AppendAllText(
            this.fileName,
            data.User
                + "-"
                + data.Category
                + "-"
                + data.Label
                + "-"
                + data.DueDate
                + "-"
                + data.Status
                + Environment.NewLine
        );
    }
}

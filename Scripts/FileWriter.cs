using System.IO;
using UnityEngine;

public class FileWriter
{
    private string _path;

    public FileWriter(string path)
    {
        _path = path;
    }


    public void Write(byte[] data)
    {
        try
        {
            using (FileStream fs = new FileStream(_path, FileMode.Create, FileAccess.Write))
            {
                fs.Write(data, 0, data.Length);
                fs.Flush();
            }
        }
        catch (IOException ex)
        {
            Debug.LogError(ex);
        }
    }
}
    
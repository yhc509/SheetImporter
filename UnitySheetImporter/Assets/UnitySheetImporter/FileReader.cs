using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class FileReader
{
    private string _path;

    public FileReader(string path)
    {
        _path = path;
    }


    public byte[] Read()
    {
        try
        {
            byte[] bytes;
            using (FileStream fs = new FileStream(_path, FileMode.Open, FileAccess.Read))
            {
                bytes = new byte[fs.Length];
                fs.Read(bytes, 0, (int) fs.Length);
                fs.Close();
            }

            return bytes;
        }
        catch (IOException ex)
        {
        }

        return null;
    }
}
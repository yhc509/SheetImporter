using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TableManager
{
    public TableInfo<int, test> TestTable = new TableInfo<int, test>();

    public static string BinaryDirectoryPath
    {
        get {return Path.Combine(Application.dataPath, "Data"); }
    }

    public void Initialize()
    {
        TestTable.Initailize(Path.Combine(BinaryDirectoryPath, "test.bytes"));
    }
}

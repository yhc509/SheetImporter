using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using UnityEditor;
using UnityEngine;

public static class XLSImporter
{
    public static string CodeDirectoryPath
    {
        get { return Path.Combine(Application.dataPath, "Scripts", "Data"); }
    }

    public static string BinaryDirectoryPath
    {
        get {return Path.Combine(Application.dataPath, "Data"); }
    }
    
    public static void Import(string path)
    {
        var sheetName = Path.GetFileName(path).Replace(".xlsx", string.Empty);
        var table = ReadTable(sheetName, path);

        if (!Directory.Exists(CodeDirectoryPath))
            Directory.CreateDirectory(CodeDirectoryPath);
        if (!Directory.Exists(BinaryDirectoryPath))
            Directory.CreateDirectory(BinaryDirectoryPath);
        
        GenerateBinary(sheetName, table);
        GenerateClassCode(sheetName, table);
    }
    
    private static InfoTable ReadTable(string sheetName, string path)
    {
        InfoTable infoTable = new InfoTable();
        infoTable.tableName = sheetName;
        
        XSSFWorkbook workbook;
        using (FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read))
        {
            workbook = new XSSFWorkbook(file);
        }

        ISheet sheet = workbook.GetSheetAt(0);
        var typeRow = sheet.GetRow(0);
        var keyRow = sheet.GetRow(1);

        string tempStr;

        for (int i = 0; i < typeRow.Cells.Count; i++)
        {
            var columeName = keyRow.Cells[i].StringCellValue;
            var columnType = typeRow.Cells[i].StringCellValue;
            if (columnType == "#") continue;
            infoTable.types.Add(columeName, columnType);
            Debug.Log(columeName);
            Debug.Log(columnType);

            List<object> dataList = new List<object>();
            for (int j = 2; j <= sheet.LastRowNum; j++)
            {
                var dataRow = sheet.GetRow(j);
                var dataCell = dataRow.Cells[i];
                switch (columnType)
                {
                    case "short":
                        dataList.Add((short) dataCell.NumericCellValue);
                        break;
                    case "int":
                        dataList.Add((int) dataCell.NumericCellValue);
                        break;
                    case "long":
                        dataList.Add((long) dataCell.NumericCellValue);
                        break;
                    case "float":
                        dataList.Add((float) dataCell.NumericCellValue);
                        break;
                    case "bool":
                        dataList.Add((bool) dataCell.BooleanCellValue);
                        break;
                    case "string":
                        dataList.Add(dataCell.StringCellValue);
                        break;
                    case "short[]":
                        tempStr = dataCell.StringCellValue;
                        if (tempStr.Last() != ';') tempStr += ";";
                        var shortSplitStr = tempStr.Split(';');
                        short[] arrShort = new short[shortSplitStr.Length];
                        for (int c = 0; c < shortSplitStr.Length; c++)
                        {
                            arrShort[c] = default(int);
                            short.TryParse(shortSplitStr[c], out arrShort[c]);
                        }
                        dataList.Add(arrShort);
                        break;
                    case "int[]":
                        tempStr = dataCell.StringCellValue;
                        if (tempStr.Last() != ';') tempStr += ";";
                        var intSplitStr = tempStr.Split(';');
                        int[] arrInt = new int[intSplitStr.Length];
                        for (int c = 0; c < intSplitStr.Length; c++)
                        {
                            arrInt[c] = default(int);
                            int.TryParse(intSplitStr[c], out arrInt[c]);
                        }
                        dataList.Add(arrInt);
                        break;
                    case "float[]":
                        tempStr = dataCell.StringCellValue;
                        if (tempStr.Last() != ';') tempStr += ";";
                        var floatSplitStr = tempStr.Split(';');
                        float[] arrFloat = new float[floatSplitStr.Length];
                        for (int c = 0; c < floatSplitStr.Length; c++)
                        {
                            arrFloat[c] = default(float);
                            float.TryParse(floatSplitStr[c], out arrFloat[c]);
                        }
                        dataList.Add(arrFloat);
                        break;
                    case "bool[]":
                        tempStr = dataCell.StringCellValue;
                        if (tempStr.Last() != ';') tempStr += ";";
                        var boolSplitStr = tempStr.Split(';');
                        bool[] arrBool = new bool[boolSplitStr.Length];
                        for (int c = 0; c < boolSplitStr.Length; c++)
                        {
                            arrBool[c] = default(bool);
                            bool.TryParse(boolSplitStr[c], out arrBool[c]);
                        }
                        dataList.Add(arrBool);
                        break;
                    case "string[]":
                        tempStr = dataCell.StringCellValue;
                        if (tempStr.Last() != ';') tempStr += ";";
                        var stringSplitStr = tempStr.Split(';');
                        dataList.Add(stringSplitStr);
                        break;
                    case "long[]":
                        tempStr = dataCell.StringCellValue;
                        if (tempStr.Last() != ';') tempStr += ";";
                        var longSplitStr = tempStr.Split(';');
                        long[] arrLong = new long[longSplitStr.Length];
                        for (int c = 0; c < longSplitStr.Length; c++)
                        {
                            arrLong[c] = default(long);
                            long.TryParse(longSplitStr[c], out arrLong[c]);
                        }
                        dataList.Add(arrLong);
                        break;
                }
            }
            infoTable.columns.Add(columeName, dataList);
        }

        return infoTable;
    }

    private static void GenerateBinary(string sheetName, InfoTable infoTable)
    {
        try
        {
            var filePath = Path.Combine(BinaryDirectoryPath, sheetName + ".bytes");

            var fw = new FileWriter(filePath);
            var bytes = infoTable.Serialize();
            fw.Write(bytes);
            
            var dataPathUri = new System.Uri(Application.dataPath);

            var relativeUri = dataPathUri.MakeRelativeUri(new System.Uri(filePath));
            var relativePath = System.Uri.UnescapeDataString(relativeUri.ToString());
            AssetDatabase.ImportAsset(relativePath, ImportAssetOptions.ForceUpdate);
        }
        catch (IOException e)
        {
            throw e;
        }
    }
    
    private static void GenerateClassCode(string sheetName, InfoTable infoTable)
    {
        var text = WriteCode(sheetName, infoTable);
        string fileName = sheetName + ".cs";
        
        var filePath = Path.Combine(CodeDirectoryPath, fileName);
        using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
        {
            using (StreamWriter writer = new StreamWriter(fs, Encoding.UTF8))
            {
                writer.Write(text.ToString());
                writer.Flush();
            }
        }

        var dataPathUri = new Uri(Application.dataPath);

        var relativeUri = dataPathUri.MakeRelativeUri(new Uri(filePath));
        var relativePath = Uri.UnescapeDataString(relativeUri.ToString());
        AssetDatabase.ImportAsset(relativePath, ImportAssetOptions.ForceUpdate);
    }
    
    private static string WriteCode(string sheetName, InfoTable infoTable)
    {
        int depth = 0;
        StringBuilder sb = new StringBuilder();
        sb.Append('\t', depth).AppendLine("using System;");
        sb.Append('\t', depth).AppendLine("");
	    
	    
        sb.Append('\t', depth).AppendLine("[Serializable]");
        sb.Append('\t', depth).AppendLine($"public class {sheetName}");
        sb.Append('\t', depth).AppendLine("{");
        depth++;

        foreach (var type in infoTable.types)
        {
            string columnName = type.Key;
            string typeName = type.Value;
		    
            sb.Append('\t', depth).AppendLine($"public {typeName} {columnName};");
        }
        sb.Append('\t', depth).AppendLine("");
        depth--;
        
        sb.Append('\t', depth).AppendLine("}");

        return sb.ToString();
    }

}

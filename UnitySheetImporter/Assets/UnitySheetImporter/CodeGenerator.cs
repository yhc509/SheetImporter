using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using UnityEditor;
using UnityEngine;

public class CodeGenerator
{
	private string _sheetName;
	private string _sheetRowName;
	private string _sheetColName;
	private string _path;

	private StringBuilder sb = new StringBuilder();
	private int _depth = 0;
	
	public CodeGenerator(string targetPath, string sheetName)
	{
		_sheetName = sheetName;
		_sheetRowName = sheetName + "Row";
		_sheetColName = sheetName + "Col";
		_path = Path.Combine(targetPath, sheetName + ".cs");
	}
	
    public void Generate(string sheetPath) 
    {
	    XSSFWorkbook workbook;
            
	    using (FileStream file = new FileStream(sheetPath, FileMode.Open, FileAccess.Read))
	    {
		    workbook = new XSSFWorkbook(file);
	    }

	    ISheet sheet = workbook.GetSheetAt(0);
	    
	    var typeRow = sheet.GetRow(0);
	    var keyRow = sheet.GetRow(1);
	    
        WriteHeader();
        WriteSheet(typeRow, keyRow);
        WriteSheetRow(typeRow, keyRow);

        Save();
    }

    private void WriteHeader()
    {
	    sb.Append('\t', _depth).AppendLine("using System;");
	    sb.Append('\t', _depth).AppendLine("using System.Linq;");
	    sb.Append('\t', _depth).AppendLine("");
    }

    private void WriteSheet(IRow typeRow, IRow keyRow)
    {
/*
[Serializable]
public class TestSheet : Sheet
{
	public SheetCol<string> a;
	public SheetCol<int> b;
	public SheetCol<float> c;
	public SheetCol<bool> d;
	
	public TestSheet()
	{
		a = new SheetCol<string>(this, "a");
		b = new SheetCol<int>(this, "b");
		c = new SheetCol<float>(this, "c");
		d = new SheetCol<bool>(this, "d");
	}
	
	public new TestSheetRow[] Rows
	{
		get { return rows.Select(x => x as TestSheetRow).ToArray();  }
	}
	
	public new TestSheetRow this[int rowNum]
	{
		get { return rows[rowNum] as TestSheetRow; }
	}
	
}
*/
	    
	    sb.Append('\t', _depth).AppendLine("[Serializable]");
	    sb.Append('\t', _depth).AppendLine($"public class {_sheetName} : Sheet");
	    sb.Append('\t', _depth).AppendLine("{");
	    _depth++;

	    // variables
	    for (int i = 0; i < typeRow.Cells.Count; i++)
	    {
		    string typeName = typeRow.Cells[i].StringCellValue;
		    string keyName = keyRow.Cells[i].StringCellValue;
		    sb.Append('\t', _depth).AppendLine($"public SheetCol<{typeName}> {keyName};");
	    }
	    sb.Append('\t', _depth).AppendLine("");
	    
	    // constructor
	    sb.Append('\t', _depth).AppendLine($"public {_sheetName}()");
	    sb.Append('\t', _depth).AppendLine("{");
	    _depth++;
	    
	    for (int i = 0; i < typeRow.Cells.Count; i++)
	    {
		    string typeName = typeRow.Cells[i].StringCellValue;
		    string keyName = keyRow.Cells[i].StringCellValue;
		    sb.Append('\t', _depth).AppendLine(
			    $"{keyName} = new SheetCol<{typeName}>(this, \"{keyName}\");"
		    );
	    }
	    _depth--;
	    sb.Append('\t', _depth).AppendLine("}");
	    sb.Append('\t', _depth).AppendLine("");
	    
	    // methods
	    sb.Append('\t', _depth).AppendLine($"public new {_sheetRowName}[] Rows");
	    sb.Append('\t', _depth).AppendLine("{");
	    _depth++;

	    sb.Append('\t', _depth).AppendLine($"get {{ return rows.Select(x => x as {_sheetRowName}).ToArray();  }}");
	    
	    _depth--;
	    sb.Append('\t', _depth).AppendLine("}");
	    sb.Append('\t', _depth).AppendLine("");
	    
	    sb.Append('\t', _depth).AppendLine(
		    $"public new {_sheetRowName} this[int rowNum]");
	    sb.Append('\t', _depth).AppendLine("{");
	    _depth++;
	    
	    sb.Append('\t', _depth).AppendLine($"get {{ return rows[rowNum] as {_sheetRowName}; }}");
	    
	    _depth--;
	    sb.Append('\t', _depth).AppendLine("}");
	    sb.Append('\t', _depth).AppendLine("");
	    
	    _depth--;
	    sb.Append('\t', _depth).AppendLine("}");
    }

    private void WriteSheetRow(IRow typeRow, IRow keyRow)
    {
/*
[Serializable]
public class TestSheetRow : SheetRow
{
    public string a;
    public string b;
    public string c;
}
*/
	    sb.Append('\t', _depth).AppendLine("[Serializable]");
	    sb.Append('\t', _depth).AppendLine($"public class {_sheetRowName} : SheetRow");
	    sb.Append('\t', _depth).AppendLine("{");
	    _depth++;

	    for (int i = 0; i < typeRow.Cells.Count; i++)
	    {
		    string typeName = typeRow.Cells[i].StringCellValue;
		    string keyName = keyRow.Cells[i].StringCellValue;
		    sb.Append('\t', _depth).AppendLine($"public {typeName} {keyName};");
	    }
	    
	    _depth--;
	    sb.Append('\t', _depth).AppendLine("}");
    }

    private void Save()
    {
	    try
	    {
		    var filePath = Path.Combine(Application.dataPath, _path);
		    using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
		    {
			    using (StreamWriter writer = new StreamWriter(fs, Encoding.UTF8))
			    {
				    writer.Write(sb.ToString());
				    writer.Flush();
			    }
		    }

		    var dataPathUri = new Uri(Application.dataPath);

		    var relativeUri = dataPathUri.MakeRelativeUri(new Uri(filePath));
		    var relativePath = Uri.UnescapeDataString(relativeUri.ToString());
		    AssetDatabase.ImportAsset(relativePath, ImportAssetOptions.ForceUpdate);
	    }
	    catch (Exception e)
	    {
		    throw e;
	    }
    }

}

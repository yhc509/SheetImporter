using System;
using System.Linq;

[Serializable]
public class SampleSheet : Sheet
{
	public SheetCol<string> stringColumn;
	public SheetCol<int> intColumn;
	public SheetCol<float> floatColumn;
	public SheetCol<bool> boolColumn;
	
	public SampleSheet()
	{
		stringColumn = new SheetCol<string>(this, "stringColumn");
		intColumn = new SheetCol<int>(this, "intColumn");
		floatColumn = new SheetCol<float>(this, "floatColumn");
		boolColumn = new SheetCol<bool>(this, "boolColumn");
	}
	
	public new SampleSheetRow[] Rows
	{
		get { return rows.Select(x => x as SampleSheetRow).ToArray();  }
	}
	
	public new SampleSheetRow this[int rowNum]
	{
		get { return rows[rowNum] as SampleSheetRow; }
	}
	
}
[Serializable]
public class SampleSheetRow : SheetRow
{
	public string stringColumn;
	public int intColumn;
	public float floatColumn;
	public bool boolColumn;
}

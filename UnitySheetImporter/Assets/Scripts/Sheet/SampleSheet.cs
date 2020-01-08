using System;
using System.Linq;

[Serializable]
public class SampleSheet : Sheet
{
	public SheetCol<string> stringColumn;
	public SheetCol<int> intColumn;
	public SheetCol<float> floatColumn;
	public SheetCol<bool> boolColumn;
	public SheetCol<long> longColumn;
	public SheetCol<string[]> stringArrColumn;
	public SheetCol<int[]> intArrColumn;
	public SheetCol<float[]> floatArrColumn;
	public SheetCol<bool[]> boolArrColumn;
	public SheetCol<long[]> longArrColumn;
	
	public SampleSheet()
	{
		stringColumn = new SheetCol<string>(this, "stringColumn");
		intColumn = new SheetCol<int>(this, "intColumn");
		floatColumn = new SheetCol<float>(this, "floatColumn");
		boolColumn = new SheetCol<bool>(this, "boolColumn");
		longColumn = new SheetCol<long>(this, "longColumn");
		stringArrColumn = new SheetCol<string[]>(this, "stringArrColumn");
		intArrColumn = new SheetCol<int[]>(this, "intArrColumn");
		floatArrColumn = new SheetCol<float[]>(this, "floatArrColumn");
		boolArrColumn = new SheetCol<bool[]>(this, "boolArrColumn");
		longArrColumn = new SheetCol<long[]>(this, "longArrColumn");
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
	public long longColumn;
	public string[] stringArrColumn;
	public int[] intArrColumn;
	public float[] floatArrColumn;
	public bool[] boolArrColumn;
	public long[] longArrColumn;
}

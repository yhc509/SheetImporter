using System;
using System.Linq;

[Serializable]
public class Sample2Sheet : Sheet
{
	public SheetCol<string> stringColumn;
	public SheetCol<int> intColumn;
	public SheetCol<float> floatColumn;
	public SheetCol<bool> boolColumn;
	public SheetCol<string[]> stringArrColumn;
	public SheetCol<int[]> intArrColumn;
	public SheetCol<float[]> floatArrColumn;
	public SheetCol<bool[]> boolArrColumn;
	
	public Sample2Sheet()
	{
		stringColumn = new SheetCol<string>(this, "stringColumn");
		intColumn = new SheetCol<int>(this, "intColumn");
		floatColumn = new SheetCol<float>(this, "floatColumn");
		boolColumn = new SheetCol<bool>(this, "boolColumn");
		stringArrColumn = new SheetCol<string[]>(this, "stringArrColumn");
		intArrColumn = new SheetCol<int[]>(this, "intArrColumn");
		floatArrColumn = new SheetCol<float[]>(this, "floatArrColumn");
		boolArrColumn = new SheetCol<bool[]>(this, "boolArrColumn");
	}
	
	public new Sample2SheetRow[] Rows
	{
		get { return rows.Select(x => x as Sample2SheetRow).ToArray();  }
	}
	
	public new Sample2SheetRow this[int rowNum]
	{
		get { return rows[rowNum] as Sample2SheetRow; }
	}
	
}
[Serializable]
public class Sample2SheetRow : SheetRow
{
	public string stringColumn;
	public int intColumn;
	public float floatColumn;
	public bool boolColumn;
	public string[] stringArrColumn;
	public int[] intArrColumn;
	public float[] floatArrColumn;
	public bool[] boolArrColumn;
}

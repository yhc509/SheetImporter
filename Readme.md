# UnitySheetImporter
- developed at Unity 2019.1.8 f1.

## Features
1. Drag Excel(.xlsx) file to project.
2. UnitySheetImporter will generate binary(.bytes) and code(.cs)

## Support types
- [x] int
- [x] float
- [x] bool
- [x] string

## Todo
- Support binary loader.
- Support more types. (int[], float[], bool[], string[])
- Add edit field for binary & code target directory.
- Measure data access time.

## Example

### Original Excel Table (Sample.xlsx)
- first row is type of column.
- second row is name of column.

| **string**       | **int**       | **float**       | **bool**       |
|--------------|-----------|-------------|------------|
| **stringColumn** | **intColumn** | **floatColumn** | **boolColumn** |
|--------------|-----------|-------------|------------|
| row1         | 1         | 1.1         | TRUE       |
| row2         | 10        | 3.14        | FALSE      |

### Generated Code file (SampleSheet.cs) 
```csharp
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
```

### Using Example
```csharp
// file load (I will support easy loader)
var bytePath = Application.dataPath + "/Data/Sample.bytes";
var fr = new FileReader(bytePath);
var bytes = fr.Read();

// deserialize
var sheet = bytes.Deserialize<SampleSheet>();

// access by column
Debug.Log(sheet.intColumn[0]);
Debug.Log(sheet.stringColumn[0]);

// access by row -- recommend
Debug.Log(sheet.Rows[0].intColumn);
Debug.Log(sheet.Rows[0].stringColumn);
```

## Relation projects
- [dotnetcore/NPOI](https://github.com/dotnetcore/NPOI)
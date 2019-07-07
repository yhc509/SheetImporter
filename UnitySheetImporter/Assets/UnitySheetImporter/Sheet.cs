using System;
using System.Collections.Generic;


[Serializable]
public class Sheet
{
    protected List<SheetRow> rows = new List<SheetRow>();

    public void AddRow(SheetRow row)
    {
        rows.Add(row);
    }

    public SheetRow[] Rows
    {
        get { return rows.ToArray(); }
    }

    public SheetRow this[int rowNum]
    {
        get { return rows[rowNum]; }
    }
}

[Serializable]
public class SheetRow
{
    public void AddCell(string key, object value)
    {
        this.GetType().GetField(key).SetValue(this, value);
    }
}


[Serializable]
public class SheetCol<T>
{
    private Sheet _sheet;
    private string _key;
	
    public SheetCol(Sheet sheet, string key)
    {
        _sheet = sheet;
        _key = key;
    }
    public T this[int rowNum]
    {
        get
        {
            var row = _sheet.Rows[rowNum];
            T value = (T) row.GetType().GetField(_key).GetValue(row);
            return value;
        }
    }
}

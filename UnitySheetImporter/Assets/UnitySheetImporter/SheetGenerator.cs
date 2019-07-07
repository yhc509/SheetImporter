using System;
using System.IO;
using System.Reflection;
using System.Text;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using UnityEditor;
using UnityEngine;

public class SheetGenerator
{
    private string _sheetName;
    private string _sheetRowName;
    private string _path;

    private StringBuilder sb = new StringBuilder();

    private ISheet _sheet;
    private object _generatedSheet;

    public SheetGenerator(string targetPath, string sheetName)
    {
        _sheetName = sheetName + "Sheet";
        _sheetRowName = sheetName + "SheetRow";
        _path = Path.Combine(targetPath, sheetName);
    }

    public void Generate(string sheetPath)
    {
        Convert(sheetPath);
        Save();
    }

    private void Convert(string sheetPath)
    {
        XSSFWorkbook workbook;

        using (FileStream file = new FileStream(sheetPath, FileMode.Open, FileAccess.Read))
        {
            workbook = new XSSFWorkbook(file);
        }

        ISheet sheet = workbook.GetSheetAt(0);

        Type sheetType = Type.GetType(_sheetName);
        if (sheetType == null) return;
        _generatedSheet = Activator.CreateInstance(sheetType);
        var typeRow = sheet.GetRow(0);
        var keyRow = sheet.GetRow(1);

        for (int row = 2; row <= sheet.LastRowNum; row++)
        {
            var dataRow = sheet.GetRow(row);
            if (dataRow != null)
            {
                var dataSheetRow = ConvertRow(dataRow, typeRow, keyRow);
                if (dataSheetRow == null) continue;

                var method = sheetType.GetMethod("AddRow");
                method.Invoke(_generatedSheet, new object[] {dataSheetRow});
            }
        }
    }

    private SheetRow ConvertRow(IRow row, IRow typeRow, IRow keyRow)
    {
        Type sheetRowType = Type.GetType(_sheetRowName);
        if (sheetRowType == null) return null;
        object dataSheetRow = Activator.CreateInstance(sheetRowType);

        for (int col = 0; col < row.Cells.Count; col++)
        {
            var cell = row.GetCell(col);
            var type = typeRow.GetCell(col).StringCellValue;
            var key = keyRow.GetCell(col).StringCellValue;

            var method = sheetRowType.GetMethod("AddCell");
            if (method == null) continue;
            switch (type)
            {
                case "string":
                    method.Invoke(dataSheetRow, new object[] {key, cell.StringCellValue});
                    break;
                case "int":
                    method.Invoke(dataSheetRow, new object[] {key, (int) cell.NumericCellValue});
                    break;
                case "float":
                    method.Invoke(dataSheetRow, new object[] {key, (float) cell.NumericCellValue});
                    break;
                case "bool":
                    method.Invoke(dataSheetRow, new object[] {key, (bool) cell.BooleanCellValue});
                    break;
                default:
                    return null;
            }
        }

        return dataSheetRow as SheetRow;
    }

    private void Save()
    {
        try
        {
            var filePath = Path.Combine(Application.dataPath, _path + ".bytes");

            var fw = new FileWriter(filePath);

            MethodInfo method = typeof(ObjectExtensions).GetMethod("Serialize");
            if(method == null) return;
            byte[] bytes = method.Invoke(null, new object[] {_generatedSheet}) as byte[];

            fw.Write(bytes);
            
            var dataPathUri = new System.Uri(Application.dataPath);

            var relativeUri = dataPathUri.MakeRelativeUri(new System.Uri(filePath));
            var relativePath = System.Uri.UnescapeDataString(relativeUri.ToString());
            AssetDatabase.ImportAsset(relativePath, ImportAssetOptions.ForceUpdate);
        }
        catch (IOException e)
        {
        }
    }
}
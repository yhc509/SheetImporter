using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

[Serializable]
public class InfoTable
{
    public string tableName;
    
    // columnName , typeName
    public Dictionary<string, string> types = new Dictionary<string, string>();
        
    // TODO - 값형식은 딕셔너리를 따로 할당하여 박싱/언방식 피할 것.
    // columnName , datas
    public Dictionary<string, List<object>> columns = new Dictionary<string, List<object>>();
}

public class TableInfo<K, V> where V : class
{
    // index, values
    public Dictionary<K, V> _dic = new Dictionary<K, V>();
    
    public TableInfo()
    {
        
    }

    public void Initailize(string tablePath)
    {
        FileReader reader = new FileReader(tablePath);
        var bytes = reader.Read();
        var info = bytes.Deserialize<InfoTable>();

        var tableName = Path.GetFileName(tablePath).Replace(".bytes", string.Empty);
        Type tableType = Type.GetType(tableName);
        if (tableType == null)
        {
            Debug.LogErrorFormat("{0} - 클래스 파일이 없습니다.", tableName);
            return;
        }
        
        List<object> indexList;
        info.columns.TryGetValue("Index", out indexList);
        if (indexList == null)
        {
            Debug.LogErrorFormat("{0} - Index 컬럼이 없습니다.", tablePath);
            return;
        }
        
        for(int r = 0; r < indexList.Count; r++)
        {
            var index = (K) indexList[r];
            object row = Activator.CreateInstance(tableType);
            
            foreach (var column in info.columns)
            {
                var columnName = column.Key;
                var columnValueList = column.Value;
                
                for (int i = 0; i < columnValueList.Count; i++)
                {
                    var type = row.GetType();
                    var prop = type.GetField(columnName);
                    if (prop == null)
                    {
                        Debug.LogErrorFormat("{0} 컬럼이 없음!" , columnName);
                        return;
                    };
                    prop.SetValue(row, columnValueList[r]);
                }
            }
            _dic.Add(index, (V) row);
        }
    }
    
    public V this[K key]
    {
        get
        {
            if (_dic.ContainsKey(key))
                return _dic[key];
            return null;
        }
    }

    public V[] FindAll(Func<V, bool> condition)
    {
        return _dic.Values.Where(condition).ToArray();
    }
    
    public V Find(Func<V, bool> condition)
    {
        return _dic.Values.FirstOrDefault(condition);
    }

    public void Clear()
    {
        _dic.Clear();
    }
}

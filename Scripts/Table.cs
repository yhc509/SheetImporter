using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class Table <K, V> where V : class
{
    // index, values
    private Dictionary<K, V> _dic = new Dictionary<K, V>();
    
    public Table()
    {
        
    }

    public void Initailize(string tablePath)
    {
        if (!File.Exists(tablePath))
        {
            Debug.LogErrorFormat("{0} - 파일이 없습니다.", tablePath);
            return;
        }
        
        FileReader reader = new FileReader(tablePath);
        var bytes = reader.Read();
        MemoryStream ms = new MemoryStream(bytes);
        BinaryFormatter b = new BinaryFormatter();
        var info = (InfoTable) b.Deserialize(ms);

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
            V v = null;
            _dic.TryGetValue(key, out v);
            return v;
        }
    }

    public List<V> ToList()
    {
        return _dic.Select(x => x.Value).ToList();
    }

    public int Count()
    {
        return _dic.Count;
    }

    public V ElementAt(int index)
    {
        return ToList().ElementAt(index);
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

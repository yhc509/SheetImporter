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

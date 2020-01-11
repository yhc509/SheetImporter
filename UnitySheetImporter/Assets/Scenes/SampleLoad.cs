using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleLoad : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        TableManager tableManager = new TableManager();
        tableManager.Initialize();

        var testTable = tableManager.TestTable;
        
        // index 기반 접근
        Debug.Log(testTable[10000].stringColumn);
        
        // 조건 탐색 접근
        Debug.Log(testTable.Find(x=>x.Index == 10000).stringColumn);
    }
    
}

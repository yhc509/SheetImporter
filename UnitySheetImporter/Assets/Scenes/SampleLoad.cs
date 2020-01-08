using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleLoad : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var bytePath = Application.dataPath + "/Data/Sample.bytes";
       
        var fr = new FileReader(bytePath);
        var bytes = fr.Read();
        var sheet = bytes.Deserialize<SampleSheet>();

        // access by column
        Debug.Log(sheet.intColumn[0]);
        Debug.Log(sheet.stringColumn[0]);
        Debug.Log(sheet.longColumn[0]);
        
        // access by row
        Debug.Log(sheet.Rows[0].intColumn);
        Debug.Log(sheet.Rows[0].stringColumn);
        Debug.Log(sheet.Rows[0].stringArrColumn[0]);
        Debug.Log(sheet.Rows[0].longArrColumn[0]);
    }
}

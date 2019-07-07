using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class BytesExtensions
{
    
    public static T Deserialize<T>(this byte[] obj) where T : class
    {
        object deserializeObj;
        MemoryStream ms = new MemoryStream(obj);
        BinaryFormatter b = new BinaryFormatter();
        deserializeObj = b.Deserialize(ms);

        return (T) deserializeObj;
    }
}

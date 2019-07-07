using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static partial class ObjectExtensions
{
    public static byte[] Serialize(this object obj)
    {
        MemoryStream ms = new MemoryStream();
        BinaryFormatter b = new BinaryFormatter();

        b.Serialize(ms, obj);

        return ms.GetBuffer();
    }
    
}

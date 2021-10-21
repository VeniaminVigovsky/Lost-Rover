using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class SaveSystem
{
    private static string path = Application.persistentDataPath + "/sd.rvr";

    public static void SaveData(SaveData data)
    {
        BinaryFormatter formatter = new BinaryFormatter();

        using (FileStream fileStream = new FileStream(path, FileMode.Create))
        {
            formatter.Serialize(fileStream, data);
        }
    }

    public static SaveData LoadData()
    {
        BinaryFormatter formatter = new BinaryFormatter();

        using (FileStream fileStream = new FileStream(path, FileMode.Open))
        {
            return formatter.Deserialize(fileStream) as SaveData;
        }
    }

}

using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class BaseData : MonoBehaviour
{
    public static BaseData Instance;

    public JunkyardData junkyardData = new JunkyardData();

    void Awake()
    {
        Instance = this;
        LoadFromBinary();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            SaveToBinary();
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            LoadFromBinary();
        }
    }

    public void SaveToBinary()
    {
        string filePath = Application.persistentDataPath + "/Junkyard.dat";
        BinaryFormatter formatter = new BinaryFormatter();

        FileStream stream = new FileStream(filePath, FileMode.Create);

        formatter.Serialize(stream, junkyardData);
        stream.Close();

        Debug.Log("Saved to Binary | Junkyard: " + filePath);
    }

    public void LoadFromBinary()
    {


        string filePath = Application.persistentDataPath + "/Junkyard.dat";
        if (File.Exists(filePath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(filePath, FileMode.Open);

            junkyardData = (JunkyardData)formatter.Deserialize(stream);
            stream.Close();

            Debug.Log("Loaded from Binary | Junkyard");
        }
        else
        {
            Debug.LogWarning("Save file not found at " + filePath);
        }
    }
}

[System.Serializable]
public class JunkyardData
{
    public LevelData levelData = new LevelData();
    public MoneyData moneyData = new MoneyData();
}
[System.Serializable]
public class LevelData
{
    public int level = 1;
    public int experience;
}
[System.Serializable]
public class MoneyData
{
    public float money;
    public float totalMoneyEarned;
}
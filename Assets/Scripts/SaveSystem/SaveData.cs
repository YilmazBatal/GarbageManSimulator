using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveData : MonoBehaviour
{
    public static SaveData Instance;

    public Inventory inventory = new Inventory();

    void Awake() {
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
        string filePath = Application.persistentDataPath + "/GameData.dat";
        BinaryFormatter formatter = new BinaryFormatter();

        FileStream stream = new FileStream(filePath, FileMode.Create);

        formatter.Serialize(stream, inventory);
        stream.Close();

        Debug.Log("Saved to Binary: " + filePath);
    }

    public void LoadFromBinary()
    {


        string filePath = Application.persistentDataPath + "/GameData.dat";
        if (File.Exists(filePath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(filePath, FileMode.Open);

            inventory = (Inventory)formatter.Deserialize(stream);
            stream.Close();

            Debug.Log("Loaded from Binary");
        }
        else
        {
            Debug.LogWarning("Save file not found at " + filePath);
        }
    }
}

[System.Serializable]
public class Inventory
{
    public float money;
    public int experience;
    public int level;
    public List<Items> storage = new List<Items>();
    public PlayerSkills playerSkills = new PlayerSkills();
}

[System.Serializable]
public class Items
{
    public float asdf;
    public float ghjk;
}
[System.Serializable]
public class PlayerSkills
{
    public string[] equippedSkillIDs = new string[3];
}

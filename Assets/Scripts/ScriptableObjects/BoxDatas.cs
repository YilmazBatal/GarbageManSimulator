using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New_Box_Data", menuName = "Scriptable Objects/BoxDatas")]
public class BoxDatas : ScriptableObject
{
    public string boxName;
    public List<ItemRarityChance> rarityChances;
}

[System.Serializable]
public class ItemRarityChance
{
    public Rarity rarity;
    [Range(0, 1)] public float chance;  // % olarak 0.0-1.0
    public int minDrop; // minimum eşya sayısı
    public int maxDrop; // maximum eşya sayısı
    
    public List<TrashTypes> possibleItems; // o rarity’de çıkabilecek eşyalar
}
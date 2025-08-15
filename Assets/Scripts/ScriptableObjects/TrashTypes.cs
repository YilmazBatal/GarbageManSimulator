using UnityEngine;

[CreateAssetMenu(fileName = "New_Trash_Type", menuName = "Scriptable Objects/TrashTypes")]
public class TrashTypes : ScriptableObject
{
    public string trashName;
    public string trashDescription;
    public string rarity;
    public float weight;
    public float value; // Value in game currency
    public bool isToxic;
    public bool isRecyclable; // Indicates if the trash can be converted into sellable materials
    public Sprite trashIcon;
    public GameObject trashPrefab;
}

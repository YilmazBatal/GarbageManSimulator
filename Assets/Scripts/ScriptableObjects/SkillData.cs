using UnityEngine;

[CreateAssetMenu(fileName = "New_Skill_Data", menuName = "Scriptable Objects/SkillDatas")]
[System.Serializable]
public class SkillData : ScriptableObject
{
    public string skillID;
    public string skillName;
    public string skillDesc;
    public Sprite icon;
    public SkillType skillType;
    public float value;
}

[System.Serializable]
public enum SkillType
{
    None,
    ToxicUnlock,
    SpeedBoost,
    MoneyBoost,
    ExpBoost,
    HeavyUnlock
}
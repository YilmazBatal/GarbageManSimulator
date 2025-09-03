using System.Linq;
using UnityEngine;

public class SkillDatabase : MonoBehaviour
{
    public static SkillDatabase Instance;
    [SerializeField] public SkillData[] allSkills;

    void Awake() => Instance = this;

    public SkillData GetSkillByID(string id)
    {
        return allSkills.FirstOrDefault(s => s.skillID == id);
    }
}

using UnityEngine;

public class PlayerSkillManager : MonoBehaviour
{
    [SerializeField] private SkillData[] equippedSkills = new SkillData[3];

    void Start()
    {
        LoadPlayerSkills();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            SavePlayerSkills();
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            LoadPlayerSkills();
        }
    }
    public void SavePlayerSkills()
    {
        for (int i = 0; i < equippedSkills.Length; i++)
        {
            if (equippedSkills[i] != null)
                SaveData.Instance.inventory.playerSkills.equippedSkillIDs[i] = equippedSkills[i].skillID;
            else
                SaveData.Instance.inventory.playerSkills.equippedSkillIDs[i] = SkillDatabase.Instance.allSkills[0].skillID;
                print("Saved Skill ID: " + SaveData.Instance.inventory.playerSkills.equippedSkillIDs[i]);
        }
    }
    public void LoadPlayerSkills()
    {
        string[] savedIDs = SaveData.Instance.inventory.playerSkills.equippedSkillIDs;

        for (int i = 0; i < savedIDs.Length; i++)
        {
            if (!string.IsNullOrEmpty(savedIDs[i]))
                equippedSkills[i] = SkillDatabase.Instance.GetSkillByID(savedIDs[i]);
            else
                equippedSkills[i] = SkillDatabase.Instance.allSkills[0];
        }
    }
}

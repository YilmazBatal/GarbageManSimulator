using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillPanel : MonoBehaviour
{
    [SerializeField] SkillData[] allSkills => SkillDatabase.Instance.allSkills;
    [SerializeField] GameObject skillsContent;
    [SerializeField] GameObject skillTemplate;

    [SerializeField] TMP_Text activeSkillName;
    [SerializeField] TMP_Text activeSkillDescription;
    [SerializeField] Image activeSkillIcon;
    [SerializeField] TMP_Text activeSkillEffect;

    [SerializeField] Button assignButton_1;
    [SerializeField] Button assignButton_2;
    [SerializeField] Button assignButton_3;

    [SerializeField] GameObject[] playerSlots = new GameObject[3];


    SkillData selectedSkill;

    void Start()
    {
        UpdateSkillsUI();
        UpdateActiveSkillUI();
    }

    private void UpdateSkillsUI()
    {
        foreach (SkillData item in allSkills)
        {
            GameObject newSkill = Instantiate(skillTemplate, skillsContent.transform);
            newSkill.transform.GetChild(0).GetComponent<Image>().sprite = item.icon;
            newSkill.GetComponent<Button>().onClick.AddListener(() => InspectSkill(item));
        }
    }

    private void UpdateActiveSkillUI()
    {

    }

    #region Button Methods
    /// <summary>
    /// Called when a skill is selected from the list.
    /// </summary>
    public void InspectSkill(SkillData SkillInfo)
    {
        selectedSkill = SkillInfo;

        activeSkillName.text = SkillInfo.skillName;
        activeSkillDescription.text = SkillInfo.skillDesc;
        activeSkillIcon.sprite = SkillInfo.icon;
        activeSkillEffect.text = SkillInfo.skillType.ToString();
    }
    

    public void AssignSkillToSlot(int slotIndex)
    {
        playerSlots[slotIndex].transform.GetChild(0).GetComponent<Image>().sprite = selectedSkill.icon;
    }
    #endregion
}

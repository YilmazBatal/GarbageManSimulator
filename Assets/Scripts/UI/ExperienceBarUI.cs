using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExpBarUI : MonoBehaviour
{
    [SerializeField] private Image expFill;
    [SerializeField] private TMP_Text expInfoText;
    [SerializeField] private TMP_Text currentLevelText;
    [SerializeField] private TMP_Text nextLevelText;

    private void OnEnable()
    {
        EventManager.OnExpChanged += UpdateExpBar; // Dinlemeye başla
    }

    private void OnDisable()
    {
        EventManager.OnExpChanged -= UpdateExpBar; // Temizlik
    }

    private void UpdateExpBar(int currentExp, int requiredExp)
    {
        expFill.fillAmount = (float)currentExp / requiredExp;
        expInfoText.text = $"{currentExp} / {requiredExp} XP";
        currentLevelText.text =  BaseData.Instance.junkyardData.levelData.level.ToString();
        nextLevelText.text = (BaseData.Instance.junkyardData.levelData.level + 1).ToString();
    }
}

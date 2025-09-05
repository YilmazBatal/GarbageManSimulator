using UnityEngine;

public class LevelSystem
{
    private int currentExp => BaseData.Instance.junkyardData.levelData.experience;
    private int currentLevel => BaseData.Instance.junkyardData.levelData.level;
    private int requiredExp = 100;

    public void AddExp(int amount)
    {
        BaseData.Instance.junkyardData.levelData.experience += amount;

        // Level atlama kontrolü
        while (currentExp >= requiredExp)
        {
            BaseData.Instance.junkyardData.levelData.experience -= requiredExp;
            BaseData.Instance.junkyardData.levelData.level++;
            Debug.Log("Level Up! New Level: " + currentLevel);
        }

        // EventManager üzerinden UI ve diğer sistemleri bilgilendir
        EventManager.ExpChanged(currentExp, requiredExp);
    }

    // İsteğe bağlı getterlar
    public int GetCurrentExp() => currentExp;
    public int GetCurrentLevel() => currentLevel;
    public int GetRequiredExp() => requiredExp;
}
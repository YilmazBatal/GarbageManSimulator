using System;

public static class EventManager
{
    public static event Action<int> OnMoneyChanged;
    public static event Action<int, int> OnExpChanged;
    // int junkyardLevel => BaseData.Instance.junkyardData.junkyardLevel;
    // float experience => BaseData.Instance.junkyardData.experience;

    public static void MoneyChanged(int newMoney) => OnMoneyChanged?.Invoke(newMoney);
    public static void ExpChanged(int current, int required) => OnExpChanged?.Invoke(current, required);
}
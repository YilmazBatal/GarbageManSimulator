using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class TrashCollector : MonoBehaviour
{
    [SerializeField] private TMP_Text infoText;
    /// <summary>
    /// Maximum vehicle trash capacity
    /// </summary>
    [SerializeField] public int vehicleCapacity = 20;
    public int vehicleTrashCount;
    [SerializeField] public List<TrashSlot> vehicleInventory = new List<TrashSlot>();

    void Start()
    {
        UpdateInfoText();
    }

    public void AddFromBin(List<TrashSlot> binInventory)
{
    foreach (TrashSlot slot in binInventory)
    {
        int spaceLeft = vehicleCapacity - vehicleTrashCount;
        if (spaceLeft <= 0)
            break; // araç dolu

        // Kaç tane aktarılabilir?
        int transferable = Mathf.Min(spaceLeft, slot.amount);

        // Var olan aynı türden varsa ekle
        TrashSlot existing = vehicleInventory.Find(s => s.trashType == slot.trashType);
        if (existing != null)
            existing.amount += transferable;
        else
            vehicleInventory.Add(new TrashSlot { trashType = slot.trashType, amount = transferable });

        // Araçtaki mevcut çöp sayısını arttır
        vehicleTrashCount += transferable;

        // Bin'den eksilt
        slot.amount -= transferable;
    }

    infoText.text = vehicleTrashCount + " / " + vehicleCapacity;
}

    void OnTriggerEnter(Collider other)
    {
        // Çarpılan objede TrashBin scripti var mı diye kontrol et
        if (other.TryGetComponent<TrashBin>(out TrashBin bin))
        {
            bin.isNearVehicle = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<TrashBin>(out TrashBin bin))
        {
            bin.isNearVehicle = false;
        }
    }
    private void UpdateInfoText()
    {
        infoText.text = vehicleTrashCount.ToString() + "/" + vehicleCapacity;
    }
}

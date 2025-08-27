using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class TrashCollector : MonoBehaviour
{
    [SerializeField] private TMP_Text infoText;
    [SerializeField] public int vehicleCapacity = 20;
    int currentTrashCount;
    [SerializeField] public List<TrashSlot> vehicleInventory = new List<TrashSlot>();

    public void AddFromBin(List<TrashSlot> binInventory)
    {
        foreach (TrashSlot slot in binInventory)
        {
            TrashSlot existing = vehicleInventory.Find(s => s.trashType == slot.trashType);
            if (existing != null)
                existing.amount += slot.amount; // aynı tür → miktarı ekle
            else
                vehicleInventory.Add(new TrashSlot { trashType = slot.trashType, amount = slot.amount });
            currentTrashCount += slot.amount;
        }

        infoText.text = currentTrashCount.ToString() + "/" + vehicleCapacity;
    }

    void OnTriggerEnter(Collider other)
    {
        // Çarpılan objede TrashBin scripti var mı diye kontrol et
        if (other.TryGetComponent<TrashBin>(out TrashBin bin))
        {
            bin.isNearVehicle = true;
            Debug.Log("Çöp kutusu bulundu!");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<TrashBin>(out TrashBin bin))
        {
            bin.isNearVehicle = false;
            Debug.Log("Çöp kutusundan uzaklaşıldı.");
        }
    }
}

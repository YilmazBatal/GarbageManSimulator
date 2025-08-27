using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[System.Serializable]
public class TrashSlot
{
    public TrashTypes trashType; 
    public int amount;          
}

public class TrashBin : MonoBehaviour
{
    [SerializeField] private int binCapacity = 5; // kapasite
    public List<TrashSlot> inventory = new List<TrashSlot>();

    [NonSerialized] public bool isNearVehicle;
    [NonSerialized] public bool isGettingLooked;

    [SerializeField] Animator trashBinLidAnimator;
    [SerializeField] TMP_Text trashInventoryText;
    [SerializeField] AudioSource audioSource;

    public TrashCollector trashCollector;

    private void Start()
    {
        UpdateTrashInventoryText();
        GetTrashCollector();
    }

    private void GetTrashCollector()
    {
        trashCollector = GameObject.FindGameObjectWithTag("VehicleTrashCollector").GetComponent<TrashCollector>();
    }

    private void UpdateTrashInventoryText()
    {
        trashInventoryText.text = CurrentTrashCount + " / " + binCapacity;
    }

    // 🔑 Toplamı inventory'den hesaplıyoruz
    public int CurrentTrashCount {
        get {
            int total = 0;
            foreach (var slot in inventory)
                total += slot.amount;
            return total;
        }
    }

    void Update()
    {
        TransferBin();
    }

    private void TransferBin()
    {
        if (isNearVehicle && isGettingLooked && Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("Trash box Inventory deposited to vehicle");
            // code here
            TrashCollector vehicle = FindAnyObjectByType<TrashCollector>(); // ya da OnTriggerEnter ile cachele
            if (vehicle != null)
            {
                vehicle.AddFromBin(inventory); // çöpleri araca aktar
                inventory.Clear();             // kutuyu boşalt
                trashBinLidAnimator.SetBool("isFull", false);
                UpdateTrashInventoryText();    // UI sıfırla
                audioSource.PlayOneShot(AudioManager.Instance.trashPile);

            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Trash"))
        {
            if (CurrentTrashCount < binCapacity) 
            {
                AddTrash(other.GetComponent<PickableItem>().trashData);
                UpdateTrashInventoryText();

                if (CurrentTrashCount == binCapacity)
                    trashBinLidAnimator.SetBool("isFull", true);
            }

            Destroy(other.gameObject);
        }
    }

    public void AddTrash(TrashTypes type)
    {
        TrashSlot slot = inventory.Find(s => s.trashType == type);
        if (slot != null)
            slot.amount++;
        else
            inventory.Add(new TrashSlot { trashType = type, amount = 1 });
    }
}

using System;
using System.Collections.Generic;
using TMPro;
using Unity.Cinemachine;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class TrashSlot
{
    public TrashTypes trashType; // SO referansı
    public int amount;          // kaç adet
}

public class TrashBin : MonoBehaviour
{
    private int binCapacity = 1; 
    private int trashInBinInventory = 0; 
    public List<TrashSlot> inventory = new List<TrashSlot>();
    
    [SerializeField] GameObject trashBinLid;
    [SerializeField] Animator trashBinLidAnimator;
    [SerializeField] TMP_Text trashInventoryText;


    private void Start()
    {
        UpdateTrashInventoryText();
    }

    private void UpdateTrashInventoryText()
    {
        trashInventoryText.text = trashInBinInventory + " / " + binCapacity;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            GameObject.Find("Player").transform.GetChild(0).GetComponent<CinemachineImpulseSource>().GenerateImpulse();
            // in the future get the players nearby 

        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            // in the future get the players nearby 

        }
        // if (trashBinLidAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !trashBinLidAnimator.IsInTransition(0) && trashBinLidAnimator.GetBool("isFull"))
        // {
        //     GameObject.Find("Player").transform.GetChild(0).GetComponent<CinemachineImpulseSource>().GenerateImpulse();

        // }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Trash"))
        {
            if (trashInBinInventory != binCapacity) // inventory is not full
            {
                //Add the trash item to the bin inventory
                AddTrash(other.GetComponent<PickableItem>().trashData);
                trashInBinInventory++;
                UpdateTrashInventoryText();
                if (trashInBinInventory == binCapacity) // if bin is now full then close with lean tween 
                {
                    trashBinLidAnimator.SetBool("isFull", true);
                }
            }

            Destroy(other.gameObject);
        }
    }

    public void AddTrash(TrashTypes type)
    {
        TrashSlot slot = inventory.Find(s => s.trashType == type);
        if (slot != null)
        {
            slot.amount++;
        }
        else
        {
            inventory.Add(new TrashSlot { trashType = type, amount = 1 });
        }
    }
}

using UnityEngine;

public class TrashCollector : MonoBehaviour
{
    void Update()
    {
        
    }
    void OnTriggerEnter(Collider other)
    {
        // Çarpılan objede TrashBin scripti var mı diye kontrol et
        if (other.TryGetComponent<TrashBin>(out TrashBin bin))
        {
            bin.isNearVehicle = true;
            Debug.Log("Çöp kutusu bulundu!");

            if (Input.GetKeyDown(KeyCode.F))
            {
                Debug.Log("Çöp kutusu bulundu! Çeşit: " + bin.inventory.Count);
            }
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

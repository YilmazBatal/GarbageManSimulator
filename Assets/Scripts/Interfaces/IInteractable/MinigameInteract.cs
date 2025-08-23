using UnityEngine;

public enum MinigameType {Smash, HitGreen}
public class MinigameInteract : MonoBehaviour, IInteractable
{
    [SerializeField] MinigameType minigameType; // Type of the minigame to be played // choosed in inspector
    [SerializeField] BoxDatas boxData; // script that holds UI Prefabs for minigames
    [SerializeField] AudioSource audioSource; // script that holds UI Prefabs for minigames

    GameObject minigameUI;

    void Start()
    {
        switch (minigameType)
        {
            case MinigameType.Smash:
                minigameUI = Minigames.Instance.smashBoxUI;
                break;
            case MinigameType.HitGreen:
                minigameUI = Minigames.Instance.greenZone;
                break;
            // case MinigameType.HitGreen:
            //     minigameUI = Minigames.Instance.hitGreenUI;
            //     break;
        }
    }

    /// <summary>
    /// Method to interact with the minigame.
    /// </summary>
    public void Interact()
    {
        UIManager.Instance.GenerateMinigamePanel(minigameUI, gameObject);
        audioSource.PlayOneShot(AudioManager.Instance.interactSfx);
    }

    /// <summary>
    /// Main method of rewarding the player after a successful minigame.
    /// </summary>
    void ReleaseReward(GameObject rewardedObject)
    {
        GameObject rewardObject = Instantiate(rewardedObject, transform.position + new Vector3(0, 0, 0), Quaternion.identity);
        rewardObject.GetComponent<PickableItem>().isReward = true; // Set the item as a reward
        rewardObject.transform.SetParent(transform.parent); // Set the parent to the same as the minigame interact object
        rewardObject.GetComponent<Rigidbody>().isKinematic = true; // Disable physics for the reward object

        LeanTween.moveY(rewardObject, rewardObject.transform.position.y + 2f, 1.5f)
            .setEase(LeanTweenType.easeOutQuad);
        LeanTween.rotateAroundLocal(rewardObject, Vector3.up, 360f, 1f)
            .setRepeat(-1)       // sonsuz tekrar
            .setEase(LeanTweenType.linear); // sabit hız
        
    }

    public void GenerateReward() {
        // Kümülatif (birikimli) olasılıkları hesaplayalım:
        // Common: 0.0    - 0.55  -> (%55)
        // Uncommon: 0.55 - 0.80  -> (%25) (0.55 + 0.25)
        // Rare: 0.80   - 0.92  -> (%12) (0.80 + 0.12)
        // Epic: 0.92   - 0.98  -> (%6)  (0.92 + 0.06)
        // Legendary: 0.98 - 1.0  -> (%2)  (0.98 + 0.02)

        float chanceValue = Random.Range(0f, 1f); // example data 0.8549
        float cumulativeProbability = 0f;
        
        foreach (var rarityInfo in boxData.rarityChances) // kutunun içerdiği nadirlik sayısı kadar
        {
            cumulativeProbability += rarityInfo.chance;

            if (chanceValue < cumulativeProbability)
            {
                // SEÇİLEN NADİRLİK BUDUR!
                // Debug.Log($"<color=green>Ödül bulundu! Nadirlik: {rarityInfo.rarity.ToString()}</color>");

                // --- Buradan sonra seçilen nadirliğe göre eşya verme işlemleri yapılır ---

                // Kaç adet eşya düşeceğini belirle (min ve max arasından)
                int dropCount = Random.Range(rarityInfo.minDrop, rarityInfo.maxDrop + 1);
                // Debug.Log($"{dropCount} adet eşya verilecek.");

                // Belirlenen adette rastgele eşya seç ve ver
                for (int i = 0; i < dropCount; i++)
                {
                    if (rarityInfo.possibleItems.Count > 0)
                    {
                        // Eşya listesinden rastgele bir eşya seç
                        int randomIndex = Random.Range(0, rarityInfo.possibleItems.Count);
                        TrashTypes selectedItem = rarityInfo.possibleItems[randomIndex];

                        // Debug.Log($"Verilen Eşya: {selectedItem.ToString()}");

                        ReleaseReward(selectedItem.trashPrefab);
                    }
                }

                // Ödülü bulduğumuz için döngüden ve fonksiyondan çıkabiliriz.
                return;
            }
        }
    }


}
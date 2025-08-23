using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HitGreen : MonoBehaviour
{
    [Header("UI Objects")]
    [SerializeField] RectTransform RedZone;
    [SerializeField] GameObject GreenZone;
    [SerializeField] int greenCount = 2;
    [SerializeField] TMP_Text SuccessfullHitsText;


    [Space(10)]
    [SerializeField] int requiredSuccessfulHits = 3;
    int successfullHits = 0;
    [SerializeField] float hitCooldown = 1f;
    bool isOnCooldown = false;
    
    LTDescr SliderTween;

    bool success => UIManager.Instance.isSucessfull;

    void Start()
    {
        SuccessfullHitsText.text = successfullHits + "/" + requiredSuccessfulHits;

        GenerateZones();
        StartTween();
    }

    private void GenerateZones()
    {
         float barWidth = RedZone.rect.width;

        for (int i = 0; i < greenCount; i++)
        {
            // Rastgele genişlik ve pozisyon
            float width = UnityEngine.Random.Range(30f, 80f);
            float posX = UnityEngine.Random.Range(-barWidth / 2f + width / 2f, barWidth / 2f - width / 2f);

            GameObject zone = Instantiate(GreenZone, RedZone);
            RectTransform rt = zone.GetComponent<RectTransform>();

            rt.sizeDelta = new Vector2(width, rt.sizeDelta.y);
            rt.anchoredPosition = new Vector2(posX, 0f);
        }
    }

    void StartTween()
    {
        // SliderTween = LeanTween.value(gameObject, 0f, 1f, 0.66f)
        //     .setOnUpdate((float value) =>
        //     {
        //         slider.value = value;
        //     }).setEaseInOutQuad().setLoopPingPong();
    }

    // Update is called once per frame
    void Update()
    {
        // PlayerHit();
    }

    // void PlayerHit()
    // {
    //     bool spacePressed = Input.GetKeyDown(KeyCode.Space);

    //     if (spacePressed && !isOnCooldown && !success)
    //     {
    //         if (SliderTween != null)
    //         {
    //             StartCoroutine(WaitCooldown());
    //         }
    //     }
    //     else if (spacePressed && isOnCooldown && !success)
    //     {
    //         ToastNotification.Show("You are on cooldown, wait a bit before hitting again", 2, "alert");
    //     }
    //     else if (success)
    //     {
    //         UIManager.Instance.SuccessfulMinigame(gameObject);
    //     }
    // }

    // IEnumerator WaitCooldown()
    // {
    //     UIManager.Instance.ShakeUI(gameObject, 30f, 0.10f, 10);

    //     isOnCooldown = true;
    //     sliderHandle.color = Color.gray3;
    //     CheckSuccess();
    //     yield return new WaitForSeconds(hitCooldown);
    //     sliderHandle.color = Color.white;
    //     isOnCooldown = false;
    // }
    // void CheckSuccess()
    // {
    //    if (slider.value <= 0.065f || slider.value >= 0.935f || (slider.value >= 0.41f && slider.value <= 0.59f))
    //     {
    //         successfullHits++;
    //         hitText.text = successfullHits + "/" + requiredSuccessfulHits;

    //         if (successfullHits >= requiredSuccessfulHits)
    //         {
    //             UIManager.Instance.isSucessfull = true;
    //             LeanTween.cancel(SliderTween.id);
    //         }
    //     }
    // }
}

using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HitGreen : MonoBehaviour
{
    [Header("UI Objects")]
    [SerializeField] Slider slider;
    [SerializeField] Image sliderHandle;
    [SerializeField] TMP_Text hitText;
    [SerializeField] int requiredSuccessfulHits = 3;
    [SerializeField] float hitCooldown = 0.5f;

    [Space(10)]
    bool isOnCooldown = false;
    int successfullHits = 0;
    bool isSucessfull = false;

    LTDescr SliderTween;
    void Start()
    {
        StartTween();
    }

    void StartTween()
    {
        SliderTween = LeanTween.value(gameObject, 0f, 1f, 0.66f)
            .setOnUpdate((float value) =>
            {
                slider.value = value;
            }).setEaseInOutQuad().setLoopPingPong();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerHit();
    }

    void PlayerHit()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isOnCooldown && !isSucessfull)
        {
            if (SliderTween != null)
            {
                StartCoroutine(WaitCooldown());
            }
        } else if (Input.GetKeyDown(KeyCode.Space) && isOnCooldown && !isSucessfull)
        {
            ToastNotification.Show("You are on cooldown, wait a bit before hitting again", 2, "alert");
        } else if (isSucessfull)
        {
            ToastNotification.Show("You are rewarded now!", 2, "success");
            UIManager.Instance.ClosePanel();
            Destroy(gameObject);
        }
    }
    IEnumerator WaitCooldown()
    {
        UIManager.Instance.ShakeUI(gameObject, 30f, 0.10f, 10);

        isOnCooldown = true;
        sliderHandle.color = Color.gray7;
        CheckSuccess();
        yield return new WaitForSeconds(hitCooldown);
        sliderHandle.color = Color.white;
        isOnCooldown = false;

    }
    void CheckSuccess()
    {
       if (slider.value <= 0.065f || slider.value >= 0.935f || (slider.value >= 0.41f && slider.value <= 0.59f))
        {
            successfullHits++;
            hitText.text = successfullHits + "/" + requiredSuccessfulHits;

            if (successfullHits >= 3)
            {
                Debug.Log("Player rewarded!");
                isSucessfull = true;
                LeanTween.cancel(SliderTween.id);
            }
        }
    }
}

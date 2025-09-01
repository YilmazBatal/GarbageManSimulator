using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GreenZone : MonoBehaviour
{
    [Header("UI Objects")]
    [SerializeField] RectTransform RedZone;
    [SerializeField] GameObject GreenZoneUI;
    [SerializeField] RectTransform BarArrow;
    [SerializeField] int greenCount = 2;
    [SerializeField] TMP_Text SuccessfullHitsText;



    [Space(10)]
    [SerializeField] int requiredSuccessfulHits = 3;
    int successfullHits = 0;
    [SerializeField] float hitCooldown = 1f;
    bool isOnCooldown = false;
    float barArrowPos;
    bool isDamaged;
    List<(float min, float max)> greenZones = new List<(float, float)>();

    LTDescr handleTween;

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
            float width = Random.Range(30f, 80f);
            float posX = Random.Range(-barWidth / 2f + width / 2f, barWidth / 2f - width / 2f);

            GameObject zone = Instantiate(GreenZoneUI, RedZone);
            RectTransform rt = zone.GetComponent<RectTransform>();

            rt.sizeDelta = new Vector2(width, rt.sizeDelta.y);
            rt.anchoredPosition = new Vector2(posX, 0f);
            greenZones.Add((rt.anchoredPosition.x - (width / 2), rt.anchoredPosition.x + width / 2));
        }
    }

    void StartTween()
    {
        handleTween = LeanTween.moveX(BarArrow, 330, 0.5f)
            .setEase(LeanTweenType.easeInOutQuad)
            .setLoopPingPong()
            .setFrom(-330f);
    }

    void Update()
    {
        PlayerHit();
    }

    void PlayerHit()
    {
        bool spacePressed = Input.GetKeyDown(KeyCode.Space);

        if (spacePressed && !isOnCooldown && !success)
        {
            if (handleTween != null)
            {
                StartCoroutine(WaitCooldown());
            }
        }
        else if (spacePressed && isOnCooldown && !success)
        {
            ToastNotification.Show("You are on cooldown, wait a bit before hitting again", 2, "alert");
        }
        else if (success)
        {
            UIManager.Instance.SuccessfulMinigame(gameObject);
        }
    }

    IEnumerator WaitCooldown()
    {
        UIManager.Instance.ShakeUI(gameObject, 30f, 0.10f, 10);

        isOnCooldown = true;
        handleTween.pause();
        BarArrow.gameObject.GetComponent<UnityEngine.UI.Image>().color = Color.gray6;
        barArrowPos = BarArrow.anchoredPosition.x;
        CheckSuccess();
        yield return new WaitForSeconds(hitCooldown);
        BarArrow.gameObject.GetComponent<UnityEngine.UI.Image>().color = Color.white;
        handleTween.resume();
        isOnCooldown = false;
    }

    void CheckSuccess()
    {
        for (int i = 0; i < greenZones.Count; i++)
        {
            if (barArrowPos >= greenZones[i].min && barArrowPos <= greenZones[i].max)
            {
                successfullHits++;
                SuccessfullHitsText.text = successfullHits + "/" + requiredSuccessfulHits;
                if (successfullHits >= requiredSuccessfulHits)
                {
                    UIManager.Instance.isSucessfull = true;
                    LeanTween.cancel(handleTween.id);
                    UIManager.Instance.SuccessfulMinigame(gameObject);
                    break;
                }
                break;
            }
            else
            {
                isDamaged = true;
            }
        }

    }
}

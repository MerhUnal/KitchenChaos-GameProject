using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayingClockUI : MonoBehaviour
{
    [SerializeField] private Image timerImage;

    [SerializeField] private TextMeshProUGUI dialsecondText;

    private void Update()
    {
        timerImage.fillAmount = KitchenGameManager.Instance.GetGamePlayingTimerNormalized();

        float gamePlayingTimer = KitchenGameManager.Instance.GetGamePlayingTimer();
        dialsecondText.text = Mathf.Ceil(gamePlayingTimer).ToString();

    }
}

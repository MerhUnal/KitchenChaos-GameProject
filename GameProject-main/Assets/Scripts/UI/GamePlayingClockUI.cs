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
    {   float gamePlayingTimer = KitchenGameManager.Instance.GetGamePlayingTimer();

        timerImage.fillAmount = KitchenGameManager.Instance.GetGamePlayingTimerNormalized();
        //if (gamePlayingTimer > 60f)
        //{
        //    timerImage.fillAmount = 1f;
        //}
        //else
        //{
            
        //}


        dialsecondText.text = Mathf.Ceil(gamePlayingTimer).ToString();

    }
}

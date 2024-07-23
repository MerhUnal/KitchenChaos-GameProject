using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameEndCountdownUI : MonoBehaviour
{
    private const string NUMBER_POPUP = "Number_Popup";


    [SerializeField] private TextMeshProUGUI countdownText;

    private Animator animator;
    private int previousCountDownNumber;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        
        KitchenGameManager.Instance.OnGamePlayingTimerChanged += KitchenGameManager_OnGamePlayingTimerChanged;
        Hide();
    }

    private void KitchenGameManager_OnGamePlayingTimerChanged(object sender, System.EventArgs e)
    {
        // Geri sayim süresini aliyoruz.
        int countdownNumber = Mathf.CeilToInt(KitchenGameManager.Instance.GetGamePlayingTimer());
       
        if (countdownNumber <= 10 && countdownNumber > 0) // Son 10 saniye kontrolu.
        {
           // print(countdownNumber);
            countdownText.text = countdownNumber.ToString();
            Show();
            if (previousCountDownNumber != countdownNumber)
            {
                previousCountDownNumber = countdownNumber;
                animator.SetTrigger(NUMBER_POPUP); // Animasyon tetikleniyor.
                SoundManager.Instance.PLayCountdownSound(); // Ses caliniyor.
            }
        }
        else
        {
            Hide();
        }
    }

   

   
    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}

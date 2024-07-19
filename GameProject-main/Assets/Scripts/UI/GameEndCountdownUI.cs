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
        KitchenGameManager.Instance.OnStateChanged += KitchenGameManager_OnstateChanged;
        KitchenGameManager.Instance.OnGamePlayingTimerChanged += KitchenGameManager_OnGamePlayingTimerChanged;
        Hide();
    }

    private void KitchenGameManager_OnGamePlayingTimerChanged(object sender, System.EventArgs e)
    {
        // Geri sayim süresini aliyoruz.
        int countdownNumber = Mathf.CeilToInt(KitchenGameManager.Instance.GetGamePlayingTimer());
        if (countdownNumber <= 10 && countdownNumber > 0) // Son 10 saniye kontrolu.
        {
            countdownText.text = countdownNumber.ToString();

            if (previousCountDownNumber != countdownNumber)
            {
                previousCountDownNumber = countdownNumber;
                animator.SetTrigger(NUMBER_POPUP); // Animasyon tetikleniyor.
                SoundManager.Instance.PlayCountdownSound(); // Ses caliniyor.
            }
        }
        else
        {
            Hide();
        }
    }

    private void KitchenGameManager_OnstateChanged(object sender, System.EventArgs e)
    {
        if (KitchenGameManager.Instance.IsCountdownToStartActive())
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    private void Update()
    {
        int countdownNumber = Mathf.CeilToInt(KitchenGameManager.Instance.GetCountdownToStartTimer());
        countdownText.text = countdownNumber.ToString();

        if (previousCountDownNumber != countdownNumber)
        {
            previousCountDownNumber = countdownNumber;
            animator.SetTrigger("NUMBER_Popup");
            SoundManager.Instance.PLayCountdownSound();
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

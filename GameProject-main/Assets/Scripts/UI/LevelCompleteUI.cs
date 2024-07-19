using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NextLevelUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI recipesDeliveredText;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button nextLevelButton;

    private void Awake()
    {

        restartButton.onClick.AddListener(() => {
            Loader.Load(Loader.Scene.GameScene);
        });
        mainMenuButton.onClick.AddListener(() => {
            Loader.Load(Loader.Scene.MainMenuScene);
        });
    }

    private void Start()
    {
        KitchenGameManager.Instance.OnStateChanged += KitchenGameManager_OnstateChanged;

        Hide();
    }

    private void KitchenGameManager_OnstateChanged(object sender, System.EventArgs e)
    {
        if (KitchenGameManager.Instance.IsGameOver())
        {
            Show();
            recipesDeliveredText.text = DeliveryManager.Instance.GetsuccessfulRecipesAmount().ToString();
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

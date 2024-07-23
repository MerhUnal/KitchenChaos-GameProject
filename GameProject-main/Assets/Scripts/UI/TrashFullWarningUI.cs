using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TrashFullWarningUI : MonoBehaviour
{
    [SerializeField] private TrashCounter trashCounter;
    [SerializeField] private TextMeshProUGUI warningText;

    private void Start()
    {
        trashCounter.OnProgressChanged += TrashCounter_OnProgressChanged;

        Hide();
    }

    private void TrashCounter_OnProgressChanged(object sender, IHasProgress.OnprogressChangedEventArgs e)
    {
        float trashFullShowProgressAmount = 1f; 

        bool show = e.progressNormalized >= trashFullShowProgressAmount;

        if (show)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    private void Show()
    {
        gameObject.SetActive(true);
        if (warningText != null)
        {
            warningText.gameObject.SetActive(true);
            
        }
    }

    private void Hide()
    {
        gameObject.SetActive(false);
        if (warningText != null)
        {
            warningText.gameObject.SetActive(true);
            
        }
    }
}


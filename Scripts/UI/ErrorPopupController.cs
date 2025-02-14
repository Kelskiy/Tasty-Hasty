using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ErrorPopupController : MonoBehaviour
{

    public TextMeshProUGUI errorText;

    public GameObject view;

    public void ShowNotificationText(string text)
    {
        errorText.text = text;
        view.SetActive(true);
    }

}

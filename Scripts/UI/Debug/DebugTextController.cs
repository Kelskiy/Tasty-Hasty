using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DebugTextController : MonoBehaviour
{
    public TextMeshProUGUI debugText;

    public void SetDebugText(string debugString)
    {
        debugText.text = debugString;
    }
}

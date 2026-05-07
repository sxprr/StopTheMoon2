using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QTEUI : MonoBehaviour
{
    public TMP_Text[] qteUIElements;
    public Color successColor = Color.green;
    public Color failColor = Color.red;
    public Color defaultColor = Color.white;

    public void ResetUI()
    {
        foreach (var text in qteUIElements)
        {
            text.color = defaultColor;
        }
    }

    public void UpdateStep(int index, bool isCorrect)
    {
        if (index < qteUIElements.Length)
        {
            qteUIElements[index].color = isCorrect ? successColor : failColor;
        }
    }
}

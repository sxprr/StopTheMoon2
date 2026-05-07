using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QTEManager : MonoBehaviour
{
    public KeyCode[] qteSequence = { KeyCode.W, KeyCode.A, KeyCode.S };
    private int currentIndex = 0;
    private bool isQTEActive = false;

    [SerializeField] private QTEUI qteUI; // Reference to our UI script

    public void StartQTE()
    {
        currentIndex = 0;
        isQTEActive = true;
        qteUI.ResetUI();
        Debug.Log("QTE Started!");
    }

    void Update()
    {
        if (!isQTEActive) return;

        if (Input.anyKeyDown)
        {
            if (Input.GetKeyDown(qteSequence[currentIndex]))
            {
                qteUI.UpdateStep(currentIndex, true);
                currentIndex++;

                if (currentIndex >= qteSequence.Length)
                {
                    FinishQTE(true);
                }
            }
            else
            {
                qteUI.UpdateStep(currentIndex, false);
                FinishQTE(false);
            }
        }
    }

    private void FinishQTE(bool success)
    {
        isQTEActive = false;
        if (success)
        {
            Debug.Log("QTE Complete! Triggering action...");
            // You could call an event here or a specific function
        }
        else
        {
            Debug.Log("QTE Failed.");
            currentIndex = 0;
        }
    }
}

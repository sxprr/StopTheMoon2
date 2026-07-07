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

        //please Ignore the frame if left, right or middle click was pressed.
        if(Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2))
        {
            return;
        }

        // 
        if (Input.anyKeyDown)
        {
            // 1. Log what was actually pressed vs what the sequence expected
            string pressedKey = "Unknown/Mouse";
            foreach (KeyCode k in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(k))
                {
                    pressedKey = k.ToString();
                    break;
                }
            }

            KeyCode expectedKey = qteSequence[currentIndex];
            Debug.Log($"<color=cyan>[QTE INPUT]</color> Pressed: <b>{pressedKey}</b> | Expected: <b>{expectedKey}</b> (Index: {currentIndex})");

            // Checking the qte array positions....
            if (Input.GetKeyDown(expectedKey))
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
                // 2. Log exactly why they failed (wrong key vs missing the window)
                Debug.Log($"<color=red>[QTE FAILED]</color> Input mismatch at index {currentIndex}. Striking out.");

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

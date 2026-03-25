using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class StaminaBar : MonoBehaviour
{
    public Scrollbar scrollbar;

    private void OnEnable()
    {
        // We add a new event to GameEvents specifically for data updates
        GameEvents.OnStaminaChanged += UpdateVisuals;
    }

    private void OnDisable()
    {
        GameEvents.OnStaminaChanged -= UpdateVisuals;
    }

    // This method ONLY handles the UI component
    private void UpdateVisuals(float percentage)
    {
        scrollbar.size = percentage;
        Debug.Log("UI Updated to: " + percentage);
    }


}

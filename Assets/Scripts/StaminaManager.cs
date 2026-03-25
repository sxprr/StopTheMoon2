using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StaminaManager : MonoBehaviour
{
    public float maxStamina = 100f;
    private float currentStamina;


    void Start()
    {
        currentStamina = maxStamina;

    }

    private void OnEnable()
    {
        // Start listening for the "E" press event
        GameEvents.OnMoonResist += DecreaseStamina;
    }

    private void OnDisable()
    {
        // Stop listening (Very important to prevent memory leaks!)
        GameEvents.OnMoonResist -= DecreaseStamina;
    }

    void DecreaseStamina()
    {
        currentStamina -= 5f;
        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);

        // broadcast new message
        float percent = currentStamina / maxStamina;
        GameEvents.TriggerStaminaChanged(percent);

        // Bonus: We can create a NEW event to tell the UI to update!
        // GameEvents.TriggerStaminaChanged(currentStamina / maxStamina);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class StaminaBar : MonoBehaviour
{
    public Scrollbar scrollbar;
    private float maxStaminaValue;

    public void SetMaxStamina(float stamina)
    {
        scrollbar.size = (float)stamina;
        //scrollbar.size = 1f;
        
    }

    public void SetStamina(float currentStamina)
    {
        
        if(maxStaminaValue > 0)
        {
            scrollbar.size = currentStamina / maxStaminaValue;
        }

        Debug.Log("UI Calculated Size: " + scrollbar.size);
    }

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            scrollbar.size -= 0.01f;
        }
    }
    
    
}

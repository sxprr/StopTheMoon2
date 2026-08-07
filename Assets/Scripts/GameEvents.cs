using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public static class GameEvents 
{
    // The "Shout" - fired whenever 'E' is pressed
    public static event Action OnMoonResist;
    public static event Action OnPlayerImpact;
    public static event Action OnQTEBegin;

    public static void TriggerQTEBegin() => OnQTEBegin?.Invoke();
    public static void TriggerFailure() => OnPlayerImpact?.Invoke();

    public static void TriggerMoonResist()
    {
        OnMoonResist?.Invoke();
    }

    // The "Victory" - fired when ePressesToWin is reached
    public static event Action OnVictoryAchieved;

    

    public static void TriggerVictory()
    {
        OnVictoryAchieved?.Invoke();
    }

    // The "Data Update" Event (New)
    public static event Action<float> OnStaminaChanged;
    public static void TriggerStaminaChanged(float percent) => OnStaminaChanged?.Invoke(percent);
}


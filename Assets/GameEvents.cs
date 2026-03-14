using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public static class GameEvents 
{
    // Shouted when 'E' is pressed
    public static event Action OnMoonMash;

    public static void TriggerMoonMash()
    {
        OnMoonMash?.Invoke();
    }

    // Shouted when the player wins
    public static event Action OnVictory;

    public static void TriggerVictory()
    {
        OnVictory?.Invoke();
    }


}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public static class GameEvents 
{
    // The "Shout" - fired whenever 'E' is pressed
    public static event Action OnMoonResist;

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


}

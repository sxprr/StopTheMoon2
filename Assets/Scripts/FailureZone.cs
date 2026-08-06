using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FailureZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Celestial"))
        {
            LogHandler.Log("Game over: Impact detected");
            GameEvents.TriggerFailure();
        }
    }
}

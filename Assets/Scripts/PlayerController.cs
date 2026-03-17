using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float sensX, sensY;
    float xRotation, yRotation;
    public Transform orientation;

    void Update()
    {
        // 1. Handle Camera Look (Its actual job)
        HandleLook();

        // 2. Handle Input (Just the trigger)
        if (Input.GetKeyDown(KeyCode.E))
        {
            GameEvents.TriggerMoonResist();
        }
    }

    private void HandleLook() { /* Your rotation logic here */ }
}

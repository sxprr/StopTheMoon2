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

    private void HandleLook() { /* Your rotation logic here */

        // get mouse input
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX; // Multiplying that variable lets us modify the value
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        yRotation += mouseX;

        xRotation += mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        //rotate cam and orientation
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);


    }
}

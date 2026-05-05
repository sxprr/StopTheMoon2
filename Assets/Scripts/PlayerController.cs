using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float sensX, sensY;
    float xRotation, yRotation;
    public Transform orientation;

    public KeyCode[] qteSequence = { KeyCode.W, KeyCode.A, KeyCode.S }; // The "Combo"
    private int currentIndex = 0; // Track progress

    void Update()
    {
        HandleLook();
        Debug.DrawRay(transform.position, transform.forward * 5f, Color.blue);

        // QTE Sequence Logic
        if (currentIndex < qteSequence.Length)
        {
            // Check if the player pressed ANY key this frame
            if (Input.anyKeyDown)
            {
                // Was it the RIGHT key?
                if (Input.GetKeyDown(qteSequence[currentIndex]))
                {
                    currentIndex++;
                    Debug.Log($"Correct! Step {currentIndex} of {qteSequence.Length}");

                    // Check if sequence is complete
                    if (currentIndex >= qteSequence.Length)
                    {
                        HandleRay(); // Trigger the action
                        currentIndex = 0; // Reset for next time
                    }
                }
                else
                {
                    // Optional: Penalty for wrong key
                    Debug.Log("Wrong key! Sequence reset.");
                    currentIndex = 0;
                }
            }
        }
    }

    private void HandleLook() {

        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        yRotation += mouseX;

        // CHANGE: Subtract mouseY instead of adding it
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // Apply rotations
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);

    }

    void HandleRay()
    {

        // 1. Define the Ray
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        // 2. Visual Debugging (Draws a line in the SCENE VIEW for 2 seconds)
        // It will be Green if it hits, Red if it misses.

        //getMask will get the the layer by name
        int layerMask = LayerMask.GetMask("Celestial");

        //additional parameter added to force ray to hit trigger
        bool didHit = Physics.Raycast(ray, out hit, 300f, layerMask, QueryTriggerInteraction.Collide);
        Debug.DrawRay(transform.position, transform.forward * 100f, didHit ? Color.green : Color.red, 2f);

        // 3. Logic Check
        if (didHit)
        {
            Debug.Log("<color=green>SUCCESS:</color> Raycast punched through to the Celestial layer!");
            if (hit.collider.CompareTag("Celestial"))
            {
                Debug.Log("<color=cyan>Targeting:</color> Moon Hit!");
                GameEvents.TriggerMoonResist();
            }
            else
            {
                Debug.Log($"<color=white>Targeting:</color> Hit {hit.collider.name}, but it's not the Moon.");
            }
        }
    }


}

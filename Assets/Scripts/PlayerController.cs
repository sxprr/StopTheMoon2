using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // handling player movement
    public float sensX, sensY;
    float xRotation, yRotation;
    public Transform orientation;

    [SerializeField] private QTEManager qteManager; // Connection to the manager
    [SerializeField] private Transform moonTransform;

    public float qteTriggerDistance = 160f;
    private bool qteTriggered = false;



    void Update()
    {
        // 1. Handle Camera Look
        HandleLook();

        Debug.Log("Trigger distance is " + qteTriggerDistance + ", and the boolean is now set to " + qteTriggered);

        float distanceToMoon1 = Vector3.Distance(transform.position, moonTransform.position);

        Debug.Log($"<color=pink>Moon's current distance is :</color> " + distanceToMoon1);

        Debug.DrawRay(transform.position, transform.forward * 5f, Color.blue);
        // Instead of doing logic here, we just tell the manager to go!
        // 2. Interaction Trigger
      
        // activate QTE when the moon (it's transform) reaches a certain distance

        if (!qteTriggered && moonTransform != null)
        {
            float distanceToMoon = Vector3.Distance(transform.position, moonTransform.position);

            //Debug.Log($"[Distance Tracker] Moon is {distanceToMoon} units away. Target threshold: {qteTriggerDistance}");

            // NOT FIRING DESPITE TWEAKING DISTANCE AAAAAAAAAA
            if (distanceToMoon <= qteTriggerDistance)
            {
                // 1. Log the values BEFORE changing the state
                Debug.Log($"<color=yellow>QTE TRIGGER CRITERIA MET!</color> Current Distance: {distanceToMoon} <= Target Distance: {qteTriggerDistance}. Initial qteTriggered state: {qteTriggered}");

                qteTriggered = true; // Lock the gate

                // 2. Log the values AFTER changing the state
                Debug.Log($"<color=orange>GATE LOCKED.</color> qteTriggered is now: {qteTriggered}. QTE System starting.");

                qteManager.StartQTE();
                HandleRay();

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

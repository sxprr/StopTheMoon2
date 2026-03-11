using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    //This Playercam script also controls player input.
    public float sensX;
    public float sensY;

    float xRotation;
    float yRotation;

    public Transform orientation;

    //moon manipulation
    public GameObject moon;
    private Rigidbody LeMoon;
    private Transform MoonSize;
    Vector3 MoonScale;

    private int DescentCount = 0;
   
    //the button press sfx
    public AudioSource ButtonPress;

    //ScoreManager object
    public ScoreManager Scoring;

    //stamina bar
    public StaminaBar staminaBar;
    public float MaxStamina = 100f;
    public float currentStamina;

    [Header("Victory Settings")]
    public int ePressesToWin = 100;
    private int currentPresses = 0;
    public MenuFunctionality Success;
    
   


    // Start is called before the first frame update
    private void Start()
    {
       
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        LeMoon = moon.GetComponent<Rigidbody>();
        MoonSize = moon.GetComponent<Transform>();
        currentStamina = MaxStamina;
        staminaBar.SetMaxStamina(MaxStamina);
        
    }

    // Update is called once per frame
    void Update()
    {
        // get mouse input
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX; // Multiplying that variable lets us modify the value
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        yRotation += mouseX;

        xRotation += mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        //rotate cam and orientation
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);

        // the "F" was purely for debugging. will remove later.
        // who knows, could be used for multiplayer.
        /*
        if(Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("F pressed.");
            LeMoon.mass -= 0.1f;
            LeMoon.drag -= 0.1f;
            Debug.Log(LeMoon.mass);
            Debug.Log(LeMoon.drag);
        }
        */

        if (Input.GetKeyDown(KeyCode.E))
        {
            // 1. EXIT if game is over (stops the post-death scoring bug)
            // 2. EXIT if stamina is too low (prevents 'Exert' from going negative)
            if (MenuFunctionality.isGameOver || currentStamina <= 0)
            {
                return;
            }

            // --- Logic only runs if the checks above pass ---

            currentPresses++;

            if (currentPresses >= ePressesToWin)
            {
                TriggerVictory();
            }

            // Increase Moon mass and drag
            LeMoon.mass += 0.1f;
            LeMoon.drag += 0.1f;

            // Score
            Scoring.UpdateScore(1f);

            // SFX
            ButtonPress.Play();

            // Decrement stamina
            Exert(1);

            // Debugs (Optional: remove these for the Final Build to save performance)
            Debug.Log($"Moon slowed! Mass: {LeMoon.mass}, Drag: {LeMoon.drag}, Stamina: {currentStamina}");
        }

        /*
        if (Input.GetKeyDown(KeyCode.C))
        {
            Debug.Log("C pressed.");
            LeMoon.mass += 0.1f;
            Debug.Log(LeMoon.mass);
            MoonSize.transform.localScale += MoonScale;

        }
        */
    }

    public void TrackDescentCount()
    {
        DescentCount += 1;
        Debug.Log("How many times 'descend' was pressed: " + DescentCount);
    }

    void Exert(float effort)
    {
        currentStamina -= effort;
        staminaBar.SetStamina(currentStamina);
    }

    void TriggerVictory()
    {
        // 1. Stop all movement immediately
        //LeMoon.linearVelocity = Vector3.zero;
        LeMoon.angularVelocity = Vector3.zero;

        // 2. Disable gravity so it doesn't start falling again
        LeMoon.useGravity = false;

        // 3. Optional: Make it Kinematic so nothing can push it anymore
        LeMoon.isKinematic = true;

        // 5. Block further input (Reuse your Game Over flag!)
        MenuFunctionality.isGameOver = true;

        Debug.Log("THE MOON HAS STOPPED. YOU WIN!");

        // There's probably a more effecient way to call this method.
        Success.DisplayVictory();

        //Change moon color:
        LeMoon.GetComponent<Renderer>().material.color = new Color32(90, 215, 255, 255);

        //Update high score
        Scoring.UpdateHighScore();
    }
}

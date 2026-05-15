using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MoonController : MonoBehaviour
{
    // player reference
    public GameObject Player;

    //moon event reference (DEPRECATED references, PLEASE IGNORE FOR NOW.)
    public GameManager game;
    public UnityEvent playerApproach;

    // moon rigidbody reference required for raycast
    private Rigidbody rb;

    [SerializeField] private float shakeHeight;
    private bool hasTriggeredShake = false;

    // Inside MoonController or MoonPhysics
    public ParticleSystem airResistanceFX;


    // Start is called before the first frame update
    void Start()
    {
        Rigidbody MoonRg = Player.GetComponent<Rigidbody>();

        //Set color when scene starts
        GetComponent<Renderer>().material.color = new Color32(229, 14, 0, 255);
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //TODO: Wrap the Cinemachine shake logic in a function.

        // 1. Calculate the bottom of the moon (Scale is 100, so radius is 50)
        float moonRadius = transform.lossyScale.y / 2f;
        Vector3 raystart = transform.position + (Vector3.down * moonRadius);

        // 2. The ray now starts at the surface and looks 'shakeHeight' further down
        Ray landingRay = new Ray(raystart, Vector3.down);

        // Visualize it: This should now start at the bottom of the moon
        Debug.DrawRay(raystart, Vector3.down * shakeHeight, Color.green);

        if (Physics.Raycast(landingRay, out RaycastHit hit, shakeHeight))
        {
            if (!hasTriggeredShake && hit.collider.CompareTag("Environment"))
            {
                Debug.Log("DETECTION: The moon surface is " + hit.distance + " units from the " + hit.collider.name);

                playerApproach.Invoke();
                hasTriggeredShake = true;
            }
        }
    }

    // events and listeners for EVENT BUS
    private void OnEnable()
    {
        // Start listening for the "E" press
        GameEvents.OnMoonResist += IncreaseMoonMassAndDrag;
        GameEvents.OnVictoryAchieved += FreezeMoon; // New Listener
    }

    private void OnDisable()
    {
        // Always unsubscribe to prevent errors when switching scenes
        GameEvents.OnMoonResist -= IncreaseMoonMassAndDrag;
        GameEvents.OnVictoryAchieved -= FreezeMoon;
    }

    // methods that will be listening.
    public void IncreaseMoonMassAndDrag()
    {
        rb.mass += 0.1f;
        rb.drag += 0.1f;

        // TRACKER: This tells us the event successfully reached the Moon.
        // It also prints the current values so you can see if 0.1 is "enough".
        Debug.Log($"<color=cyan>Moon Resist:</color> Event Received! New Mass: {rb.mass} | New Drag: {rb.drag}");

        // Increase particle density as resistance grows
        var emission = airResistanceFX.emission;
        emission.rateOverTime = rb.drag * 10f;
        var main = airResistanceFX.main;



        // Scale the size and rate based on how fast the moon is plummeting
        float speed = rb.velocity.magnitude;
        main.startSize = speed * 0.0f; // Faster = Bigger flames
        emission.rateOverTime = speed * 5f; // Faster = More fire

    }

    void FreezeMoon()
    {
        // 1. Kill the current movement immediately
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // 2. Stop future movement
        rb.useGravity = false;
        rb.isKinematic = true;

        // 3. Visual confirmation
        GetComponent<Renderer>().material.color = new Color32(90, 215, 255, 255);

        Debug.Log("<color=cyan>Moon Logic:</color> Velocity killed. Moon is officially frozen.");
    }
}

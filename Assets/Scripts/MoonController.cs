using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MoonController : MonoBehaviour
{

    public GameObject Player;
    public GameManager game;
    public UnityEvent playerApproach;

    [SerializeField] private float shakeHeight;
    private bool hasTriggeredShake = false;
    

    // Start is called before the first frame update
    void Start()
    {
        Rigidbody MoonRg = Player.GetComponent<Rigidbody>();
        //Set color when scene starts
        GetComponent<Renderer>().material.color = new Color32(229, 14, 0, 255);
    }

    // Update is called once per frame
    void Update()
    {
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

    // When the moon hits player. (Idek if this works or not.)
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Moon collides with player and is sent back to menu");
            game.OnGameStateChanged.Invoke();
           
        }
    }
}

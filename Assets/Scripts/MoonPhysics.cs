using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonPhysics : MonoBehaviour
{
    private Rigidbody rb;
    public float pushForce = 5f;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        GameEvents.OnMoonResist += ApplyResistance;
    }

    private void OnDisable()
    {
        GameEvents.OnMoonResist -= ApplyResistance;
    }

    void ApplyResistance()
    {
        // Every time 'E' is pressed, the Moon pushes itself up!
        rb.AddForce(Vector3.up * pushForce, ForceMode.Impulse);
    }

}

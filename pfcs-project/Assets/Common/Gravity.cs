using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Gravity : MonoBehaviour
{
    public const float G = 10;
    
    public float mass = 50;
    [SerializeField]
    private Vector3 speed = Vector3.zero;

    protected Vector3 Speed
    {
        get => speed;
        set => speed = value;
    }
    
    public GameObject rotCenter;

    private List<Gravity> _otherPlanets;

    // Start is called before the first frame update
    protected void Start()
    {
        FindPlanets();
        if (rotCenter == null) return;
        var distance = rotCenter.transform.position - transform.position;
        var force = G * mass * rotCenter.GetComponent<Gravity>().mass /
                        Mathf.Pow(distance.magnitude, 2);
        var omega = Mathf.Sqrt(force / (distance.magnitude * mass));
        speed = new Vector3(distance.z, 0, - distance.x).normalized * distance.magnitude * omega;
    }

    protected void FindPlanets()
    {
        _otherPlanets = FindObjectsOfType<Gravity>().ToList();
        _otherPlanets.Remove(this);
        
    }

    private void FixedUpdate()
    {
        UpdateGravitySpeed();
        ApplySpeed();
    }

    protected void ApplySpeed()
    {
        transform.position += Speed * Time.deltaTime;
    }

    protected void UpdateGravitySpeed()
    {
        var totalForce = Vector3.zero;
        _otherPlanets.ForEach(planet =>
        {
            // F = G * M1 * M2 / R^2
            var distance = planet.transform.position - transform.position;
            var force = G * mass * planet.mass /
                        Mathf.Pow(distance.magnitude, 2);
            totalForce += distance.normalized * force;
        });
        Speed += totalForce / mass * Time.deltaTime;
    }
}

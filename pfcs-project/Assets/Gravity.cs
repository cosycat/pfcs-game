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
    public Vector3 Speed
    {
        get => speed;
        set => speed = value;
    }

    private List<Gravity> _otherPlanets;

    

    // Start is called before the first frame update
    protected void Start()
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

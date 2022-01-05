using System;
using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;

public class Rocket : Gravity
{
    private bool _isAccelerating = false;
    public bool IsAccelerating
    {
        get => _isAccelerating;
        private set
        {
            switch (_isAccelerating)
            {
                case true when !value:
                    // Stopped accelerating
                    fireParticleSystem.Stop();
                    break;
                case false when value:
                    // Started accelerating
                    fireParticleSystem.Play();
                    break;
            }

            _isAccelerating = value;
        }
    }

    // private Vector3 speed = new Vector3(0, 0, 0);
    public float accForward = 1f;
    public float accBackward = 0.1f;
  	public GameObject goal;

    [SerializeField] private ParticleSystem fireParticleSystem;

    private void Awake()
    {
        var main = fireParticleSystem.main;
        main.startDelay = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 acceleration = new Vector3(0, 0, 0);
        Vector3 forward = transform.rotation * Vector3.forward;

        if (Input.GetKey(KeyCode.W))
        {
            acceleration += forward * accForward;
            IsAccelerating = true;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            acceleration += -forward * accBackward;
            IsAccelerating = false;
        }
        else
        {
            IsAccelerating = false;
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Rotate(0, -1, 0, Space.Self);
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Rotate(0, 1, 0, Space.Self);
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.Rotate(-1, 0, 0, Space.Self);
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.Rotate(1, 0, 0, Space.Self);
        }
        
        Debug.DrawLine(transform.position, transform.position + Speed * 10, Color.white);
        Debug.DrawLine(transform.position, goal.transform.position, Color.green);
        
        Speed += acceleration * Time.fixedDeltaTime;

        UpdateGravitySpeed();
        ApplySpeed();
    }

	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject == goal)
		{
			ScoreTracker.Instance.LoadNextLevel();
		// TODO: Load next level, or game over
		}
		else
        {
            ScoreTracker.Instance.LoadGameOver();
        }
	}


}

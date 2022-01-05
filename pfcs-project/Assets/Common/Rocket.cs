using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : Gravity
{
    // private Vector3 speed = new Vector3(0, 0, 0);
    public float accForward = 1f;
    public float accBackward = 0.1f;
	public GameObject goal;

    [SerializeField] private ParticleSystem fireParticleSystem;


    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 acceleration = new Vector3(0, 0, 0);
        Vector3 forward = transform.rotation * Vector3.forward;

        if (Input.GetKey(KeyCode.W))
        {
            acceleration += forward * accForward;
            if (!fireParticleSystem.isPlaying)
            {
                fireParticleSystem.Play();
            }
        }
        else if (Input.GetKey(KeyCode.S))
        {
            acceleration += -forward * accBackward;
        }
        else
        {
            if (fireParticleSystem.isPlaying)
            {
                fireParticleSystem.Stop();
            }
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrubHead : MonoBehaviour {

    public float turnSpeed = 1f;
    public float maxTurnAngle = 45;
    public GrubSegment previousSegment;

    public float ANGLE;

	void Start ()
    {
		
	}
	
	void Update ()
    {
        ANGLE = Vector3.SignedAngle(transform.forward, transform.forward, transform.up);

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            if(ANGLE < maxTurnAngle)
            {
                transform.RotateAround(transform.position, transform.up, -turnSpeed * Time.deltaTime);
            }
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            if (ANGLE > -maxTurnAngle)
            {
                transform.RotateAround(transform.position, transform.up, turnSpeed * Time.deltaTime);
            }
        }
	}
}

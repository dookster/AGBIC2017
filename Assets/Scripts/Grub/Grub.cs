using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grub : MonoBehaviour {

    public List<GrubSegment> segments;

    public bool moveButtonDown = false;
    public bool canMoveAgain = true;

    public float distanceFrontToBack = 0f;

	void Start ()
    {
		for(int n = 0; n < segments.Count; n++)
        {
            if(n < segments.Count - 1)
            {
                segments[n].nextSegment = segments[n + 1];
            }
            segments[n].grub = this;
            if (n == 0) segments[n].isBehind = true;
        }
	}
	
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) && canMoveAgain)
        {
            //segments[0].buttonDown = true;
            moveButtonDown = true;
            canMoveAgain = false;
        }
        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            //segments[0].buttonDown = false;
            moveButtonDown = false;
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            // move rear segment
            //segments[0].Grow();
            if (NoSegmentsMoving() && canMoveAgain)
            {
                moveButtonDown = true;
                canMoveAgain = false;
            }
        }
        else
        {
        }

        distanceFrontToBack = Vector3.Distance(segments[0].transform.position, segments[segments.Count - 1].transform.position);

        segments[0].growing = moveButtonDown;
	}

    private bool NoSegmentsMoving()
    {
        foreach (GrubSegment gs in segments)
        {
            if (gs.growing)
            {
                return false;
            }
        }
        return true;
    }

}

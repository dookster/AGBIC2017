using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grub : MonoBehaviour {

    public List<GrubSegment> segments;

    public bool moveButtonDown = false;

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
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            //segments[0].buttonDown = true;
        }
        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            //segments[0].buttonDown = false;
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            // move rear segment
            //segments[0].Grow();
            moveButtonDown = true;
        }
        else
        {
            moveButtonDown = false;
        }

        segments[0].growing = moveButtonDown;
	}

}

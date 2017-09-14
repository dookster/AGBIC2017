using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrubSegment : MonoBehaviour {

    public enum GrubState { Growing, Shrinking, Idle }
    public GrubState currentState = GrubState.Idle;

    public GrubSegment nextSegment;

    public Vector3 growScale = Vector3.one;
    public float growSpeed = 1f;
    public float shrinkSpeed = 1f;

    float growMod = 0;
    float currentGrowTarget = 0;
    Vector3 originalScale;

    [HideInInspector]
    public Grub grub;

    public bool growing = false;
    public bool isBehind = false;

	void Start ()
    {
        originalScale = transform.localScale;
	}
	
	void Update ()
    {
        switch (currentState)
        {
            case GrubState.Growing:
                Grow();
                break;
            case GrubState.Shrinking:
                Shrink();
                break;
            case GrubState.Idle:
                break;
        }

        if (growing)
        {
            Grow();
        }
        else
        {
            Shrink();
        }
        transform.localScale = originalScale + (growMod * growScale);

        growing = false;
	}

    private void LateUpdate()
    {
    }

    //public void GrowToSize(float target)
    //{
    //    if (target > 1f) Debug.LogWarning("Grow target larger than 1, wtf?");
    //    currentGrowTarget = target;
    //    currentState = GrubState.Growing;
    //}

    public void Grow()
    {
        if(growMod < 1)
        {
            growMod += Time.deltaTime * growSpeed;
        }
        else
        {
            if (isBehind) grub.moveButtonDown = false;
            growing = false;
        }
    }

    public void Shrink()
    {
        if (growMod > 0)
        {
            growMod -= Time.deltaTime * shrinkSpeed;
            if (nextSegment != null)
            {
                nextSegment.growing = true;
            }
            // move
            transform.Translate(transform.right * growScale.x * Time.deltaTime * growSpeed);
        }
        else
        {
            if (nextSegment != null)
            {
                nextSegment.growing = false;
            }
            growing = false;
        }
    }

}

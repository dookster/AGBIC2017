using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewGrubSegment : MonoBehaviour {

    public NewGrub grub;
    public NewGrubSegment nextSegment;

    public Transform graphicTransform; 

    public bool moving = false;

    public float CurMoveDelta = 0f;

    private Vector3 startPosition;
    private Quaternion startRotation;
    private Vector3 moveTarget;
    private Vector3 originalScale;

    private Vector3 planetNormal; // direction from this to planet center

    private Ray groundRay;
    private RaycastHit hitInfo;

    private int planetLayerMask;

    void Start ()
    {
        planetLayerMask = 1 << LayerMask.NameToLayer("Planet");
        startPosition = transform.position;
        startRotation = transform.localRotation;
        planetNormal = transform.up;
        originalScale = graphicTransform.localScale;
        //startForward = transform.forward;
        PlaceOnGround();
	}
	
	void Update ()
    {
        if (moving)
        {
            //CurMoveDelta += Time.deltaTime * grub.moveSpeed * (nextSegment == null ? 0.5f : 1f);
            CurMoveDelta += Time.deltaTime * grub.moveSpeed;
            float moveDeltaInterpolated = Interpolators.interpolate(CurMoveDelta, grub.moveInterpolator); 

            if (nextSegment != null && !nextSegment.moving && CurMoveDelta >= grub.nextMoveDelta)
            {
                nextSegment.StartMoving();
            }
            if (CurMoveDelta >= 1)
            {
                // done moving
                CurMoveDelta = 1;
                
                if (nextSegment == null)
                {
                    grub.IsMoving = false;
                }
                else
                {
                    //nextSegment.StartMoving();
                }
                moving = false;
            }

            // move
            if(nextSegment != null)
            {
                
                transform.position = Vector3.Lerp(startPosition, moveTarget, moveDeltaInterpolated);
                // Turn to face the direction of movement
                transform.localRotation = Quaternion.Lerp(startRotation, nextSegment.transform.localRotation, moveDeltaInterpolated);
            }
            else
            {
                transform.position = Vector3.Lerp(startPosition, startPosition + (Vector3.ProjectOnPlane(transform.forward, planetNormal) * grub.moveAmount), moveDeltaInterpolated);
            }

            // Squish
            if(CurMoveDelta < 0.5f)
            {
                graphicTransform.localScale = Vector3.Lerp(originalScale, Vector3.Scale(originalScale, grub.squishAmount), Interpolators.interpolate(CurMoveDelta * 2, grub.squishInterpolator));
            }
            else
            {
                graphicTransform.localScale = Vector3.Lerp(Vector3.Scale(originalScale, grub.squishAmount), originalScale, Interpolators.interpolate((CurMoveDelta - 0.5f) * 2, grub.squishInterpolator));
            }
            PlaceOnGround();
        }

	}

    public void PlaceOnGround()
    {
        // Place on ground
        planetNormal = (grub.currentPlanet.transform.position - transform.position).normalized;
        Debug.DrawRay(transform.position - (planetNormal * 5), planetNormal * 20f, Color.red, 1);
        //if (Physics.Raycast(transform.position - (planetNormal * 5), planetNormal, out hitInfo, 100f, planetLayerMask))
        
        if (Physics.SphereCast(transform.position - (planetNormal * 5), 0.5f, planetNormal, out hitInfo, 100f, planetLayerMask))
        {
            //Debug.DrawRay(hitInfo.point, hitInfo.normal, Color.blue, 1);
            DebugExtension.DebugWireSphere(hitInfo.point, Color.magenta, 0.5f, 0.5f);
            transform.position = hitInfo.point;
            transform.localRotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(transform.forward, hitInfo.normal), hitInfo.normal);
        }
    }

    private void LateUpdate()
    {
        
    }

    public void StartMoving()
    {
        moving = true;
        CurMoveDelta = 0f;
        startPosition = transform.position;
        startRotation = transform.localRotation;
        if(nextSegment != null)
        {
            moveTarget = nextSegment.transform.position;
        }

        //startForward = transform.forward;

    }

    Coroutine eatAnimation;
    public void PlaytEatAnimation()
    {
        if(eatAnimation == null)
        {
            eatAnimation = StartCoroutine(PlayEatAnimationRoutine());
        }
    }

    IEnumerator PlayEatAnimationRoutine()
    {
        float t = 0;
        Vector3 eatScale = Vector3.one;
        while (t < 1)
        {
            t += Time.deltaTime * grub.eatAnimSpeed;
            eatScale = Vector3.Lerp(Vector3.one, grub.eatAnimSquish, t);
            transform.localScale = eatScale;
            yield return 0;
        }
        t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * grub.eatAnimSpeed;
            eatScale = Vector3.Lerp(grub.eatAnimSquish, Vector3.one, t);
            transform.localScale = eatScale;
            yield return 0;
        }
        transform.localScale = Vector3.one;
        eatAnimation = null;
    }

}

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

    public TextMesh textMesh;

    float moveFactor = 2f;
    float growMod = 0;
    float currentGrowTarget = 0;
    Vector3 originalScale;
    Vector3 positionMod;

    private Material mat;

    [HideInInspector]
    public Grub grub;

    public bool growing = false;
    public bool isBehind = false;

    private Vector3 lastRestingPos;
    private GameMaster gM;

	void Start ()
    {
        originalScale = transform.localScale;
        mat = GetComponent<MeshRenderer>().material;
        gM = GameMaster.Instance;
        lastRestingPos = transform.localPosition;
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

        // Turning
        if(nextSegment != null)
        {
            //transform.right = nextSegment.transform.position - transform.position;
        }

        // Grow / shrink
        if (growing)
        {
            Grow();
            if (gM.DebugOn) mat.color = Color.red;
        }
        else if (growMod > 0)
        {
            Shrink();
            if (gM.DebugOn) mat.color = Color.blue;
        }
        else
        {
            // nether growing or shrinking, remember pos so we can adjust after moving
            lastRestingPos = transform.localPosition;
            if (gM.DebugOn) mat.color = Color.white;
        }
        transform.localScale = originalScale + (growMod * growScale);
        //positionMod = transform.localPosition;
        //positionMod.y = (growMod / 4f) - 1f;
        //transform.localPosition = positionMod;

        growing = false;

        // debug
        if (gM.DebugOn)
        {
        }
        textMesh.text = (Mathf.Round(growMod * 100) / 100f) + "";
        //textMesh.transform.LookAt(Camera.main.transform);
	}

    private void LateUpdate()
    {
        if (nextSegment != null)
        {
            //transform.LookAt(nextSegment.transform.position + (nextSegment.transform.forward * 0));
        }

    }

    //public void GrowToSize(float target)
    //{
    //    if (target > 1f) Debug.LogWarning("Grow target larger than 1, wtf?");
    //    currentGrowTarget = target;
    //    currentState = GrubState.Growing;
    //}

    public void Grow()
    {
        if (growMod < 1)
        {
            growMod += Time.deltaTime * growSpeed;
            if (growMod >= 1)
            {
                growMod = 1;
                if (nextSegment != null)
                {
                    transform.position = lastRestingPos - (transform.forward * growScale.z * moveFactor);
                }
            }
            else
            {
                // move
                //transform.Translate(-transform.forward * growScale.z * moveFactor * Time.deltaTime * growSpeed, Space.World);
                transform.position = Vector3.Lerp(lastRestingPos, lastRestingPos - (transform.forward * growScale.z * moveFactor), growMod);
            }
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

            // if we're done shrinking and last segment, tell Grub script
            if(growMod <= 0 && nextSegment == null)
            {
                grub.canMoveAgain = true;
            }
            if (growMod <= 0)
            {
                growMod = 0;
                lastRestingPos += transform.forward * (growScale.z * moveFactor);
                //transform.localPosition = lastRestingPos;
                Debug.Log("Done shrinking");
            }
        }
        else
        {
            //if (nextSegment != null)
            //{
            //    nextSegment.growing = false;
            //}
            //growing = false;
        }
    }

}

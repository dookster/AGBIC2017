using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewGrub : MonoBehaviour {

    public List<NewGrubSegment> segments;
    public float moveSpeed = 1f;
    public float moveAmount = 2f;
    public float nextMoveDelta = 0.8f;
    public Interpolators.Type moveInterpolator;

    public Vector3 squishAmount;
    public Interpolators.Type squishInterpolator;

    public float turnSpeed = 1f;
    public float maxTurnAngle = 45;

    public GameObject currentPlanet;

    public bool IsMoving = false;

    private NewGrubSegment FrontSegment;
    private NewGrubSegment SecondSegment;

    [Header("Eating")]
    public float force = 10f;
    public float forceOffset = 0.1f;

    public Transform GrubMouth;
    public ParticleSystem particleEffect;

    public float eatTime = 0.5f;
    private float isEating = 0f;


    float angle;

	void Start ()
    {
        for (int n = 0; n < segments.Count-1; n++)
        {
            segments[n].nextSegment = segments[n + 1];
            segments[n].grub = this;
        }
        FrontSegment  = segments[segments.Count - 1];
        SecondSegment = segments[segments.Count - 2];
    }

    void Update ()
    {
        // Eat
        if(isEating <= 0)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (EatFruit())
                {
                    isEating = eatTime;
                    particleEffect.Play();
                }
            }
            
        }
        else
        {
            isEating -= Time.deltaTime;
        }

        // Move
        if (Input.GetKey(KeyCode.UpArrow))
        {
            if (!IsMoving)
            {
                segments[0].StartMoving();
                IsMoving = true;
            }
        }

        if (!FrontSegment.moving)
        {
            // TODO measure angle diff in local angles instead...
            angle = Vector3.SignedAngle(FrontSegment.transform.forward, SecondSegment.transform.forward, FrontSegment.transform.up);

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                if (angle < maxTurnAngle)
                {
                    FrontSegment.transform.Rotate(Vector3.up, -turnSpeed * Time.deltaTime, Space.Self);
                }
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                if (angle > -maxTurnAngle)
                {
                    FrontSegment.transform.Rotate(Vector3.up, turnSpeed * Time.deltaTime, Space.Self);
                }

            }
        }
    }

    bool EatFruit()
    {
        Vector3 planetNormal = (currentPlanet.transform.position - GrubMouth.transform.position).normalized;
        Debug.DrawRay(GrubMouth.position - planetNormal * 5, planetNormal * 20, Color.green, 3);
        Debug.DrawRay(GrubMouth.position - planetNormal * 5, planetNormal * 10, Color.red, 2);
        Ray inputRay = new Ray(GrubMouth.position -  planetNormal * 5, planetNormal * 20);
        RaycastHit hit;
        //if (Physics.Raycast(inputRay, out hit, 20f, 1 << LayerMask.NameToLayer("Planet")))
        if (Physics.SphereCast(inputRay, 0.7f, out hit, 20f, 1 << LayerMask.NameToLayer("Planet")))
        {
            MeshDeformer deformer = hit.collider.GetComponent<MeshDeformer>();
            if (deformer)
            {
                Vector3 point = hit.point;
                point += hit.normal * forceOffset;
                //deformer.AddDeformingForce(point, force);
                DebugExtension.DebugWireSphere(point, Color.red, 0.7f, 1f);
                deformer.EatAtPoint(point, force);
            }
            MeshEater eater = hit.collider.GetComponent<MeshEater>();
            if (eater)
            {
                Vector3 point = hit.point;
                eater.EatVertex(point, hit.normal);
            }
            return true;
        }
        return false;
    }


}

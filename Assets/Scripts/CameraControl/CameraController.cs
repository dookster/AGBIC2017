using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public float moveSpeed;
    public float camHeight;
    public Transform cameraTarget;
    public Transform mainCamera;
    public NewGrub grub;
    public Transform frontLookTarget;

    public float grubAngle = 0;

	void Start ()
    {
		
	}
	
	void Update ()
    {
        transform.rotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(transform.position - frontLookTarget.position, Vector3.up), Vector3.up);

        Vector3 targetPos = cameraTarget.position;
        targetPos.y = meanPosition().y + 2f;
        cameraTarget.position = targetPos;

        mainCamera.position = Vector3.Slerp(mainCamera.position, cameraTarget.position, Time.deltaTime * moveSpeed);

        // look at grub head
        Vector3 lookTarget = grub.segments[grub.segments.Count - 1].transform.position - mainCamera.position;
        Quaternion lookRot = Quaternion.LookRotation(lookTarget);
        mainCamera.rotation = Quaternion.Lerp(mainCamera.rotation, lookRot, Time.deltaTime);
        
        //mainCamera.LookAt(grub.segments[grub.segments.Count-1].transform, Vector3.up);
        
	}

    private Vector3 meanPosition()
    {
        Vector3 result = Vector3.zero;
        int count = 0;
        for(int n = 2; n < grub.segments.Count; n++)
        {
            result += grub.segments[n].transform.position;
            count++;
        }
        result += frontLookTarget.position;
        return result / (count + 1);
    }

}

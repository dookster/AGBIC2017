using UnityEngine;

public class MeshDeformerInput : MonoBehaviour {
	
	public float force = 10f;
	public float forceOffset = 0.1f;

    public Transform GrubMouth;
    public ParticleSystem particleEffect;

    public float eatTime = 0.5f;
    private float isEating = 0f;

	void Update ()
    {
		if (Input.GetMouseButtonDown(0))
        {
			//HandleInput();
		}

        if (Input.GetKeyDown(KeyCode.Space))
        {
            EatAtGrub();
        }
	}

    void EatAtGrub()
    {
        Ray inputRay = new Ray(GrubMouth.position + GrubMouth.up, -GrubMouth.up * 3);
        RaycastHit hit;

        if (Physics.Raycast(inputRay, out hit))
        {
            MeshDeformer deformer = hit.collider.GetComponent<MeshDeformer>();
            if (deformer)
            {
                Vector3 point = hit.point;
                point += hit.normal * forceOffset;
                //deformer.AddDeformingForce(point, force);
                deformer.EatAtPoint(point, force);
            }
            MeshEater eater = hit.collider.GetComponent<MeshEater>();
            if (eater)
            {
                Vector3 point = hit.point;
                eater.EatVertex(point, hit.normal);
            }
        }
    }

	void HandleInput ()
    {
		Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		
		if (Physics.Raycast(inputRay, out hit))
        {
			MeshDeformer deformer = hit.collider.GetComponent<MeshDeformer>();
			if (deformer)
            {
				Vector3 point = hit.point;
				point += hit.normal * forceOffset;
                //deformer.AddDeformingForce(point, force);
                deformer.EatAtPoint(point, force);
			}
            MeshEater eater = hit.collider.GetComponent<MeshEater>();
            if (eater)
            {
                Vector3 point = hit.point;
                eater.EatVertex(point, hit.normal);
            }
		}
	}
}

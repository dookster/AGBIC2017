using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshDeformer : MonoBehaviour {

	public float springForce = 20f;
	public float damping = 5f;

	Mesh deformingMesh;
	Vector3[] originalVertices, displacedVertices;
	Vector3[] vertexVelocities;

    MeshCollider meshCollider;

	float uniformScale = 1f;

	void Start () {
        meshCollider = GetComponent<MeshCollider>();
		deformingMesh = GetComponent<MeshFilter>().mesh;
		originalVertices = deformingMesh.vertices;
		displacedVertices = new Vector3[originalVertices.Length];
		for (int i = 0; i < originalVertices.Length; i++) {
			displacedVertices[i] = originalVertices[i];
		}
		vertexVelocities = new Vector3[originalVertices.Length];
	}

	void Update () {
		uniformScale = transform.localScale.x;
		for (int i = 0; i < displacedVertices.Length; i++) {
			//UpdateVertex(i);
		}

        deformingMesh.vertices = displacedVertices;
		deformingMesh.RecalculateNormals();
        deformingMesh.RecalculateTangents();

        meshCollider.sharedMesh = null;
        meshCollider.sharedMesh = deformingMesh;
    }

	void UpdateVertex (int i) {
		Vector3 velocity = vertexVelocities[i];
        //Vector3 displacement = displacedVertices[i] - originalVertices[i];
        //displacement *= uniformScale;
        //velocity -= displacement * springForce * Time.deltaTime;
        //velocity *= 1f - damping * Time.deltaTime;
        
        if(Vector3.Distance(displacedVertices[i], Vector3.zero) < 0.9f)
        {
            return;
        }

        vertexVelocities[i] = velocity;
		displacedVertices[i] += velocity * (Time.deltaTime / uniformScale);
	}

	public void AddDeformingForce (Vector3 point, float force) {
		point = transform.InverseTransformPoint(point);
		for (int i = 0; i < displacedVertices.Length; i++) {
		//	AddForceToVertex(i, point, force);
		}
	}

    public void EatAtPoint(Vector3 point, float amount)
    {
        point = transform.InverseTransformPoint(point);
        for (int i = 0; i < displacedVertices.Length; i++)
        {
            EatAtVertex(i, point, amount);
        }
    }

    void EatAtVertex(int i, Vector3 point, float amount)
    {
        
        Vector3 pointToVertex = displacedVertices[i] - point;
        if (pointToVertex.sqrMagnitude > 0.05f) return; // Too far away to be affected

        pointToVertex *= uniformScale;
        float attenuatedAmount = amount / (1f + pointToVertex.sqrMagnitude);
        Debug.Log("Amount: " + attenuatedAmount);
        //displacedVertices[i] = originalVertices[i] + ((Vector3.zero - originalVertices[i]).normalized * attenuatedAmount);
        Vector3 newVert = displacedVertices[i] + ((Vector3.zero - originalVertices[i]).normalized * attenuatedAmount);
        if(Vector3.Distance(newVert, originalVertices[i]) < 0.2f)
        {
            displacedVertices[i] = newVert;
        }
    }

  //  void AddForceToVertex (int i, Vector3 point, float force) {
		//Vector3 pointToVertex = displacedVertices[i] - point;
  //      if (pointToVertex.sqrMagnitude > 0.05f) return;
		//pointToVertex *= uniformScale;
		//float attenuatedForce = force / (1f + pointToVertex.sqrMagnitude);
		//float velocity = attenuatedForce * Time.deltaTime;
  //      //vertexVelocities[i] += pointToVertex.normalized * velocity;
  //      vertexVelocities[i] += (Vector3.zero - displacedVertices[i]).normalized * velocity;
  //  }

    //void AddForceToVertex(int i, Vector3 point, float force)
    //{
    //    Vector3 pointToVertex = displacedVertices[i] - point;
    //    pointToVertex *= uniformScale;
    //    float attenuatedForce = force / (1f + pointToVertex.sqrMagnitude);
    //    float velocity = attenuatedForce * Time.deltaTime;
    //    vertexVelocities[i] += pointToVertex.normalized * velocity;
    //}
}

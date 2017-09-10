using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshEater : MonoBehaviour {

    public float springForce = 20f;
    public float damping = 5f;

    Mesh deformingMesh;
    Vector3[] originalVertices, displacedVertices;

    MeshCollider meshCollider;

    HashSet<int> eatenVerts = new HashSet<int>();

    float uniformScale = 1f;

    void Start()
    {
        meshCollider = GetComponent<MeshCollider>();
        deformingMesh = GetComponent<MeshFilter>().mesh;
        originalVertices = deformingMesh.vertices;
        displacedVertices = new Vector3[originalVertices.Length];
        for (int i = 0; i < originalVertices.Length; i++)
        {
            displacedVertices[i] = originalVertices[i];
        }
    }

    void Update()
    {
        //uniformScale = transform.localScale.x;
        //for (int i = 0; i < displacedVertices.Length; i++)
        //{
        //    UpdateVertex(i);
        //}

        //deformingMesh.vertices = displacedVertices;
        //deformingMesh.RecalculateNormals();

        //meshCollider.sharedMesh = null;
        //meshCollider.sharedMesh = deformingMesh;
    }

    public void EatVertex(Vector3 point, Vector3 normal)
    {
        point = transform.InverseTransformPoint(point);
        int eatVertex = GetNearestVertex(point);

        if (eatenVerts.Contains(eatVertex)) return;

        normal = deformingMesh.normals[eatVertex] * 0.002f;

        for (int i = 1; i < displacedVertices.Length; i++)
        {
            if(displacedVertices[i] == displacedVertices[eatVertex])
            {
                displacedVertices[i] = displacedVertices[i] - normal;
                eatenVerts.Add(i);
            }
        }

        //displacedVertices[eatVertex] = displacedVertices[eatVertex] - normal;

        deformingMesh.vertices = displacedVertices;
        deformingMesh.RecalculateNormals();
        deformingMesh.RecalculateTangents();

        meshCollider.sharedMesh = null;
        meshCollider.sharedMesh = deformingMesh;

        eatenVerts.Add(eatVertex);

        //for (int i = 0; i < displacedVertices.Length; i++)
        //{

        //}
    }

    int GetNearestVertex(Vector3 point)
    {
        int result = 0;
        float smallestDist = Vector3.Distance(point, displacedVertices[0]);
        for (int i = 1; i < displacedVertices.Length; i++)
        {
            float d = Vector3.Distance(point, displacedVertices[i]);
            if(d < smallestDist)
            {
                smallestDist = d;
                result = i;
            }
        }
        return result;
    }

}

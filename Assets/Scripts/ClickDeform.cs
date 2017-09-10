using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class ClickDeform : MonoBehaviour {

    //private MeshFilter meshFilter;

    //Mesh deformingMesh;
    //Vector3[] originalVertices, displacedVertices;

    //float uniformScale = 1f;

    //void Start ()
    //{
    //    meshFilter = GetComponent<MeshFilter>();

    //    deformingMesh = GetComponent<MeshFilter>().mesh;
    //    originalVertices = deformingMesh.vertices;
    //    displacedVertices = new Vector3[originalVertices.Length];
    //    for (int i = 0; i < originalVertices.Length; i++)
    //    {
    //        displacedVertices[i] = originalVertices[i];
    //    }
    //}

    //void Update ()
    //{
    //    if (Input.GetMouseButton(0))
    //    {
    //        HandleInput();
    //    }

    //    uniformScale = transform.localScale.x;
    //    for (int i = 0; i < displacedVertices.Length; i++)
    //    {
    //        UpdateVertex(i);
    //    }
    //    deformingMesh.vertices = displacedVertices;
    //    deformingMesh.RecalculateNormals();
    //}

    //void HandleInput()
    //{
    //    Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
    //    RaycastHit hit;

    //    if (Physics.Raycast(inputRay, out hit))
    //    {
    //        if(hit.collider.gameObject == this)
    //        {
    //            Vector3 point = hit.point;
    //            point += hit.normal * forceOffset;
    //            deformer.AddDeformingForce(point, force);
    //        }
    //    }
    //}



}

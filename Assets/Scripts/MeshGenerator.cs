using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGenerator : MonoBehaviour
{
    private Mesh mesh;
    private MeshFilter meshFilter;
    private List<Vector3> vertices = new List<Vector3>();
    private List<int> triangles = new List<int>();
    private int triangleCnt = 0;
    public Material material;
    [HideInInspector]
    public float stemLength = 5f;
    [HideInInspector]
    public float stemThicknes = 0.5f;
    [HideInInspector]
    public Vector3 lastPositionL = Vector3.zero;
    [HideInInspector]
    public Vector3 lastPositionR = new Vector3(1, 0, 0);
    [HideInInspector]
    public Vector3 lastPosition = Vector3.zero;
    private Quaternion rotation = Quaternion.identity;
    private Quaternion lastRotation = Quaternion.identity;
    public int MeshVerticeNbr
    {
        get
        {
            return mesh.vertices.Length;
        }
    }

    void Awake()
    {
        mesh = new Mesh();
        meshFilter = gameObject.AddComponent<MeshFilter>();
        var meshRenderer = gameObject.AddComponent<MeshRenderer>();
        if (!material)
            meshRenderer.material = new Material(Shader.Find("Standard"));
        else meshRenderer.material = material;
        meshFilter.mesh = mesh;
    }

    public void Reinitialise()
    {
        vertices.Clear();
        triangles.Clear();
        triangleCnt = 0;
        lastPositionL = Vector3.zero;
        lastPositionR = new Vector3(1, 0, 0);
        lastPosition = Vector3.zero;
        rotation = Quaternion.identity;
        lastRotation = Quaternion.identity;
        UpdateMesh();
    }
    public void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();
    }

    public void CreateShape(Quaternion rotation)
    {
        var t = stemThicknes / 2;
        var l = stemLength;
        vertices.AddRange(new Vector3[]{
                lastPosition + rotation * new Vector3(-t*0.99f,0,0),
                lastPosition + rotation * new Vector3(t*0.99f,0,0),
                lastPosition + rotation * new Vector3(-t,l+t,0),
                lastPosition + rotation * new Vector3(t,l+t,0),

                lastPosition + rotation * new Vector3(0,0,-t*0.99f),
                lastPosition + rotation * new Vector3(0,0,t*0.99f),
                lastPosition + rotation * new Vector3(0,l+t,-t),
                lastPosition + rotation * new Vector3(0,l+t,t),
            });
        lastPosition = lastPosition + rotation * new Vector3(0, l, 0);
        lastRotation = rotation;
        var c = triangleCnt;
        triangles.AddRange(new List<int>{
            c,c+6,c+4,
            c,c+2,c+6,

            c+4,c+3,c+1,
            c+4,c+6,c+3,

            c+1,c+7,c+5,
            c+1,c+3,c+7,

            c+5,c+2,c,
            c+5,c+7,c+2
        });
        triangleCnt += 8;
    }
}
using System.Collections.Generic;
using UnityEngine;

public class MeshGenerator : MonoBehaviour
{
    private Mesh mesh;
    private MeshFilter meshFilter;
    private readonly List<Vector3> vertices = new ();
    private readonly List<int> triangles = new ();
    private int triangleCnt;
    public int triangleCursor;
    public Material material;
    [HideInInspector] public float stemLength = 5f;
    [HideInInspector] public float stemThickness = 0.5f;
    [HideInInspector] public Vector3 lastPosition = Vector3.zero;
    private Quaternion rotation = Quaternion.identity;
    private Quaternion lastRotation = Quaternion.identity;
    
    public int MeshVerticeNbr => mesh.vertices.Length;
    
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
        triangleCursor = 0;
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
        var t = stemThickness / 2;
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
            c+5,c+7,c+2,

            c+7,c+6,c+2,
            c+6,c+7,c+3,

            c+5,c,c+4,
            c+4,c+1,c+5,
        });
        triangleCnt += 8;
    }

    public void CreateShapeMergedVertices(Quaternion rotation)
    {
        var t = stemThickness / 2;
        var l = stemLength;
        if (vertices.Count == 0)
        {
            vertices.AddRange(new Vector3[]{
                lastPosition + rotation * new Vector3(-t,t,0),
                lastPosition + rotation * new Vector3(t,t,0),
                lastPosition + rotation * new Vector3(0,t,-t),
                lastPosition + rotation * new Vector3(0,t,t),
            });
            triangles.AddRange(new List<int>{
                1,0,2,
                0,1,3
            });
            triangleCnt = 4;
            triangleCursor = 0;
        }
        vertices.AddRange(new Vector3[]{
            lastPosition + rotation * new Vector3(-t,l+t,0),
            lastPosition + rotation * new Vector3(t,l+t,0),
            lastPosition + rotation * new Vector3(0,l+t,-t),
            lastPosition + rotation * new Vector3(0,l+t,t),
        });
        lastPosition = lastPosition + rotation * new Vector3(0, l, 0);
        lastRotation = rotation;
        var c = triangleCnt;
        var s = triangleCursor;
        triangles.AddRange(new List<int>{
            s,c+2,s+2,
            s,c,c+2,

            s+2,c+1,s+1,
            s+2,c+2,c+1,

            s+1,c+3,s+3,
            s+1,c+1,c+3,

            s+3,c,s,
            s+3,c+3,c
        });
        triangleCursor = triangleCnt;
        triangleCnt += 4;
    }

    public void CloseShape()
    {
        var c = triangleCnt;
        triangles.AddRange(new List<int>
        {
            c-2,c-1,c-3,
            c-1,c-2,c-4
        });
    }
}
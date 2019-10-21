using UnityEngine;

[RequireComponent(typeof(MeshGenerator))]
public class LSystemMeshMergedVertices : LSystem
{
    private MeshGenerator meshViz;
    public override int MeshVerticesCount
    {
        get
        {
            return meshViz.MeshVerticeNbr;
        }
    }

    protected override void Awake()
    {
        meshViz = GetComponent<MeshGenerator>();
        base.Awake();
    }

    public override void Generate()
    {
        meshViz.stemLength = parameters.stemLength;
        meshViz.stemThicknes = parameters.stemThickness;
        base.Generate();
        meshViz.UpdateMesh();
    }

    public override void Reinitialise()
    {
        base.Reinitialise();
        meshViz.Reinitialise();
    }

    protected override void Letter()
    {
        lastPosition = lastRotation * stem + lastPosition;
        meshViz.CreateShapeMergedVertices(lastRotation);
    }

    protected override void ClosingBracket()
    {
        TransformInfo ti = transformStack.Pop();
        lastPosition = ti.position;
        lastRotation = ti.rotation;
        meshViz.lastPosition = ti.position;
        meshViz.triangleCursor = ti.triangleCursor;
        meshViz.CloseShape();
    }

    protected override void OpeningBracket()
    {
        transformStack.Push(new TransformInfo()
        {
            position = lastPosition,
            rotation = lastRotation,
            triangleCursor = meshViz.triangleCursor
        });
    }
}

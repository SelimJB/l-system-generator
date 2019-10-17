using UnityEngine;

[RequireComponent(typeof(MeshGenerator))]
public class LSystemMesh : LSystem
{
    private MeshGenerator meshViz;
    
    protected override void Awake()
    {
        meshViz = GetComponent<MeshGenerator>();
        base.Awake();
    }

    public override void Generate(){
        meshViz.stemLength = parameters.stemLength;
        meshViz.stemThicknes = parameters.stemThickness;
        base.Generate();
        meshViz.UpdateMesh();
    }

    public override void Reset(){
        base.Reset();
        meshViz.Reset();
    }

    protected override void Letter()
    {
        lastPosition = lastRotation * stem + lastPosition;
        meshViz.CreateShape(lastRotation);
    }

    protected override void ClosingBracket()
    {
        TransformInfo ti = transformStack.Pop();
        lastPosition = ti.position;
        lastRotation = ti.rotation;
        meshViz.lastPosition = ti.position;
    }
}

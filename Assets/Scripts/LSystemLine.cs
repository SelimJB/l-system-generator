using System.Collections.Generic;
using UnityEngine;

public class LSystemLine : LSystem
{
    private LineRenderer lineRenderer;
    private int lineCount = 1;
    Queue<int> lineRendererPositionNbrQueue;

    protected override void Letter()
    {
        lastPosition = lastRotation * stem + lastPosition;
        lineRenderer.SetPosition(lineCount, lastPosition);
        lineCount++;
    }

    public override void Reset()
    {
        base.Reset();
    }
    protected override void ClosingBracket()
    {
        TransformInfo ti = transformStack.Pop();
        lastPosition = ti.position;
        lastRotation = ti.rotation;

        var lineRendererPositionNbr = lineRendererPositionNbrQueue.Dequeue();
        if (lineRendererPositionNbr > 0)
        {
            var child = new GameObject();
            child.transform.parent = transform;
            lineRenderer = child.AddComponent<LineRenderer>();
            lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
            lineRenderer.positionCount = lineRendererPositionNbr;
            SetupLineRenderer();
            lineCount = 1;
        }
    }

    public void SetupLineRenderer()
    {
        lineRenderer.startColor = Color.cyan;
        lineRenderer.endColor = Color.blue;
        lineRenderer.startWidth = 0.3f;
        lineRenderer.SetPosition(0, lastPosition);
        lineRenderer.startWidth = parameters.stemThickness;
        lineRenderer.loop = false;
    }

    public override void Generate()
    {
        rules = parameters.Rules;
        transform.position = parameters.position;
        stem = new Vector3(0, parameters.stemLength, 0);
        GeneratePath();

        lineRendererPositionNbrQueue = new Queue<int>();
        var lineSize = currentPath.Split(']');
        foreach (var i in lineSize)
        {
            var count = 0;
            foreach (var j in i)
            {
                if (char.IsLetter(j) && !parameters.constants.Contains(j))
                    count++;
            }
            lineRendererPositionNbrQueue.Enqueue(count + 1);
        }

        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.positionCount = lineRendererPositionNbrQueue.Dequeue();
        SetupLineRenderer();
        GenerateFigure();
    }
}
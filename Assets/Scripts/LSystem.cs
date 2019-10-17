using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System;

public abstract class LSystem : MonoBehaviour
{
    public LSystemParameters parameters;

    protected Dictionary<char, string> rules;
    protected Stack<TransformInfo> transformStack;
    protected Vector3 lastPosition = Vector3.zero;
    protected Quaternion lastRotation = Quaternion.identity;
    protected string currentPath = "";
    protected Vector3 stem;

    protected virtual void Awake()
    {
        transformStack = new Stack<TransformInfo>();
    }

    protected virtual void Start(){
        Generate();
    }
    
    public virtual void Reset()
    {
        transformStack.Clear();
        lastPosition = Vector3.zero;
        lastRotation = Quaternion.identity;
        currentPath = "";
    }

    public virtual void Generate()
    {
        rules = parameters.Rules;
        transform.position = parameters.position;
        stem = new Vector3(0, parameters.stemLength, 0);
        GeneratePath();
        GenerateFigure();
    }

    protected void GeneratePath()
    {
        currentPath = parameters.axiom;
        StringBuilder stringBuilder = new StringBuilder();
        for (int i = 0; i < parameters.iteration; i++)
        {
            // Debug.Log(String.Format("Iteration n°{0} : \t{1}", i, currentPath));
            foreach (char step in currentPath)
            {
                stringBuilder.Append(rules.ContainsKey(step) ? rules[step] : step.ToString());
            }
            currentPath = stringBuilder.ToString();
            stringBuilder = new StringBuilder();
        }
        // Debug.Log(String.Format("Iteration n°{0} : \t{1}", parameters.iteration, currentPath));
    }

    protected abstract void Letter();
    protected abstract void ClosingBracket();

    protected void GenerateFigure()
    {
        for (var i = 0; i < currentPath.Length; i++)
        {
            if (parameters.constants.Contains(currentPath[i]))
            {
                continue;
            }
            else if (Char.IsLetter(currentPath[i]))
            {
                Letter();
            }
            else
            {
                switch (currentPath[i])
                {
                    case '+':
                        lastRotation *= Quaternion.Euler(0, 0, parameters.angle);
                        break;
                    case '-':
                        lastRotation *= Quaternion.Euler(0, 0, -parameters.angle);
                        break;
                    case '*':
                        lastRotation *= Quaternion.Euler(0, parameters.angle, 0);
                        break;
                    case '/':
                        lastRotation *= Quaternion.Euler(0, -parameters.angle, 0);
                        break;
                    case '>':
                        lastRotation *= Quaternion.Euler(parameters.angle, 0, 0);
                        break;
                    case '<':
                        lastRotation *= Quaternion.Euler(-parameters.angle, 0, 0);
                        break;
                    case '[':
                        transformStack.Push(new TransformInfo()
                        {
                            position = lastPosition,
                            rotation = lastRotation
                        });
                        break;
                    case ']':
                        ClosingBracket();
                        break;
                }
            }
        }
    }
}

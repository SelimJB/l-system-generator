using UnityEngine;

[RequireComponent(typeof(LSystem))]
public class SandBox : MonoBehaviour
{
    private LSystem lSystem;
    public LSystemParameters[] exemples;

    void Start()
    {
        lSystem = GetComponent<LSystem>();
    }

    void OnGUI()
    {}
}
using UnityEngine;

[RequireComponent(typeof(LSystem))]
public class ShowRoom : MonoBehaviour
{
    private LSystem lSystem;
    public LSystemParameters[] exemples;
    private string lSystemInformations;
    
    void Start()
    {
        lSystem = GetComponent<LSystem>();
        lSystemInformations = lSystem.parameters.ToString();
    }

    void OnGUI()
    {
        var guiOptions = new GUILayoutOption[]{
            GUILayout.Width(180),
            GUILayout.Height(28),
        };

        GUILayout.BeginHorizontal();
        GUILayout.BeginVertical();

        GUILayout.TextField("Use arrows, the wheel,\nand +- buttons to naviguate\nwhen the Orbital checkbox\nis toggled of.", GUILayout.Width(180));
        CameraController.Instance.Orbit = GUILayout.Toggle(CameraController.Instance.Orbit, "Orbit");
        if (GUILayout.Button("Reset Camera", guiOptions))
        {
            CameraController.Instance.Reset();
        }
        GUILayout.Space(15f);

        foreach (var ex in exemples)
        {
            if (GUILayout.Button(ex.name, guiOptions))
            {
                lSystem.Reinitialise();
                lSystem.parameters = ex;
                lSystem.Generate();
                lSystemInformations = lSystem.parameters.ToString();
            }
        }
        GUILayout.EndVertical();

        GUILayout.Space(5f);
        GUILayout.TextField(lSystemInformations);

        GUILayout.EndHorizontal();
    }
}

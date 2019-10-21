using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(LSystem))]
public class SandBox : MonoBehaviour
{
    public LSystemParameters preset1, preset2, preset3, preset4;
    LSystem lSystem;
    Dictionary<char, string> rules = new Dictionary<char, string>();
    string constant = "X";
    string lSystemInformations;

    void Start()
    {
        lSystem = GetComponent<LSystem>();
        Preset(preset1);
    }

    void OnGUI()
    {
        // Axiom
        GUILayout.BeginHorizontal();
        GUILayout.TextArea("Axiom : ", GUILayout.Width(90));
        lSystem.parameters.axiom = GUILayout.TextArea(lSystem.parameters.axiom, GUILayout.Width(140));
        GUILayout.EndHorizontal();
        // Constant
        GUILayout.BeginHorizontal();
        GUILayout.TextArea("Constante : ", GUILayout.Width(90));
        constant = GUILayout.TextArea(constant, GUILayout.Width(140));
        GUILayout.EndHorizontal();
        // Iteration
        GUILayout.BeginHorizontal();
        GUILayout.TextArea("Iterations : " + lSystem.parameters.iteration, GUILayout.Width(90));
        lSystem.parameters.iteration = (int)GUILayout.HorizontalSlider(lSystem.parameters.iteration, 1f, 7f, GUILayout.Width(140));
        GUILayout.EndHorizontal();
        // Angle
        GUILayout.BeginHorizontal();
        GUILayout.TextArea("Angle : " + lSystem.parameters.angle, GUILayout.Width(90));
        lSystem.parameters.angle = (int)GUILayout.HorizontalSlider(lSystem.parameters.angle, 0f, 180f, GUILayout.Width(140));
        GUILayout.EndHorizontal();
        // Length
        GUILayout.BeginHorizontal();
        GUILayout.TextArea("Longueur : " + lSystem.parameters.stemLength, GUILayout.Width(90));
        lSystem.parameters.stemLength = GUILayout.HorizontalSlider(lSystem.parameters.stemLength, 0f, 5f, GUILayout.Width(140));
        GUILayout.EndHorizontal();
        // Thickness
        GUILayout.BeginHorizontal();
        GUILayout.TextArea("Largeur : " + lSystem.parameters.stemThickness, GUILayout.Width(90));
        lSystem.parameters.stemThickness = GUILayout.HorizontalSlider(lSystem.parameters.stemThickness, 0f, 4f, GUILayout.Width(140));
        GUILayout.EndHorizontal();
        // Rules
        GUILayout.BeginHorizontal();
        GUILayout.TextArea("X : ", GUILayout.Width(30));
        rules['X'] = GUILayout.TextField(rules['X'], GUILayout.Width(200));
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        GUILayout.TextArea("F : ", GUILayout.Width(30));
        rules['F'] = GUILayout.TextField(rules['F'], GUILayout.Width(200));
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        GUILayout.TextArea("G : ", GUILayout.Width(30));
        rules['G'] = GUILayout.TextField(rules['G'], GUILayout.Width(200));
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        GUILayout.TextArea("H : ", GUILayout.Width(30));
        rules['H'] = GUILayout.TextField(rules['H'], GUILayout.Width(200));
        GUILayout.EndHorizontal();
        // Draw
        if (GUILayout.Button("DRAW", GUILayout.Width(230), GUILayout.Height(60)))
        {
            Draw();
        }
        if (Input.GetKey(KeyCode.Return))
        {
            Debug.Log("Enter");
            Draw();
        }
        GUILayout.Space(10);

        GUILayout.BeginHorizontal();
        GUILayout.TextArea("Presets : ", GUILayout.Width(60f), GUILayout.Height(30f));
        if (GUILayout.Button("1", GUILayout.Width(38.5f), GUILayout.Height(30f)))
        {
            Preset(preset1);
        }
        if (GUILayout.Button("2", GUILayout.Width(38.5f), GUILayout.Height(30f)))
        {
            Preset(preset2);
        }
        if (GUILayout.Button("3", GUILayout.Width(38.5f), GUILayout.Height(30f)))
        {
            Preset(preset3);
        }
        if (GUILayout.Button("4", GUILayout.Width(38.5f), GUILayout.Height(30f)))
        {
            Preset(preset4);
        }
        GUILayout.EndHorizontal();

        GUILayout.TextField(lSystemInformations, GUILayout.Width(230));

        GUILayout.Space(10);
        GUILayout.TextField("Use arrows, the wheel,and +- buttons\n to naviguate when the Orbital checkbox\nis toggled of.", GUILayout.Width(230));
        CameraController.Instance.Orbit = GUILayout.Toggle(CameraController.Instance.Orbit, "Orbit");
        if (GUILayout.Button("Reset Camera", GUILayout.Width(230), GUILayout.Height(25)))
        {
            CameraController.Instance.Reset();
        }

        GUILayout.BeginArea(new Rect(500f, 300f, 500f, 200f));
        if (lSystem.MeshVerticesCount > 65535)
        {
            var style = new GUIStyle();
            style.fontSize = 22;
            GUILayout.TextField(string.Format("Vertices limit (65535) exceeded !\nActual number of Vertices : {0}\nThe iteration number must be reduced", lSystem.MeshVerticesCount), style);
        }
        GUILayout.EndArea();
    }

    void Draw()
    {
        lSystem.parameters.Rules = rules;
        lSystem.parameters.constants.Clear();
        foreach (var i in constant)
        {
            lSystem.parameters.constants.Add(i);
        }
        lSystem.Reinitialise();
        lSystem.Generate();
        lSystemInformations = lSystem.parameters.ToString();
    }

    void Preset(LSystemParameters param)
    {
        lSystem.parameters = Instantiate(param);
        constant = lSystem.parameters.constants.Count > 0 ? lSystem.parameters.constants[0].ToString() : "";
        rules.Clear();
        rules.Add('X', lSystem.parameters.Rules.ContainsKey('X') ? lSystem.parameters.Rules['X'] : "");
        rules.Add('F', lSystem.parameters.Rules.ContainsKey('F') ? lSystem.parameters.Rules['F'] : "");
        rules.Add('G', lSystem.parameters.Rules.ContainsKey('G') ? lSystem.parameters.Rules['G'] : "");
        rules.Add('H', lSystem.parameters.Rules.ContainsKey('H') ? lSystem.parameters.Rules['H'] : "");
        Draw();
    }
}
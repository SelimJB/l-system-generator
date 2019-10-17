using UnityEngine;
using System;
using System.Collections.Generic;
using System.Text;

[CreateAssetMenu(fileName = "L-System parameters")]
public class LSystemParameters : ScriptableObject
{
    public new string name;
    public float angle;
    public int iteration;

    [Space(10)]
    public string axiom;
    [SerializeField]
    private Rule[] _rules;
    public List<char> constants;

    [Space(10)]
    public float stemLength = 5f;
    public float stemThickness = 0.5f;
    public Vector3 position = Vector3.zero;

    public Dictionary<char, string> Rules
    {
        get
        {
            var dic = new Dictionary<char, string>();
            foreach (var r in _rules)
            {
                dic.Add(r.symbol, r.axiom);
            }
            return dic;
        }
    }
    [Serializable]
    private class Rule
    {
        public char symbol;
        public string axiom;
    }

    public override string ToString()
    {
        var lSystemInfos = new StringBuilder();
        lSystemInfos.AppendFormat("Iteration : {0}", iteration);
        lSystemInfos.AppendFormat("\nAxiom : {0}", axiom);
        lSystemInfos.Append("\nRules : ");
        if (constants.Count > 0)
        {
            lSystemInfos.Append("\nConstants : ");
            foreach (var constant in constants)
            {
                lSystemInfos.AppendFormat("{0}  ", constant);
            }
        }
        foreach (var rule in Rules)
            lSystemInfos.AppendFormat("\n    {0}  ->  {1}", rule.Key, rule.Value);
        return lSystemInfos.ToString();
    }
}
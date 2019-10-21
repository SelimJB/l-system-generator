using UnityEngine;
using System;
using System.Collections.Generic;
using System.Text;

[CreateAssetMenu(fileName = "L-System parameters")]
public class LSystemParameters : ScriptableObject
{
	public new string name;
	[Header("L-System rules")]
	public int iteration;
	public string axiom;
	[SerializeField]
	private Rule[] _rules;
	public List<char> constants;

	[Header("Drawing parameters")]
	public float angle;
	public float stemLength = 5f;
	public float stemThickness = 0.5f;
	public Vector3 position = Vector3.zero;
	private Dictionary<char, string> _rulesDic = null;
	public Dictionary<char, string> Rules
	{
		get
		{
			if (_rulesDic == null)
			{
				var dic = new Dictionary<char, string>();
				foreach (var r in _rules)
				{
					dic.Add(r.symbol, r.axiom);
				}
				_rulesDic = dic;
			}
			return _rulesDic;
		}
		set
		{
			_rulesDic = value;
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
		if (constants.Count > 0)
		{
			lSystemInfos.Append("\nConstants : ");
			foreach (var constant in constants)
			{
				lSystemInfos.AppendFormat("{0}  ", constant);
			}
		}
		lSystemInfos.Append("\nRules : ");
		foreach (var rule in Rules)
			lSystemInfos.AppendFormat("\n    {0}  ->  {1}", rule.Key, rule.Value);
		return lSystemInfos.ToString();
	}
}
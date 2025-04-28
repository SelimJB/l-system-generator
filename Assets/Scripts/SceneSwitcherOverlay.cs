using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcherOverlay : MonoBehaviour
{
	[SerializeField] private string[] sceneNames;

	private void Awake()
	{
		var existingInstances = FindObjectsOfType<SceneSwitcherOverlay>();
		if (existingInstances.Length > 1)
		{
			Destroy(gameObject);
			return;
		}

		DontDestroyOnLoad(gameObject);
	}

#if UNITY_EDITOR
	[ContextMenu("Auto-Fill Scene Names")]
	private void AutoFillSceneNames()
	{
		var sceneCount = EditorBuildSettings.scenes.Length;
		sceneNames = new string[sceneCount];

		for (var i = 0; i < sceneCount; i++)
		{
			var path = EditorBuildSettings.scenes[i].path;
			sceneNames[i] = System.IO.Path.GetFileNameWithoutExtension(path);
		}
	}
#endif

	private void OnGUI()
	{
		const int width = 200;
		GUILayout.BeginArea(new Rect(Screen.width - width - 10, 10, width, 200));
		GUILayout.Label("Scenes:");


		foreach (var sceneName in sceneNames)
		{
			if (GUILayout.Button(sceneName))
			{
				SceneManager.LoadScene(sceneName);
			}
		}

		GUILayout.EndArea();
	}
}
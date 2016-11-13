#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using UnityEditor;

[InitializeOnLoad]
public class MNPInit  {
	
	static MNPInit () {
				
		if(!MNP_PlatformSettingsEditor.IsInstalled) {
			EditorApplication.update += OnEditorLoaded;
		} else {
			if(!MNP_PlatformSettingsEditor.IsUpToDate) {
				EditorApplication.update += OnEditorLoaded;
			}
		}
		
	}
	
	private static void OnEditorLoaded() {
		
		EditorApplication.update -= OnEditorLoaded;
		Debug.LogWarning("Mobile Native Pop Up Plugin Install Required. Opening Plugin settings...");
		Selection.activeObject = MNP_PlatformSettings.Instance;
	}
}
#endif

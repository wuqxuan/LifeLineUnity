#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections;

public class MNP_PlatformMenu : EditorWindow {

	#if UNITY_EDITOR
	
	//--------------------------------------
	//  GENERAL
	//--------------------------------------
	
	[MenuItem("Window/Stan's Assets/Mobile Native Popups/Edit Settings", false, 107)]
	public static void Edit() {
		Selection.activeObject = MNP_PlatformSettings.Instance;
	}

	[MenuItem("Window/Stan's Assets/Mobile Native Popups/Documentation", false, 107)]
	public static void Documentation() {
		Application.OpenURL("https://goo.gl/zdCgFx");
	}


	#endif
}
#endif

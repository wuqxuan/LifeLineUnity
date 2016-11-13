#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

[CustomEditor(typeof(MNP_PlatformSettings))]
public class MNP_PlatformSettingsEditor : Editor {
	
		static GUIContent SdkVersion   = new GUIContent("Plugin Version [?]", "This is Plugin version.  If you have problems or compliments please include this so we know exactly what version to look out for.");
		static GUIContent SupportEmail = new GUIContent("Support [?]", "If you have any technical quastion, feel free to drop an e-mail");
		
		
		
		private const string IOS_SOURCE_PATH 			= "Plugins/StansAssets/IOS/";
		private const string IOS_DESTANATION_PATH 		= "Plugins/IOS/";
		private const string ANDROID_SOURCE_PATH 		= "Plugins/StansAssets/Android/";
		private const string ANDROID_DESTANATION_PATH 	= "Plugins/Android/";
		
		
	public override void OnInspectorGUI() {

		#if UNITY_WEBPLAYER

		
		if(Application.isEditor) {
			return;
		}
		
		#endif
		
		GUI.changed = false;
	
		EditorGUILayout.Space();

		GeneralOptions ();

		EditorGUILayout.HelpBox("Android Platform-Specific Settings", MessageType.None);
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Android Dialogs Theme");
		MNP_PlatformSettings.Instance.AndroidDialogTheme = (MNAndroidDialogTheme) EditorGUILayout.EnumPopup(MNP_PlatformSettings.Instance.AndroidDialogTheme);
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.Space();

		AboutGUI();

		if(GUI.changed) {
			DirtyEditor();
		}
	}


	private void GeneralOptions() {
		
		if(!IsInstalled) {
			EditorGUILayout.HelpBox("Install Required ", MessageType.Error);
			
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.Space();
			Color c = GUI.color;
			GUI.color = Color.cyan;
			if(GUILayout.Button("Install Plugin",  GUILayout.Width(120))) {
				SA.Common.Editor.Instalation.Android_InstallPlugin();
				SA.Common.Editor.Instalation.IOS_InstallPlugin();
				UpdateVersionInfo();
			}
			GUI.color = c;
			EditorGUILayout.EndHorizontal();
		}
		
		if(IsInstalled) {
			if(!IsUpToDate) {
				EditorGUILayout.HelpBox("Update Required \nResources version: " + SA.Common.Editor.VersionsManager.MNP_StringVersionId + " Plugin version: " + MNP_PlatformSettings.VERSION_NUMBER, MessageType.Warning);
				
				
				
				
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.Space();
				Color c = GUI.color;
				GUI.color = Color.cyan;
				
				Debug.Log(SA.Common.Editor.VersionsManager.MNP_Version);
				if(CurrentVersion != SA.Common.Editor.VersionsManager.MNP_Version) {
					if(GUILayout.Button("Upgrade Resources",  GUILayout.Width(250))) {
						SA.Common.Editor.Instalation.Android_InstallPlugin();
						SA.Common.Editor.Instalation.IOS_InstallPlugin();
						UpdateVersionInfo();
					}
				} 
				
				GUI.color = c;
				EditorGUILayout.Space();
				EditorGUILayout.EndHorizontal();
				
				EditorGUILayout.Space();

			} else {
				EditorGUILayout.HelpBox("Mobile Native Pop Up v" + MNP_PlatformSettings.VERSION_NUMBER + " is installed", MessageType.Info);

			}
		}
		
		EditorGUILayout.Space();
		
	}
	/*****************/
	
	public static void UpdateVersionInfo() {
		SA.Common.Util.Files.Write(SA.Common.Config.MNP_VERSION_INFO_PATH, MNP_PlatformSettings.VERSION_NUMBER);
	
	}
		
		
		public static bool IsInstalled {
			get {
				return SA.Common.Editor.VersionsManager.Is_MNP_Installed;
			}
		}
		
	public static bool IsUpToDate {
		get {			
			if(CurrentVersion == SA.Common.Editor.VersionsManager.MNP_Version) {
				return true;
			} else {
				return false;
			}
		}
	}
	
	public static int CurrentVersion {
		get {
			return SA.Common.Editor.VersionsManager.ParceVersion(MNP_PlatformSettings.VERSION_NUMBER);
		}
	}
	
	public static int CurrentMagorVersion {
		get {
			return SA.Common.Editor.VersionsManager.ParceMagorVersion(MNP_PlatformSettings.VERSION_NUMBER);
		}
	}

	public static void DirtyEditor() {
		#if UNITY_EDITOR
		EditorUtility.SetDirty(MNP_PlatformSettings.Instance);
		#endif
	}
	
	private void AboutGUI() {
		
	
		EditorGUILayout.HelpBox("Version Info", MessageType.None);
		EditorGUILayout.Space();
		SelectableLabelField(SdkVersion, MNP_PlatformSettings.VERSION_NUMBER);
		EditorGUILayout.Space();
		SelectableLabelField(SupportEmail, "stans.assets@gmail.com");
			
	}
		
	private static void SelectableLabelField(GUIContent label, string value) {
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField(label, GUILayout.Width(180), GUILayout.Height(16));
		EditorGUILayout.SelectableLabel(value, GUILayout.Height(16));
		EditorGUILayout.EndHorizontal();
	}
		
	}
#endif

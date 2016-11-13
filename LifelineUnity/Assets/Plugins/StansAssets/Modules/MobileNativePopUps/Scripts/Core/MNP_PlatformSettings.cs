using UnityEngine;
using System.IO;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
[InitializeOnLoad]
#endif

public class MNP_PlatformSettings : ScriptableObject {

	private const string ISNSettingsAssetName = "MNPSettings";
	private const string ISNSettingsPath = SA.Common.Config.SETTINGS_PATH;
	private const string ISNSettingsAssetExtension = ".asset";

    public MNAndroidDialogTheme AndroidDialogTheme = MNAndroidDialogTheme.ThemeDeviceDefaultDark;

	public const string VERSION_NUMBER = "4.6";

	private static MNP_PlatformSettings instance = null;


	public static MNP_PlatformSettings Instance {
		
		get {
			if (instance == null) {
				instance = Resources.Load(ISNSettingsAssetName) as MNP_PlatformSettings;
				
				if (instance == null) {
					
					// If not found, autocreate the asset object.
					instance = CreateInstance<MNP_PlatformSettings>();
					#if UNITY_EDITOR					
					SA.Common.Util.Files.CreateFolder(ISNSettingsPath);					
					string fullPath = Path.Combine(Path.Combine("Assets", ISNSettingsPath),
					                               ISNSettingsAssetName + ISNSettingsAssetExtension);
					
					AssetDatabase.CreateAsset(instance, fullPath);
					
					

					
					#endif
				}
			}
			return instance;
		}
	}
}

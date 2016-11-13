using UnityEngine;
using System.Collections.Generic;

public abstract class MNProxyPool {

	#if UNITY_ANDROID
	private static Dictionary<string, AndroidJavaObject> pool =  new Dictionary<string, AndroidJavaObject>();
	#endif
	
	
	public static void CallStatic(string className, string methodName, params object[] args) {
		#if UNITY_ANDROID
		
		
		
		if(Application.platform != RuntimePlatform.Android) {
			return;
		}
		Debug.Log("AN: Using proxy for class: " + className + " method:" + methodName);
		
		try {
			
			AndroidJavaObject bridge;
			if(pool.ContainsKey(className)) {
				bridge = pool[className];
			} else {
				bridge = new AndroidJavaObject(className);
				pool.Add(className, bridge);
			}
			
			AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer"); 
			AndroidJavaObject act = jc.GetStatic<AndroidJavaObject>("currentActivity"); 
			
			act.Call("runOnUiThread", new AndroidJavaRunnable(() => { bridge.CallStatic(methodName, args); }));
			
			
		} catch(System.Exception ex) {
			Debug.LogWarning(ex.Message);
		}
		#endif
	}

}

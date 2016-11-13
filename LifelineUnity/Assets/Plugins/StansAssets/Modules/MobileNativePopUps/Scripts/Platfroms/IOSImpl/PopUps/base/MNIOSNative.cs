#define SA_DEBUG_MODE

using UnityEngine;
using System.Collections;
#if (UNITY_IPHONE && !UNITY_EDITOR) || SA_DEBUG_MODE
using System.Runtime.InteropServices;
#endif

public class MNIOSNative {
	
	//--------------------------------------
	//  NATIVE FUNCTIONS
	//--------------------------------------
	
	#if (UNITY_IPHONE && !UNITY_EDITOR) || SA_DEBUG_MODE
	[DllImport ("__Internal")]
	private static extern void _MNP_ShowRateUsPopUp(string title, string message, string rate, string remind, string declined);
	
	[DllImport ("__Internal")]
	private static extern void _MNP_ShowDialog(string title, string message, string yes, string no);
	
	[DllImport ("__Internal")]
	private static extern void _MNP_ShowMessage(string title, string message, string ok);
	
	[DllImport ("__Internal")]
	private static extern void _MNP_DismissCurrentAlert();


	[DllImport ("__Internal")]
	private static extern void _MNP_RedirectToAppStoreRatingPage(string appId);
	
	[DllImport ("__Internal")]
	private static extern void _MNP_ShowPreloader();
	
	
	[DllImport ("__Internal")]
	private static extern void _MNP_HidePreloader();
	#endif
	

	
	public static void dismissCurrentAlert() {
		#if (UNITY_IPHONE && !UNITY_EDITOR) || SA_DEBUG_MODE
		_MNP_DismissCurrentAlert();
		#endif
	}

	public static void showRateUsPopUP(string title, string message, string rate, string remind, string declined) {
		#if (UNITY_IPHONE && !UNITY_EDITOR) || SA_DEBUG_MODE
		_MNP_ShowRateUsPopUp(title, message, rate, remind, declined);
		#endif
	}
	
	
	public static void showDialog(string title, string message) {
		showDialog(title, message, "Yes", "No");
	}
	
	public static void showDialog(string title, string message, string yes, string no) {
		#if (UNITY_IPHONE && !UNITY_EDITOR) || SA_DEBUG_MODE
		_MNP_ShowDialog(title, message, yes, no);
		#endif
	}
	
	
	public static void showMessage(string title, string message) {
		showMessage(title, message, "Ok");
	}
	
	public static void showMessage(string title, string message, string ok) {
		#if (UNITY_IPHONE && !UNITY_EDITOR) || SA_DEBUG_MODE
		_MNP_ShowMessage(title, message, ok);
		#endif
	}

	public static void RedirectToAppStoreRatingPage(string appleId) {
		#if (UNITY_IPHONE && !UNITY_EDITOR) || SA_DEBUG_MODE
		_MNP_RedirectToAppStoreRatingPage(appleId);
		#endif
	}
	
	
	public static void ShowPreloader() {
		#if (UNITY_IPHONE && !UNITY_EDITOR) || SA_DEBUG_MODE
		_MNP_ShowPreloader();
		#endif
	}
	
	public static void HidePreloader() {
		#if (UNITY_IPHONE && !UNITY_EDITOR) || SA_DEBUG_MODE
		_MNP_HidePreloader();
		#endif
	}
}

////////////////////////////////////////////////////////////////////////////////
//  
// @module Common Android Native Lib
// @author Osipov Stanislav (Stan's Assets) 
// @support stans.assets@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;

public class MNAndroidNative {

	private const string CLASS_NAME = "com.mnp.popups.NativePopupsManager";
	
	private static void CallActivityFunction(string methodName, params object[] args) {
		MNProxyPool.CallStatic(CLASS_NAME, methodName, args);
	}
	
	//--------------------------------------
	//  MESSAGING
	//--------------------------------------


	public static void showDialog(string title, string message, MNAndroidDialogTheme theme) {
		showDialog (title, message, "Yes", "No", theme);
	}

	public static void showDialog(string title, string message, string yes, string no, MNAndroidDialogTheme theme) {
		CallActivityFunction("ShowDialog", title, message, yes, no, (int)theme);
	}

	public static void dismissDialog() {
		CallActivityFunction("DismissDialog");
	}

	public static void showMessage(string title, string message, MNAndroidDialogTheme theme) {
		showMessage (title, message, "Ok", theme);
	}

	public static void showMessage(string title, string message, string ok, MNAndroidDialogTheme theme) {
		CallActivityFunction("ShowMessage", title, message, ok, (int)theme);
	}

	public static void showRateDialog(string title, string message, string yes, string laiter, string no, MNAndroidDialogTheme theme) {
		CallActivityFunction("ShowRateDialog", title, message, yes, laiter, no, (int)theme);
	}
	
	public static void ShowPreloader(string title, string message, MNAndroidDialogTheme theme) {
		CallActivityFunction("ShowPreloader",  title, message, (int)theme);
	}
	
	public static void HidePreloader() {
		CallActivityFunction("HidePreloader");
	}

	public static void RedirectStoreRatingPage(string url) {
		CallActivityFunction("OpenAppRatingPage", url);
	}

}

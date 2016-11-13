////////////////////////////////////////////////////////////////////////////////
//  
// @module Android Native Plugin for Unity3D 
// @author Osipov Stanislav (Stan's Assets) 
// @support stans.assets@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MNAndroidMessage : MNPopup {
	
	
	public string ok;
	
	//--------------------------------------
	// INITIALIZE
	//--------------------------------------
	
	public static MNAndroidMessage Create(string title, string message) {
		return Create(title, message, "Ok");
	}
		
	public static MNAndroidMessage Create(string title, string message, string ok) {
		MNAndroidMessage dialog;
		dialog  = new GameObject("AndroidPopUp").AddComponent<MNAndroidMessage>();
		dialog.title = title;
		dialog.message = message;
		dialog.ok = ok;
		
		dialog.init();
		
		return dialog;
	}
	
	
	//--------------------------------------
	//  PUBLIC METHODS
	//--------------------------------------
	
	public void init() {
		MNAndroidNative.showMessage(title, message, ok, MNP_PlatformSettings.Instance.AndroidDialogTheme);
	}
	
	//--------------------------------------
	//  GET/SET
	//--------------------------------------
	
	//--------------------------------------
	//  EVENTS
	//--------------------------------------
	
	public void onPopUpCallBack(string buttonIndex) {
		OnComplete(MNDialogResult.YES);
		Destroy(gameObject);	
	}
	
	//--------------------------------------
	//  PRIVATE METHODS
	//--------------------------------------
	
	//--------------------------------------
	//  DESTROY
	//--------------------------------------


}

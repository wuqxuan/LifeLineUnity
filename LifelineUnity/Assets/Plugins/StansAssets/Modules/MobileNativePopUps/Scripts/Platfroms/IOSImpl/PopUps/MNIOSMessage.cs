////////////////////////////////////////////////////////////////////////////////
//  
// @module IOS Native Plugin for Unity3D 
// @author Osipov Stanislav (Stan's Assets) 
// @support stans.assets@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MNIOSMessage : MNPopup {
	
	
	public string ok;
	
	//--------------------------------------
	// INITIALIZE
	//--------------------------------------
	
	public static MNIOSMessage Create(string title, string message) {
		return Create(title, message, "Ok");
	}
		
	public static MNIOSMessage Create(string title, string message, string ok) {
		MNIOSMessage dialog;
		dialog  = new GameObject("IOSPopUp").AddComponent<MNIOSMessage>();
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
		MNIOSNative.showMessage(title, message, ok);
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

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

public class MNIOSDialog : MNPopup {
	

	public string yes;
	public string no;
	
	//--------------------------------------
	// INITIALIZE
	//--------------------------------------
	
	public static MNIOSDialog Create(string title, string message) {
		return Create(title, message, "Yes", "No");
	}
		
	public static MNIOSDialog Create(string title, string message, string yes, string no) {
		MNIOSDialog dialog;
		dialog  = new GameObject("IOSPopUp").AddComponent<MNIOSDialog>();
		dialog.title = title;
		dialog.message = message;
		dialog.yes = yes;
		dialog.no = no;
		dialog.init();
		
		return dialog;
	}
	
	
	//--------------------------------------
	//  PUBLIC METHODS
	//--------------------------------------
	
	public void init() {
		MNIOSNative.showDialog(title, message, yes, no);
	}
	
	//--------------------------------------
	//  GET/SET
	//--------------------------------------
	
	//--------------------------------------
	//  EVENTS
	//--------------------------------------
	
	public void onPopUpCallBack(string buttonIndex) {
		int index = System.Convert.ToInt16(buttonIndex);
		
		switch(index) {
			case 0: 
				OnComplete(MNDialogResult.YES);
				break;
			case 1: 
				OnComplete(MNDialogResult.NO);
				break;
		}
		
		Destroy(gameObject);
	}
	
	//--------------------------------------
	//  PRIVATE METHODS
	//--------------------------------------
	
	//--------------------------------------
	//  DESTROY
	//--------------------------------------


}

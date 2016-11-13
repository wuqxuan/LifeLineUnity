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

public class MNIOSRateUsPopUp : MNPopup {
	
	public string rate;
	public string remind;
	public string declined;
	public string appleId;
	
	//--------------------------------------
	// INITIALIZE
	//--------------------------------------

	public static MNIOSRateUsPopUp Create() {
		return Create("Like the Game?", "Rate US");
	}
	
	public static MNIOSRateUsPopUp Create(string title, string message) {
		return Create(title, message, "Rate Now", "Ask me later", "No, thanks");
	}
	
	public static MNIOSRateUsPopUp Create(string title, string message, string rate, string remind, string declined) {
		MNIOSRateUsPopUp popup = new GameObject("IOSRateUsPopUp").AddComponent<MNIOSRateUsPopUp>();
		popup.title = title;
		popup.message = message;
		popup.rate = rate;
		popup.remind = remind;
		popup.declined = declined;
		
		popup.init();
		
	
		return popup;
	}
	
	
	//--------------------------------------
	//  PUBLIC METHODS
	//--------------------------------------
	
	
	public void init() {
		MNIOSNative.showRateUsPopUP(title, message, rate, remind, declined);
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
				MNIOSNative.RedirectToAppStoreRatingPage(appleId);
				OnComplete(MNDialogResult.RATED);
				break;
			case 1:
				OnComplete( MNDialogResult.REMIND);
				break;
			case 2:
				OnComplete( MNDialogResult.DECLINED);
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

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

public class MNAndroidRateUsPopUp : MNPopup {
	


	public string yes;
	public string later;
	public string no;
	public string url;

	
	//--------------------------------------
	// INITIALIZE
	//--------------------------------------

	public static MNAndroidRateUsPopUp Create(string title, string message, string url) {
		return Create(title, message, url, "Rate app", "Later", "No, thanks");
	}
	
	public static MNAndroidRateUsPopUp Create(string title, string message, string url, string yes, string later, string no) {
		MNAndroidRateUsPopUp rate = new GameObject("AndroidRateUsPopUp").AddComponent<MNAndroidRateUsPopUp>();
		rate.title = title;
		rate.message = message;
		rate.url = url;

		rate.yes = yes;
		rate.later = later;
		rate.no = no;

		rate.init();
			
		return rate;
	}
	
	
	//--------------------------------------
	//  PUBLIC METHODS
	//--------------------------------------
	
	
	public void init() {
		MNAndroidNative.showRateDialog(title, message, yes, later, no, MNP_PlatformSettings.Instance.AndroidDialogTheme);
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
				MNAndroidNative.RedirectStoreRatingPage(url);
				OnComplete(MNDialogResult.RATED);
				break;
			case 1:
				OnComplete(MNDialogResult.REMIND);
				break;
			case 2:
				OnComplete(MNDialogResult.DECLINED);
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

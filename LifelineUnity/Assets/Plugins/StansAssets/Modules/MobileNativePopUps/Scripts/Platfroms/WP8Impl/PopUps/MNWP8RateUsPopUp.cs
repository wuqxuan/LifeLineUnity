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

public class MNWP8RateUsPopUp : MNPopup {
	
	//--------------------------------------
	// INITIALIZE
	//--------------------------------------

	public static MNWP8RateUsPopUp Create(string title, string message) {
		MNWP8RateUsPopUp popup = new GameObject("WP8RateUsPopUp").AddComponent<MNWP8RateUsPopUp>();
		popup.title = title;
		popup.message = message;
		
		popup.init();
		
		return popup;
	}
	
	
	//--------------------------------------
	//  PUBLIC METHODS
	//--------------------------------------
	

	public void init() {

		#if UNITY_WP8
		WP8PopUps.PopUp.ShowMessageWindow_OK_Cancel(message, title, OnOkDel, OnCancelDel);
		#endif
	}
	
	//--------------------------------------
	//  GET/SET
	//--------------------------------------
	
	//--------------------------------------
	//  EVENTS
	//--------------------------------------
	
	
	public void OnOkDel() {
		#if UNITY_WP8
		WP8PopUps.PopUp.ShowRateWindow();
		#endif

		OnComplete(MNDialogResult.RATED);
		Destroy(gameObject);
	}
	
	public void OnCancelDel() {
		OnComplete(MNDialogResult.DECLINED);
		Destroy(gameObject);
	}
	
	//--------------------------------------
	//  PRIVATE METHODS
	//--------------------------------------
	
	//--------------------------------------
	//  DESTROY
	//--------------------------------------


}

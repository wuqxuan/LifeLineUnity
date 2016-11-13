////////////////////////////////////////////////////////////////////////////////
//  
// @module <module_name>
// @author Osipov Stanislav lacost.st@gmail.com
//
////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;

public class MNUseExample : MNFeaturePreview {

	public string appleId = "";
	public string androidAppUrl = "market://details?id=com.google.earth";

	void Awake() {

	}
	
	void OnGUI() {
		
		UpdateToStartPos();
		
		GUI.Label(new Rect(StartX, StartY, Screen.width, 40), "Native Pop Ups", style);
		StartY+= YLableStep;


		if(GUI.Button(new Rect(StartX, StartY, buttonWidth, buttonHeight), "Rate PopUp with events")) {
			MobileNativeRateUs ratePopUp =  new MobileNativeRateUs("Like this game?", "Please rate to support future updates!");
			ratePopUp.SetAppleId(appleId);
			ratePopUp.SetAndroidAppUrl(androidAppUrl);
			ratePopUp.OnComplete += OnRatePopUpClose;

			ratePopUp.Start();


		}
		
		
		StartX += XButtonStep;
		if(GUI.Button(new Rect(StartX, StartY, buttonWidth, buttonHeight), "Dialog PopUp")) {
			MobileNativeDialog dialog = new MobileNativeDialog("Dialog Titile", "Dialog message");
			dialog.OnComplete += OnDialogClose;

			Invoke("Dismiss", 2.0f);
		}
		
		
		StartX += XButtonStep;
		if(GUI.Button(new Rect(StartX, StartY, buttonWidth, buttonHeight), "Message PopUp")) {
			MobileNativeMessage msg = new MobileNativeMessage("Message Titile", "Message message");
			msg.OnComplete += OnMessageClose;
		}

		StartY += YButtonStep;
		StartX = XStartPos;
		if(GUI.Button(new Rect(StartX, StartY, buttonWidth, buttonHeight), "Show Prealoder")) {
			MNP.ShowPreloader("Title", "Message");
			Invoke("OnPreloaderTimeOut", 3f);
		}

		StartX += XButtonStep;
		if(GUI.Button(new Rect(StartX, StartY, buttonWidth, buttonHeight), "Hide Prealoder")) {
			MNP.HidePreloader();
		}
		
	}

	private void Dismiss() {
		Debug.Log("DIALOG DISMISS");
		MNAndroidNative.dismissDialog();
	}
	
	//--------------------------------------
	//  GET/SET
	//--------------------------------------
	
	//--------------------------------------
	//  EVENTS
	//--------------------------------------


	private void OnPreloaderTimeOut() {
		MNP.HidePreloader();
	}
	

	
	private void OnRatePopUpClose(MNDialogResult result) {

		//parsing result
		switch(result) {
		case MNDialogResult.RATED:
			Debug.Log ("Rate Option pickied");
			break;
		case MNDialogResult.REMIND:
			Debug.Log ("Remind Option pickied");
			break;
		case MNDialogResult.DECLINED:
			Debug.Log ("Declined Option pickied");
			break;
		}

		new MobileNativeMessage("Result", result.ToString() + " button pressed");

	}
	
	private void OnDialogClose(MNDialogResult result) {
		

		//parsing result
		switch(result) {
		case MNDialogResult.YES:
			Debug.Log ("Yes button pressed");
			break;
		case MNDialogResult.NO:
			Debug.Log ("No button pressed");
			break;
			
		}

		new MobileNativeMessage("Result", result.ToString() + " button pressed");
	}
	
	private void OnMessageClose() {

		new MobileNativeMessage("Result", "Message Closed");
	}




}

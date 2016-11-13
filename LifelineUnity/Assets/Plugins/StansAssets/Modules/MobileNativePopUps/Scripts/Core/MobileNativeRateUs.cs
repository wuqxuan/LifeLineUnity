using UnityEngine;
using System;
using System.Collections;

public class MobileNativeRateUs  {

	public string title;
	public string message;
	public string yes;
	public string later;
	public string no;


	public string url;
	public string appleId;

	public Action<MNDialogResult> OnComplete = delegate {};




	public MobileNativeRateUs(string title, string message) {

		this.title = title;
		this.message = message;
		this.yes = "Rate app";
		this.later = "Later";
		this.no = "No, thanks";

	}
	
	public MobileNativeRateUs(string title, string message, string yes, string later, string no) {
		this.title = title;
		this.message = message;
		this.yes = yes;
		this.later = later;
		this.no = no;
	}


	public void SetAndroidAppUrl(string _url) {
		url = _url;
	}

	public void SetAppleId(string _appleId) {
		appleId = _appleId;
	}
	
	public void Start() {
		
		#if UNITY_WP8
		MNWP8RateUsPopUp rate = MNWP8RateUsPopUp.Create(title, message);
		rate.OnComplete += OnCompleteListener;
		#endif
		
		
		#if UNITY_IPHONE
		MNIOSRateUsPopUp rate = MNIOSRateUsPopUp.Create(title, message, yes, later, no);
		rate.appleId = appleId;
		rate.OnComplete += OnCompleteListener;
		#endif
		
		#if UNITY_ANDROID
		MNAndroidRateUsPopUp rate = MNAndroidRateUsPopUp.Create(title, message, url, yes, later, no);
		rate.OnComplete += OnCompleteListener;
		#endif

	}
	
	
	
	private void OnCompleteListener(MNDialogResult res) {
		OnComplete(res);
	}
}


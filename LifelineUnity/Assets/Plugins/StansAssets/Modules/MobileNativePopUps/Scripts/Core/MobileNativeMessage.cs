using UnityEngine;
using System;
using System.Collections;

public class MobileNativeMessage  {

	public Action OnComplete = delegate {};
	

	public MobileNativeMessage(string title, string message) {
		init(title, message, "Ok");
	}
	
	public MobileNativeMessage(string title, string message, string ok) {
		init(title, message, ok);
	}
	
	
	private void init(string title, string message, string ok) {
		
		#if UNITY_WP8
		MNWP8Message msg  = MNWP8Message.Create(title, message);
		msg.OnComplete += OnCompleteListener;
		#endif
		
		
		#if UNITY_IPHONE
		MNIOSMessage msg  = MNIOSMessage.Create(title, message, ok);
		msg.OnComplete += OnCompleteListener;
		#endif
		
		#if UNITY_ANDROID
		MNAndroidMessage msg  = MNAndroidMessage.Create(title, message, ok);
		msg.OnComplete += OnCompleteListener;
		#endif
		

	}
	
	
	
	private void OnCompleteListener(MNDialogResult res) {
		OnComplete();
	}
}


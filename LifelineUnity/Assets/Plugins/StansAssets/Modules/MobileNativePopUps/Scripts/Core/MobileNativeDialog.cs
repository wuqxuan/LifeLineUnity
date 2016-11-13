using UnityEngine;
using System;
using System.Collections;

public class MobileNativeDialog  {

	public Action<MNDialogResult> OnComplete = delegate {};

	public MobileNativeDialog(string title, string message) {
		init(title, message, "Yes", "No");
	}

	public MobileNativeDialog(string title, string message, string yes, string no) {
		init(title, message, yes, no);
	}


	private void init(string title, string message, string yes, string no) {

		#if UNITY_WP8
		MNWP8Dialog dialog  = MNWP8Dialog.Create(title, message);
		dialog.OnComplete += OnCompleteListener;
		#endif


		#if UNITY_IPHONE
		MNIOSDialog dialog  = MNIOSDialog.Create(title, message, yes, no);
		dialog.OnComplete += OnCompleteListener;
		#endif

		#if UNITY_ANDROID
		MNAndroidDialog dialog  = MNAndroidDialog.Create(title, message, yes, no);
		dialog.OnComplete += OnCompleteListener;
		#endif


	}



	private void OnCompleteListener(MNDialogResult res) {
		OnComplete(res);
	}
}


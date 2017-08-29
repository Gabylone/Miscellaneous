﻿using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuManager : MonoBehaviour {

	[SerializeField]
	private SaveMenu saveMenu;

	[SerializeField]
	private GameObject quitFeedback;

	[SerializeField]
	private GameObject openButton;
	[SerializeField]
	private GameObject menuGroup;

	bool quit_Confirmed = false;


	public void Open () {

		menuGroup.SetActive (true);
		Tween.Bounce (menuGroup.transform , 0.2f , 1.1f);
	}

	public void Close () {
		
		menuGroup.SetActive (false);

		saveMenu.Opened = false;

		quitFeedback.SetActive (false);

		quit_Confirmed = false;

	}

	#region buttons
	public void SaveButton () {

		saveMenu.Saving = true;
		saveMenu.Opened = !saveMenu.Opened;
	}
	public void LoadButton () {
		saveMenu.Saving = false;
		saveMenu.Opened = !saveMenu.Opened;
	}
	public void QuitButton () {

		if (quit_Confirmed) {

			Application.Quit ();
		} else {
			quit_Confirmed = true;
			quitFeedback.SetActive (true);
			Tween.Bounce (quitFeedback.transform, 0.2f, 1.2f);
		}
	}
	#endregion
}
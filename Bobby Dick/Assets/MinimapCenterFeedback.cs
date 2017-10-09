﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCenterFeedback : MonoBehaviour {

	public GameObject group;

	public float displayDuration = 1f;

	// Use this for initialization
	void Start () {
		
//		DisplayMinimap.Instance.onCenterMap += HandleOnCenterMap;
		Quest.showQuestOnMap += HandleShowQuestOnMap;

		GetComponent<RectTransform> ().sizeDelta = DisplayMinimap.Instance.minimapChunkScale;


		Hide ();
	}

	void HandleShowQuestOnMap (Quest quest)
	{
		HandleOnCenterMap (quest.targetCoords);
	}

	void HandleOnCenterMap (Coords coords)
	{
		GetComponent<RectTransform> ().anchoredPosition = DisplayMinimap.Instance.getPosFromCoords (coords);

		Tween.Bounce ( transform );

		Show ();
		Invoke ("Hide",displayDuration);
	}

	void Show () {
		group.SetActive (true);
	}

	void Hide () {
		group.SetActive (false);
	}
}

﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FlagControl : MonoBehaviour {

	[Header ("Flag")]
	[SerializeField]
	private Image flagImage;
	[SerializeField]
	private float distanceToStop = 1.1f;

	[SerializeField]
	private float distanceToTriggerIsland = 0.8f;

	[SerializeField]
	private float boatSpeed = 1.2f;

	[SerializeField]
	private Boat playerBoat;

	[SerializeField]
	private Vector3 decalToIsland = Vector3.zero;

	[SerializeField]
	private Island island;

	public bool updatingPosition = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		if (updatingPosition && IslandManager.Instance.OnIsland == false) {
			PlaceFlagOnScreen ();
		}

		UpdateFlagToIsland ();

		NavigationManager.Instance.EnterNewChunk += ResetFlag;
	}

	public void ResetFlag () {
		Vector3 pos = Camera.main.ScreenToViewportPoint (new Vector2 (Screen.width/2 ,Screen.height/2));

		flagImage.rectTransform.anchorMin = pos;
		flagImage.rectTransform.anchorMax = pos;
	}

	private void UpdateFlagToIsland () {

			// get island pos
		Vector3 islandPos = island.transform.position;
		islandPos.z = flagImage.transform.position.z;

		// calc distances
		float distance_BoatToFlag = Vector3.Distance (flagImage.transform.position, playerBoat.GetTransform.position);
		bool flagIsNearIsland = Vector3.Distance (flagImage.transform.position, islandPos) < distanceToTriggerIsland;

		flagImage.color = flagIsNearIsland ? Color.red : Color.blue;
		flagImage.enabled = !(distance_BoatToFlag < distanceToStop + 0.3f);

		if (flagIsNearIsland) {
			Vector3 pos = Camera.main.WorldToViewportPoint (islandPos + decalToIsland);

			if ( distance_BoatToFlag < distanceToStop * 1.5f ) {
				
				IslandManager.Instance.Enter ();

				// move flag to prevent reentering on leave island
				if (islandPos.x < 0) {
					pos = Camera.main.WorldToViewportPoint (islandPos + Vector3.left * distanceToTriggerIsland * 1.5f);
				} else {
					pos = Camera.main.WorldToViewportPoint (islandPos + Vector3.right * distanceToTriggerIsland * 1.5f);
				}
				flagImage.color = Color.blue;

			}

			flagImage.rectTransform.anchorMin = pos;
			flagImage.rectTransform.anchorMax = pos;
		}


		playerBoat.TargetSpeed = (distance_BoatToFlag - distanceToStop) * boatSpeed;
		playerBoat.TargetDirection = (flagImage.transform.position - playerBoat.GetTransform.position).normalized;


		if (distance_BoatToFlag < distanceToStop) {
			playerBoat.TargetSpeed = 0f;
		}
	}

	private void PlaceFlagOnScreen () {
		Vector3 pos = Camera.main.ScreenToViewportPoint (InputManager.Instance.GetInputPosition ());

		flagImage.rectTransform.anchorMin = pos;
		flagImage.rectTransform.anchorMax = pos;
	}


	#region properties
	public Image FlagImage {
		get {
			return flagImage;
		}
	}
	#endregion


	public bool UpdatingPosition {
		get {
			return updatingPosition;
		}
		set {
			updatingPosition = value;
			PlaceFlagOnScreen ();
		}
	}
}
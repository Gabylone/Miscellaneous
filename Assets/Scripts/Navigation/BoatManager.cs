﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BoatManager : MonoBehaviour {

	public static BoatManager Instance;

	[Header("Boat")]
	[SerializeField]
	private Transform boatTransform;
	[SerializeField]
	private Vector2 boatBounds = new Vector2 ( 350f, 164f );
	[SerializeField]
	private float boatSpeed = 0.3f;
	[SerializeField]
	private Image boatLightImage;

	void Awake () {
		Instance = this;
	}
	
	void Update () {

		if ( (boatTransform.position.x < -0.5f || boatTransform.position.x > 0.5f) ||
			(boatTransform.position.y < -0.5f || boatTransform.position.y > 0.5f) ) {
			{
				if (IslandManager.Instance.OnIsland == false) {
					Vector2 getDir = NavigationManager.Instance.getDir (NavigationManager.Instance.CurrentDirection);
					boatTransform.Translate (getDir * boatSpeed * Time.deltaTime, Space.World);
				}
			}
		}
	}

	public Transform BoatTransform {
		get {
			return boatTransform;
		}
	}

	public void SetBoatPos () {
		Vector2 getDir =NavigationManager.Instance.getDir(NavigationManager.Instance.CurrentDirection);
		boatTransform.localPosition = new Vector2(-getDir.x * boatBounds.x, -getDir.y * boatBounds.y);

	}

	public Image BoatLightImage {
		get {
			return boatLightImage;
		}
	}
}
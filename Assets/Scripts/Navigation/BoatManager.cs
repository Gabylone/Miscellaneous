﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BoatManager : MonoBehaviour {

	public static BoatManager Instance;

	public bool trading = false;

	public enum UpgradeType {
		Crew,
		Cargo,
		Longview
	}

	void Awake () {
		Instance = this;
	}

	UIButton uiButton;

	[SerializeField]
	private Text boatLevelText;
	private int boatCurrentLevel = 1;


	[Header("Crew")]
	[SerializeField]
	private Button[] crewButtons;
	[SerializeField]
	private float crewIconScale = 0;

	[Header("Prices")]
	[SerializeField]
	private Text[] goldTexts;
	[SerializeField]
	private float[] upgradePrices = new float[3];
	private int [] upgradeLevels = new int[3]
	{1,1,1};

	[Header("Sounds")]
	[SerializeField] private AudioClip upgradeSound;

	public void ShowUpgradeMenu () {
		UpdateCrewImages ();
		UpdatePrices ();
		UpdateTexts ();
	}

	public void StartTrade () {
		//
	}
	public void EndTrade () {
		//
	}

	public void Upgrade ( int i ) {

		if ( !GoldManager.Instance.CheckGold ( upgradePrices[i] ))
			return;

		switch ( (UpgradeType)i ) {
		case UpgradeType.Crew:
			Crews.playerCrew.MemberCapacity += 1;
			break;
		case UpgradeType.Cargo:
			WeightManager.Instance.CurrentCapacity *= 2;
			break;
		case UpgradeType.Longview:
			NavigationManager.Instance.ShipRange++;
			break;
		}

		GoldManager.Instance.RemoveGold ( (int)upgradePrices[i] );

		SoundManager.Instance.PlaySound (upgradeSound);

		++boatCurrentLevel;
		++upgradeLevels [i];

		UpdatePrices ();
		UpdateTexts ();

	}

	public void UpdatePrices () {
		for (int i = 0; i < upgradePrices.Length; ++i ) {
			upgradePrices [i] = upgradePrices [i] * upgradeLevels [i];
		}
	}

	public void UpdateTexts () {
		for (int i = 0; i < goldTexts.Length; ++i)
			goldTexts[i].text = "" + upgradePrices[i];

		boatLevelText.text = "" + boatCurrentLevel;
	}

	public void UpdateCrewImages () {
		for (int i = 0; i < crewButtons.Length; ++i ) {

			crewButtons [i].gameObject.SetActive (i <= Crews.playerCrew.MemberCapacity);
			crewButtons [i].image.color = i == Crews.playerCrew.MemberCapacity ? Color.white : Color.black;
			crewButtons [i].interactable = i == Crews.playerCrew.MemberCapacity && trading;
		}
	}

}

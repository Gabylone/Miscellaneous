﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WeightManager : MonoBehaviour {

	public static WeightManager Instance;

	private int currentCapacity = 100;

	[SerializeField]
	private GameObject weightGroup;
	[SerializeField]
	private Image weightImage;
	[SerializeField]
	private Text currentWeightText;

	bool displayingFeedback = false;
	float timer = 0f;
	[SerializeField]
	private float feedbackDuration = 0.3f;

	[Header("Sound")]
	[SerializeField] private AudioClip noRoomSound;

	void Awake () {
		Instance = this;
	}

	public void Init () {
		PlayerLoot.Instance.openInventory += Show;
		PlayerLoot.Instance.closeInventory += Hide;

		NavigationManager.Instance.EnterNewChunk += UpdateDisplay;

		PlayerLoot.Instance.LootUI.useInventory += UpdateDisplay;

		Hide ();
	}

	void Update () {
		if ( displayingFeedback ) {
			timer += Time.deltaTime;

			if ( timer > feedbackDuration ) {
				HideFeedback ();
			}
		}
	}

	#region weight control
	public bool CheckWeight ( int amount ) {

		if ( CurrentWeight + amount > currentCapacity ) {
			SoundManager.Instance.PlaySound ( noRoomSound );
			DisplayFeedback ();
			return false;
		}

		return true;

	}
	public void UpdateDisplay (InventoryActionType inventoryActionType) {
		UpdateDisplay ();
	}
	public void UpdateDisplay () {
		currentWeightText.text = "" + CurrentWeight;
		print ("current wirhgt : " + CurrentWeight);
		weightImage.fillAmount = CurrentWeight / CurrentCapacity;
	}
	#endregion

	#region feedback
	public void DisplayFeedback () {	
		displayingFeedback = true;
		timer = 0f;

		weightImage.color = Color.red;
		currentWeightText.color = Color.red;
	}

	public void HideFeedback () {
		displayingFeedback = false;

		weightImage.color = Color.white;
		currentWeightText.color = Color.white;
	}
	#endregion

	#region properties
	public int CurrentWeight {
		get {
			return LootManager.Instance.getLoot (Crews.Side.Player).weight;
		}
	}

	public int CurrentCapacity {
		get {
			return currentCapacity;
		}
		set {
			currentCapacity = value;
			UpdateDisplay ();
		}
	}

	public void Show () {
		Visible = true;
	}
	public void Hide () {
		Visible = false;
	}

	public bool Visible {
		get {
			return weightGroup.activeSelf;
		}
		set {
			weightGroup.SetActive (value);
		}
	}
	#endregion
}

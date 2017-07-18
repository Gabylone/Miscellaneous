﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SoundManager : MonoBehaviour {

	public enum Sound {
		Select_Small,
		Select_Big,
	}

	public static SoundManager Instance;

	[SerializeField]
	private AudioSource soundSource;

	[SerializeField]
	private AudioSource ambianceSource;

	[SerializeField]
	private AudioClip[] clips;

	bool enableSound = true;

	[Header("Inventory sounds")]
	[SerializeField] private AudioClip eatSound;
	[SerializeField] private AudioClip equipSound;
	[SerializeField] private AudioClip sellSound;
	[SerializeField] private AudioClip lootSound;

	[Header ("Time sounds")]
	[SerializeField] private AudioClip rainSound;
	[SerializeField] private AudioClip daySound;
	[SerializeField] private AudioClip nightSound;

	void Start () {
		EnableSound = true;

		PlayerLoot.Instance.LootUI.useInventory += HandleUsePlayerInventory;
		OtherLoot.Instance.LootUi.useInventory += HandleUseEnemyInventory;
		NavigationManager.Instance.EnterNewChunk += HandleChunkEvent;
	}

	#region time
	void HandleChunkEvent ()
	{
		AudioClip ambiantClip;
		if (TimeManager.Instance.Raining)
			ambiantClip = rainSound;
		else if (TimeManager.Instance.IsNight)
			ambiantClip = nightSound;
		else
			ambiantClip = daySound;

		PlayAmbiance (ambiantClip);
	}
	#endregion

	#region inventory
	void HandleUsePlayerInventory (InventoryActionType actionType)
	{
		switch (actionType) {
		case InventoryActionType.Eat:
			PlaySound (eatSound);
			break;
		case InventoryActionType.Equip:
			PlaySound (equipSound);
			break;
		case InventoryActionType.Throw:
			PlaySound (equipSound);
			break;
		case InventoryActionType.Sell:
			PlaySound (sellSound);
			break;
		default:
			throw new System.ArgumentOutOfRangeException ();
		}
	}
	void HandleUseEnemyInventory (InventoryActionType actionType)
	{
		switch (actionType) {
		case InventoryActionType.PickUp:
			PlaySound (equipSound);
			break;
		case InventoryActionType.Buy:
			PlaySound (sellSound);
			break;
		}
	}
	#endregion

	void Awake () {
		Instance = this;
	}

	public void PlayRandomSound (AudioClip[] clips) {
		PlaySound (clips [Random.Range (0, clips.Length)]);
	}

	public void PlaySound ( Sound sound ) {
		PlaySound (clips [(int)sound]);
	}

	public void PlaySound ( AudioClip clip ) {

		if ( clip == null ) {
			Debug.LogError ("unassigned clip");
			return;
		}

		soundSource.clip = clip;
		soundSource.Play ();
	}

	public void PlayAmbiance ( AudioClip clip ) {
		ambianceSource.clip = clip;
		ambianceSource.Play ();
	}

	public AudioSource AmbianceSource {
		get {
			return ambianceSource;
		}
	}

	[SerializeField]
	private Image soundImage;
	[SerializeField]
	private Sprite sprite_SoundOn;

	[SerializeField]
	private Sprite sprite_SoundOff;

	public void SwitchEnableSound () {
		EnableSound = !EnableSound;
	}

	public bool EnableSound {
		get {
			return enableSound;
		}
		set {
			enableSound = value;

			soundSource.enabled = value;
			ambianceSource.enabled = value;
			if (value) {
				ambianceSource.Play ();
			}

			soundImage.sprite = value ? sprite_SoundOn : sprite_SoundOff;
		}
	}
}

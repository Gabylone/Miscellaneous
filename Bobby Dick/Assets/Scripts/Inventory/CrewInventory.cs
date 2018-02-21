﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CrewInventory : MonoBehaviour {

	public static CrewInventory Instance;

	public delegate void OpenInventory (CrewMember member);
	public OpenInventory openInventory;
	public delegate void CloseInventory ();
	public CloseInventory closeInventory;

	private CrewMember selectedMember;

	[Header("Groups")]
	[SerializeField]
	private GameObject crewGroup;
	public bool canOpen = true;

	public GameObject characterStatGroup;
	public GameObject menuButtonsObj;

	public void Lock () {
		canOpen = false;
	}
	public void Unlock () {
		canOpen = true;
	}

	[SerializeField]
	private Transform crewCanvas;

	private bool ateSomething = false;

	[SerializeField]
	private ActionGroup actionGroup;

	public bool opened = false;

	void Awake () {
		Instance = this;
	}

	void Start () {
		
		HideCharacterStats ();

		HideMenuButtons ();

	}

	public void Init () {

		LootUI.useInventory += HandleUseInventory;

		StoryLauncher.Instance.playStoryEvent += HandlePlayStory;
		StoryLauncher.Instance.endStoryEvent += HandleEndStory;

	}

	#region crew group
	public void ShowCrewGroup () {
		crewGroup.SetActive (true);
		//
	}
	public void HideCrewGroup () {
		crewGroup.SetActive (false);
		//
	}
	#endregion

	#region events
	void HandleEndStory ()
	{
//		canOpen = true;
	}

	void HandlePlayStory ()
	{
//		canOpen = false;
		HideInventory ();
	}
	#endregion

	#region event handling
	void HandleUseInventory (InventoryActionType actionType)
	{
		switch (actionType) {
		case InventoryActionType.Eat:
			EatItem ();
			break;
		case InventoryActionType.Equip:
			EquipItem ();
			break;
		case InventoryActionType.Throw:
			ThrowItem ();
			break;
		case InventoryActionType.Sell:
			SellItem ();
			break;
		default:
			break;
		}

	}
	#endregion

	#region button action
	public void EatItem () {

		Item foodItem = LootUI.Instance.SelectedItem;

		int i = 0;

		switch (foodItem.spriteID) {
		// légume
		case 0:
			i = 25;
			break;
		// poisson
		case 1:
			i = 50;
			break;
		// viande
		case 2:
			i = 75;
			break;
		default:
			break;
		}

		if (CrewMember.GetSelectedMember.Health >= CrewMember.GetSelectedMember.MemberID.maxHealth - 10) {
			CrewMember.GetSelectedMember.CurrentHunger -= i;
		}
		CrewMember.GetSelectedMember.AddHealth (i);

		if (OtherInventory.Instance.type == OtherInventory.Type.None)
			LootManager.Instance.PlayerLoot.RemoveItem (LootUI.Instance.SelectedItem);
		else
			LootManager.Instance.OtherLoot.RemoveItem (LootUI.Instance.SelectedItem);

	}

	void EquipItem ()
	{
		Item item = LootUI.Instance.SelectedItem;

		if ( CrewMember.GetSelectedMember.CheckLevel (item.level) == false ) {
			return;
		}

		if (CrewMember.GetSelectedMember.GetEquipment(item.EquipmentPart) != null)
			LootManager.Instance.PlayerLoot.AddItem (CrewMember.GetSelectedMember.GetEquipment(item.EquipmentPart) );

		CrewMember.GetSelectedMember.SetEquipment (item);

		if ( OtherInventory.Instance.type == OtherInventory.Type.None )
			LootManager.Instance.PlayerLoot.RemoveItem (item);
		else 
			LootManager.Instance.OtherLoot.RemoveItem (item);

	}

	public void ThrowItem () {

		LootManager.Instance.PlayerLoot.RemoveItem (LootUI.Instance.SelectedItem);
	}

	public void SellItem () {

		int price = (int)(LootUI.Instance.SelectedItem.price / 2f);

		GoldManager.Instance.AddGold (price);

		LootManager.Instance.OtherLoot.AddItem (LootUI.Instance.SelectedItem);
		LootManager.Instance.PlayerLoot.RemoveItem (LootUI.Instance.SelectedItem);

	}
	#endregion

	#region properties
	public void ShowInventory ( CategoryContentType catContentType ) {
		ShowInventory (catContentType, Crews.playerCrew.captain);
	}
	public void ShowInventory ( CategoryContentType catContentType , CrewMember crewMember ) {

		if (!canOpen) {
			return;
		}

		// return
		CrewMember.SetSelectedMember (crewMember);

			// set bool
		opened = true;

			// event
		openInventory (crewMember);


		if (catContentType == CategoryContentType.Inventory) {
			LootUI.Instance.Hide ();
			HideCharacterStats ();
			ShowMenuButtons ();
		} else {
			LootUI.Instance.UpdateLootUI ();
		}

			// show elements
		ShowCrewGroup();

	}
	public void HideInventory () {

		if (opened == false)
			return;

			// set bool
		opened = false;

			// event
		closeInventory();

		LootUI.Instance.Hide ();

		// hide elements
		HideCrewGroup();
		HideMenuButtons ();

	}
	#endregion

	#region character stats
	public delegate void OnShowCharacterStats();
	public static OnShowCharacterStats onShowCharacterStats;
	public void ShowCharacterStats () {
		
		characterStatGroup.SetActive (true);
		HideMenuButtons ();

		if (onShowCharacterStats != null)
			onShowCharacterStats ();
	}
	public delegate void OnHideCharacterStats ();
	public static OnHideCharacterStats onHideCharacterStats;
	public void HideCharacterStats () {
		characterStatGroup.SetActive (false);
		ShowMenuButtons ();

		if (onHideCharacterStats != null)
			onHideCharacterStats ();
	}
	#endregion

	#region menu buttons
	public void ShowMenuButtons () {
		menuButtonsObj.SetActive (true);
	}
	public void HideMenuButtons () {
		menuButtonsObj.SetActive (false);
	}
	#endregion

	#region inventory button
	public void Open () {
		
		LootUI.Instance.Show (CategoryContentType.Inventory,Crews.Side.Player);

		HideMenuButtons ();
	}
	#endregion

}

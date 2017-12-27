﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayItem_Crew : DisplayItem {

	public delegate void OnRemoveItemFromMember(Item item);
	public static OnRemoveItemFromMember onRemoveItemFromMember;

	public CrewMember.EquipmentPart part;

	public Image itemImage;

	void Start () {
		CrewInventory.Instance.openInventory += HandleOpenInventory;
		LootUI.useInventory+= HandleUseInventory;

		Display ();
	}

	void HandleUseInventory (InventoryActionType actionType)
	{
		if (actionType == InventoryActionType.Equip) {
			Display ();
		}
	}

	void HandleOpenInventory (CrewMember member)
	{
		Display ();
	}

	void Display () {

		if ( CrewMember.selectedMember.GetEquipment(part)!= null) {
			HandledItem = CrewMember.selectedMember.GetEquipment (part);
		} else {
			Clear ();
		}
	}

	public void RemoveItem () {

		LootManager.Instance.getLoot(Crews.Side.Player).AddItem (HandledItem);

		if ( onRemoveItemFromMember != null )
			onRemoveItemFromMember (HandledItem);

		CrewMember.selectedMember.SetEquipment (part, null);

		Clear ();

	}

	public override Item HandledItem {
		get {
			return base.HandledItem;
		}
		set {

			base.HandledItem = value;

			if (value == null) {
				itemImage.enabled = false;
				return;
			}

			if (value.spriteID < 0) {
				itemImage.enabled = false;
			} else {
				itemImage.enabled = true;
				itemImage.sprite = LootManager.Instance.getItemSprite (value.category, value.spriteID);
			}

			itemImage.transform.rotation = Quaternion.Euler (new Vector3 (0, 0, Random.Range (-30, 30)));
		}
	}
}

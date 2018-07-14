﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Holoville.HOTween;

public class DisplayHeart : MonoBehaviour {

	public RectTransform backGround;
	public Image fillImage;

	// Use this for initialization
	void Start () {

		CrewInventory.Instance.onOpenInventory += HandleOpenInventory;
		LootUI.useInventory += HandleUseInventory;

		UpdateUI ();
	}

	void HandleOpenInventory (CrewMember crewMember)
	{
		UpdateUI ();
	}

	void UpdateUI () {

		CrewMember member = CrewMember.GetSelectedMember;

        float l = (float)member.Health / (float)member.MemberID.maxHealth;
        float width = -backGround.rect.width + backGround.rect.width * l;

        Vector2 v = new Vector2(width, fillImage.rectTransform.sizeDelta.y);
        fillImage.rectTransform.sizeDelta = v;

        Tween.Bounce (transform, 0.2f, 1.05f);
	}

	void HandleUseInventory (InventoryActionType actionType)
	{
		if ( actionType == InventoryActionType.Eat ) {
			UpdateUI ();
		}
	}
}

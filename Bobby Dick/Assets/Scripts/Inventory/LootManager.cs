﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LootManager : MonoBehaviour {

	public static LootManager Instance;

	[SerializeField]
	private CategoryContent defaultCategoryContent;
	[SerializeField]
	private CategoryContent tradeCategoryContent_Player;
	[SerializeField]
	private CategoryContent tradeCategoryContent_Other;

	[SerializeField]
	private CategoryContent lootCategoryContent_Player;
	[SerializeField]
	private CategoryContent lootCategoryContent_Other;

	[SerializeField]
	private CategoryContent tradeCategoryContent_Combat;

	[SerializeField]
	private CategoryContent inventoryCategoryContent;

	public delegate void UdpateLoot();
	public UdpateLoot updateLoot;

	private Loot playerLoot;
	private Loot otherLoot;

	void Awake (){
		Instance = this;
	}

	void Start () {
		StoryFunctions.Instance.getFunction += HandleGetFunction;
	}

	void HandleGetFunction (FunctionType func, string cellParameters)
	{
		switch (func) {
		case FunctionType.AddToInventory:
			AddToInventory ();
			break;
		case FunctionType.RemoveFromInventory:
			RemoveFromInventory ();
			break;
		case FunctionType.CheckInInventory:
			CheckInInventory ();
			break;
		}
	}



	public void CreateNewLoot () {
		Loot playerLoot = new Loot (0, 0);
		playerLoot.Randomize (new ItemCategory[1] {ItemCategory.Provisions});

		setLoot (Crews.Side.Player, playerLoot);
	}

	public Loot PlayerLoot {
		get {
			return playerLoot;
		}
	}

	public Loot OtherLoot {
		get {
			return otherLoot;
		}
	}

	public Loot getLoot (Crews.Side side) {
		return side == Crews.Side.Player ? playerLoot : otherLoot;
	}

	public void setLoot ( Crews.Side side , Loot targetLoot) {
		if (side == Crews.Side.Player) {
			playerLoot = targetLoot;
		} else {
			otherLoot = targetLoot;
		}
	}

	public Loot GetIslandLoot () {

		int row = StoryReader.Instance.Decal;
		int col = StoryReader.Instance.Index;

		var tmpLoot = StoryReader.Instance.CurrentStoryHandler.GetLoot (row, col);

		if (tmpLoot == null) {

			Loot newLoot = new Loot (row , col);

			ItemCategory[] categories = getLootCategoriesFromCell ();

			newLoot.Randomize (categories);

			StoryReader.Instance.CurrentStoryHandler.SetLoot (newLoot);

			return newLoot;

		}

		return tmpLoot;
	}

	public ItemCategory[] getLootCategoriesFromCell () {

		string cellParams = StoryFunctions.Instance.CellParams;

		if ( cellParams.Length < 2 ) {
			print ("pas de parms, toutes les cats");
			return ItemLoader.allCategories;
		}

		string[] cellParts = cellParams.Split ('/');

		ItemCategory[] categories = new ItemCategory[cellParts.Length];

		int index = 0;

		foreach ( string cellPart in cellParts ) {

			categories [index] = getLootCategoryFromString(cellPart);

			++index;
		}

		return categories;
	}

	public ItemCategory getLootCategoryFromString ( string arg ) {

		switch (arg) {
		case "Food":
			return ItemCategory.Provisions;
			break;
		case "Weapons":
			return ItemCategory.Weapon;
			break;
		case "Clothes":
			return ItemCategory.Clothes;
			break;
		case "Misc":
			return ItemCategory.Misc;
			break;
		}

		Debug.LogError ("getLootCategoryFromString : couldn't find category in : " + arg);

		return ItemCategory.Misc;

	}

	#region item
	void RemoveFromInventory () {

		string cellParams = StoryFunctions.Instance.CellParams;

		ItemCategory targetCat = getLootCategoryFromString (cellParams.Split('/')[1]);
		StoryReader.Instance.NextCell ();

		if ( LootManager.Instance.getLoot(Crews.Side.Player).getLoot[(int)targetCat].Length == 0 ) {

			StoryReader.Instance.SetDecal (1);

		} else {

			Item item = LootManager.Instance.getLoot(Crews.Side.Player).getLoot [(int)targetCat] [0];
			if (cellParams.Contains ("<")) {
				string itemName = cellParams.Split ('<') [1];
				itemName = itemName.Remove (itemName.Length - 6);
				item = System.Array.Find (LootManager.Instance.getLoot(Crews.Side.Player).getLoot [(int)targetCat], x => x.name == itemName);
				if (item == null) {
					StoryReader.Instance.SetDecal (1);
					StoryReader.Instance.UpdateStory ();
					return;
				}
			}

			DialogueManager.Instance.LastItemName = item.name;

			LootManager.Instance.getLoot(Crews.Side.Player).RemoveItem (item);

		}

		StoryReader.Instance.UpdateStory ();
	}

	void AddToInventory () {

		string cellParams = StoryFunctions.Instance.CellParams;

		ItemCategory targetCat = getLootCategoryFromString (cellParams.Split('/')[1]);

		Item item = null;

		if (cellParams.Contains ("<")) {
			string itemName = cellParams.Split ('<') [1];
			itemName = itemName.Remove (itemName.Length - 6);
			item = System.Array.Find (ItemLoader.Instance.getItems (targetCat), x => x.name == itemName);

		} else {
			item = ItemLoader.Instance.getRandomItem (targetCat);
		}

		getLoot(Crews.Side.Player).AddItem (item);

		DialogueManager.Instance.LastItemName = item.name;

		//		DialogueManager.Instance.SetDialogue (item.name, Crews.playerCrew .captain);

		StoryReader.Instance.NextCell ();
		StoryReader.Instance.UpdateStory ();
	}

	void CheckInInventory () {
		string cellParams = StoryFunctions.Instance.CellParams;


		StoryReader.Instance.NextCell ();

		string itemName = cellParams.Split ('<')[1];

		itemName = itemName.Remove (itemName.Length - 6);

		ItemCategory targetCat = getLootCategoryFromString (cellParams.Split('/')[1]);

		Item item = System.Array.Find (LootManager.Instance.getLoot(Crews.Side.Player).getCategory (targetCat), x => x.name == itemName);

		if (item == null) {
			StoryReader.Instance.SetDecal (1);
		} else {
			DialogueManager.Instance.LastItemName = item.name;
		}

		StoryReader.Instance.UpdateStory ();
	}
	#endregion

	public CategoryContent GetCategoryContent (CategoryContentType catContentType) {
		switch (catContentType) {
		case CategoryContentType.Inventory:
			return inventoryCategoryContent;
			break;

		case CategoryContentType.OtherLoot:
			return lootCategoryContent_Other;
			break;
		case CategoryContentType.PlayerLoot:
			return lootCategoryContent_Player;
			break;
		case CategoryContentType.PlayerTrade:
			return tradeCategoryContent_Player;
			break;
		case CategoryContentType.OtherTrade:
			return tradeCategoryContent_Other;
			break;
		case CategoryContentType.Combat:
			return tradeCategoryContent_Combat;
			break;
		}
		print ("category content reached zero");
		return defaultCategoryContent;
	}
}


public enum CategoryContentType {
	PlayerTrade,
	OtherTrade,
	Inventory,
	PlayerLoot,
	OtherLoot,
	Combat,
}

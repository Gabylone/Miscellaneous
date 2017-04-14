﻿using UnityEngine;
using System.Collections;

public class StoryFunctions : MonoBehaviour {

	public static StoryFunctions Instance;

	string cellParams = "";

	public string CellParams {
		get {
			return cellParams;
		}
	}

	private string[] functionNames;

	void Awake () {
		Instance = this;
	}

	public void Read ( string content ) {

		if (content.Length == 0) {
			Debug.LogError ("cell is empty on story " + IslandManager.Instance.CurrentIsland.Story.name + "" +
				"\n at row : " + (StoryReader.Instance.Index+2) + "" +
				"\n and collumn : " + StoryReader.Instance.Decal);
			Leave ();
			return;
		}

		if ( content[0] == '[' ) {
			StoryReader.Instance.NextCell ();
			StoryReader.Instance.UpdateStory ();
			return;
		}

		foreach ( string functionName in functionNames ) {

			if ( content.Contains (functionName) ){

				cellParams = content.Remove (0, functionName.Length);

				SendMessage (functionName);

				return;
			}

		}

		Debug.LogError (
			"cell returns no function at decal\n" + StoryReader.Instance.Decal + "\n" +
			"index : " + StoryReader.Instance.Index + "\n" +
			"qui contient : " + content);

	}

	#region random
	private void RandomPercent () {
		RandomManager.Instance.RandomPercent (cellParams);
	}
	private void RandomRange () {
		RandomManager.Instance.RandomRange (cellParams);
	}
	private void RandomRedoPercent () {
		RandomManager.Instance.RandomRedoPercent (cellParams);
	}
	private void RandomRedoRange () {
		RandomManager.Instance.RandomRedoRange (cellParams);
	}
	#endregion

	#region character & crew
	private void NewCrew () {
		Crews.Instance.CreateNewCrew ();
	}

	private void AddMember () {
		Crews.Instance.AddMemberToCrew ();
	}
	private void RemoveMember () {
		Crews.Instance.RemoveMemberFromCrew ();
	}
	#endregion

	#region hide & show
	private void HidePlayer() {
		Crews.playerCrew.Hide ();

		StoryReader.Instance.NextCell ();
		StoryReader.Instance.Wait (1f);
	}
	private void ShowPlayer () {
		Crews.playerCrew.ShowCrew ();

		StoryReader.Instance.NextCell ();
		StoryReader.Instance.Wait (1f);
	}
	private void HideOther () {
		Crews.enemyCrew.Hide ();

		StoryReader.Instance.NextCell ();
		StoryReader.Instance.Wait (1f);
	}
	private void ShowOther() {
		Crews.enemyCrew.ShowCrew ();

		StoryReader.Instance.NextCell ();
		StoryReader.Instance.Wait (1f);
	}
	#endregion

	#region boatUpgrades
	private void BoatUpgrades () {
		BoatUpgradeManager.Instance.ShowUpgradeMenu ();
		BoatUpgradeManager.Instance.Trading = true;
	}
	#endregion

	#region dialogue
	private void Narrator () {
		string phrase = cellParams.Remove (0,2);
		DialogueManager.Instance.ShowNarrator (phrase);
		StoryReader.Instance.WaitForInput ();
	}
	private void OtherSpeak () {

		string phrase = cellParams.Remove (0,2);

		if ( Crews.enemyCrew.CrewMembers.Count == 0 ) {
			Debug.LogError ("no enemy crew for other speak");
			return;
		}

		Crews.enemyCrew.captain.Icon.MoveToPoint (Crews.PlacingType.Discussion);

		DialogueManager.Instance.SetDialogue (phrase, Crews.enemyCrew.captain);

		StoryReader.Instance.WaitForInput ();

	}

	private void PlayerSpeak () {
		
		string phrase = cellParams.Remove (0,2);

		DialogueManager.Instance.SetDialogue (phrase, Crews.playerCrew.captain);

		StoryReader.Instance.WaitForInput ();
	}

	private void SetChoices () {
		DiscussionManager.Instance.GetChoices ();
	}

	private void GiveTip ()  {
		DiscussionManager.Instance.GiveTip ();
	}
	#endregion

	#region end
	void LaunchCombat () {
		Crews.enemyCrew.ManagedCrew.hostile = true;
		CombatManager.Instance.StartCombat ();
	}
	void Leave () {
		

		IslandManager.Instance.Leave ();
	}
	#endregion

	#region gold
	void CheckGold () {
		GoldManager.Instance.SetGoldDecal ();
	}
	void RemoveGold () {
		int amount = int.Parse (cellParams);
		GoldManager.Instance.GoldAmount -= amount;

		StoryReader.Instance.NextCell ();
		StoryReader.Instance.Wait ( 0.5f );
	}
	void AddGold () {
		int amount = int.Parse (cellParams);
		GoldManager.Instance.GoldAmount += amount;

		StoryReader.Instance.NextCell ();
		StoryReader.Instance.Wait (0.5f);
	}
	#endregion

	#region trade & loot
	void Loot() {
		LootManager.Instance.setLoot ( Crews.Side.Enemy, LootManager.Instance.GetIslandLoot(getLootCategories()));
		OtherLoot.Instance.StartLooting ();
	}
	void Trade() {

		ItemLoader.Instance.Mult = 3;

		LootManager.Instance.setLoot ( Crews.Side.Enemy, LootManager.Instance.GetIslandLoot(getLootCategories()));
		OtherLoot.Instance.StartTrade ();
	}

	public ItemCategory[] getLootCategories () {
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

	void RemoveFromInventory () {

		ItemCategory targetCat = getLootCategoryFromString (cellParams.Split('/')[1]);
		StoryReader.Instance.NextCell ();

		if ( LootManager.Instance.PlayerLoot.getLoot[(int)targetCat].Length == 0 ) {
			
			StoryReader.Instance.SetDecal (1);

		} else {

			Item item = LootManager.Instance.PlayerLoot.getLoot [(int)targetCat] [0];
			if (CellParams.Contains ("<")) {
				string itemName = cellParams.Split ('<') [1];
				itemName = itemName.Remove (itemName.Length - 6);
				item = System.Array.Find (LootManager.Instance.PlayerLoot.getLoot [(int)targetCat], x => x.name == itemName);
				if (item == null) {
					StoryReader.Instance.SetDecal (1);
					StoryReader.Instance.UpdateStory ();
					return;
				}
			}

			DialogueManager.Instance.LastItemName = item.name;

			LootManager.Instance.PlayerLoot.RemoveItem (item);

		}

		StoryReader.Instance.UpdateStory ();
	}

	void AddToInventory () {

		ItemCategory targetCat = getLootCategoryFromString (cellParams.Split('/')[1]);

		Item item = null;

		if (cellParams.Contains ("<")) {
			string itemName = cellParams.Split ('<') [1];
			itemName = itemName.Remove (itemName.Length - 6);
			item = System.Array.Find (ItemLoader.Instance.getItems (targetCat), x => x.name == itemName);
		} else {
			item = ItemLoader.Instance.getRandomItem (targetCat);
		}

		DialogueManager.Instance.LastItemName = item.name;

		LootManager.Instance.PlayerLoot.AddItem (item);

		StoryReader.Instance.NextCell ();
		StoryReader.Instance.UpdateStory ();
	}
	void CheckInInventory () {

		string itemName = cellParams.Split ('<')[1];

		itemName = itemName.Remove (itemName.Length - 6);

		ItemCategory targetCat = getLootCategoryFromString (cellParams.Split('/')[1]);

		Item item = System.Array.Find (LootManager.Instance.PlayerLoot.getCategory (targetCat), x => x.name == itemName);

		if (item == null)
			StoryReader.Instance.SetDecal (1);
		else
			DialogueManager.Instance.LastItemName = item.name;

		StoryReader.Instance.NextCell ();
		StoryReader.Instance.UpdateStory ();
	}
	#endregion

	#region story navigation
	private void ChangeStory () {
		StoryReader.Instance.ChangeStory ();
	}
	private void Node () {
		StoryReader.Instance.Node ();
	}
	private void Switch () {
		StoryReader.Instance.Switch ();
	}

	private void CheckFirstVisit () {

		StoryReader.Instance.NextCell ();

		if ( MapManager.Instance.CurrentIsland.visited == true) {
			StoryReader.Instance.SetDecal (1);
		}

		StoryReader.Instance.UpdateStory ();
	}
	#endregion

	#region weather
	void ChangeTimeOfDay () {
		if ( WeatherManager.Instance.IsNight )
			StartCoroutine (SetWeatherCoroutine ("Day"));
		else
			StartCoroutine (SetWeatherCoroutine ("Night"));

	}
	void SetWeather() {
		StartCoroutine (SetWeatherCoroutine (cellParams));
	}
	IEnumerator SetWeatherCoroutine (string weather) {

		Transitions.Instance.FadeScreen ();

		yield return new WaitForSeconds (Transitions.Instance.ScreenTransition.Duration);

		switch ( weather ) {
		case "Day":
			WeatherManager.Instance.IsNight = false;
			WeatherManager.Instance.Raining = false;
			break;
		case "Night":
			WeatherManager.Instance.IsNight = true;
			WeatherManager.Instance.Raining = false;
			break;
		case "Rain":
			WeatherManager.Instance.Raining = true;
			break;
		}

		NavigationManager.Instance.UpdateTime ();

		yield return new WaitForSeconds (Transitions.Instance.ScreenTransition.Duration);

		StoryReader.Instance.NextCell ();
		StoryReader.Instance.UpdateStory ();
	}

	void Fade () {

		Transitions.Instance.FadeScreen ();

		StoryReader.Instance.NextCell ();
		StoryReader.Instance.Wait (Transitions.Instance.ActionTransition.Duration * 2);

	}

	void CheckDay () {

		StoryReader.Instance.NextCell ();

		if (WeatherManager.Instance.IsNight)
			StoryReader.Instance.SetDecal (1);

		StoryReader.Instance.UpdateStory ();
	}
	#endregion

	#region clues
	void CheckClues () {
		ClueManager.Instance.StartClue ();
	}
	void GiveClue() {

		string formula = getFormula ();

		Debug.Log (formula);

		if ( Crews.enemyCrew.CrewMembers.Count == 0 ) {
			DialogueManager.Instance.ShowNarrator (formula);
		} else {
			Crews.enemyCrew.captain.Icon.MoveToPoint (Crews.PlacingType.Discussion);
			DialogueManager.Instance.SetDialogue (formula, Crews.enemyCrew.captain);

		}

		StoryReader.Instance.WaitForInput ();

	}
	void GiveDirectionToClue () {

		if ( cellParams.Length == 0 ) {
			DialogueManager.Instance.SetDialogue (getDirectionToFormula (), Crews.enemyCrew.captain);
		} else {
			DialogueManager.Instance.SetDialogue (getDirectionToFormula (), Crews.playerCrew.captain);
		}

		StoryReader.Instance.WaitForInput ();
	}

	string getDirectionToFormula () {
		Directions dir = NavigationManager.Instance.getDirectionToPoint (ClueManager.Instance.GetNextClueIslandPos);
		string directionPhrase = NavigationManager.Instance.getDirName (dir);

		return directionPhrase;
	}

	string getFormula () {
		
		int clueIndex = ClueManager.Instance.ClueIndex;

		string clue = "";

		bool clueAlreadyFound = false;

		int a = 0;

		foreach ( int i in ClueManager.Instance.ClueIslands ) {

			if ( i == MapManager.Instance.IslandID ) {
				Debug.Log ("already found clue in island");
				clue = ClueManager.Instance.Clues [a];
				clueIndex = a;
				clueAlreadyFound = true;
			}

			++a;

		}

		if ( clueAlreadyFound == false ) {
			Debug.Log ("first time gave clue");
			clue = ClueManager.Instance.Clues[clueIndex];
			ClueManager.Instance.ClueIndex += 1;
		}

		ClueManager.Instance.ClueIslands [clueIndex] = MapManager.Instance.IslandID;

		return clue;
	}
	#endregion

	#region dice
	private void CheckStat () {

		StartCoroutine (CheckStat_Coroutine ());

	}

	IEnumerator CheckStat_Coroutine () {

		DiceManager.Instance.ThrowDirection = 1;

		switch ( cellParams ) {
		case "SRT":
			DiceManager.Instance.ThrowDice (DiceTypes.STR, Crews.playerCrew.captain.Strenght);
			break;
		case "DEX" :
			DiceManager.Instance.ThrowDice (DiceTypes.DEX, Crews.playerCrew.captain.Dexterity);
			break;
		case "CHA" :
			DiceManager.Instance.ThrowDice (DiceTypes.CHA, Crews.playerCrew.captain.Charisma);
			break;
		case "CON" :
			DiceManager.Instance.ThrowDice (DiceTypes.CON, Crews.playerCrew.captain.Constitution);
			break;
		}


		yield return new WaitForSeconds ( DiceManager.Instance.settlingDuration + DiceManager.Instance.ThrowDuration);

		int captainHighest = DiceManager.Instance.getHighestThrow;
		int otherHighest = 0;

		if (CombatManager.Instance.Fighting) {

			DiceManager.Instance.ThrowDirection = -1;

			switch (cellParams) {
			case "SRT":
				DiceManager.Instance.ThrowDice (DiceTypes.STR, Crews.enemyCrew.captain.Strenght);
				break;
			case "DEX":
				DiceManager.Instance.ThrowDice (DiceTypes.DEX, Crews.enemyCrew.captain.Dexterity);
				break;
			case "CHA":
				DiceManager.Instance.ThrowDice (DiceTypes.CHA, Crews.enemyCrew.captain.Charisma);
				break;
			case "CON":
				DiceManager.Instance.ThrowDice (DiceTypes.CON, Crews.enemyCrew.captain.Constitution);
				break;
			}

			yield return new WaitForSeconds (DiceManager.Instance.settlingDuration + DiceManager.Instance.ThrowDuration);

			otherHighest = DiceManager.Instance.getHighestThrow;

		} else {
			otherHighest = 5;
		}

		StoryReader.Instance.NextCell ();

		StoryReader.Instance.SetDecal (otherHighest > captainHighest ? 0 : 1);

		StoryReader.Instance.UpdateStory ();
	}

	#endregion

	public string[] FunctionNames {
		get {
			return functionNames;
		}
		set {
			functionNames = value;
		}
	}
}
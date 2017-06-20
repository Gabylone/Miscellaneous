﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SaveManager : MonoBehaviour
{
	public static SaveManager Instance;

	public delegate void LoadGameData ();
	public event LoadGameData loadData;

	public delegate void SaveGameData ();
	public event SaveGameData saveData;

	void Awake () {
		Instance = this;
	}

	void Start () {
		currentData = new GameData ();
	}

	#region action
	public void LoadGame (int index) {

		currentData = SaveTool.Instance.Load (index);

		StartCoroutine (LoadGameCoroutine ());
	}

	IEnumerator LoadGameCoroutine () {



		Transitions.Instance.ScreenTransition.Fade = true;
		yield return new WaitForSeconds (Transitions.Instance.ScreenTransition.Duration);

		if ( StoryLauncher.Instance.PlayingStory)
			StoryLauncher.Instance.PlayingStory = false;


		// player crew
		Crews.Instance.LoadPlayerCrew ();

		// boat position

		PlayerBoatInfo.Instance = CurrentData.playerBoatInfo;
		Boats.Instance.OtherBoatInfos = CurrentData.otherBoatInfos;

		// island ids
		// island datas
		// special island positions
		MapGenerator.Instance.LoadIslandsData ();

		// player loot
		LootManager.Instance.setLoot (Crews.Side.Player, currentData.playerLoot);

		// gold
		GoldManager.Instance.GoldAmount = CurrentData.playerGold;

		WeatherManager.Instance.LoadWeather ();

		MapImage.Instance.InitImage ();
		NavigationManager.Instance.ChangeChunk (Directions.None);

		yield return new WaitForSeconds (Transitions.Instance.ScreenTransition.Duration);
		Transitions.Instance.ScreenTransition.Fade = false;

	}
	public void SaveGame (int index) {

		// player crew
		Crews.Instance.SavePlayerCrew ();

		// save boats
		currentData.playerBoatInfo = PlayerBoatInfo.Instance;
		currentData.otherBoatInfos = Boats.Instance.OtherBoatInfos;

		// island ids
		// island datas
		// special island positions
		MapGenerator.Instance.SaveIslandsData ();

		// player loot
		currentData.playerLoot = LootManager.Instance.getLoot (Crews.Side.Player);

		// gold
		CurrentData.playerGold = GoldManager.Instance.GoldAmount;

		WeatherManager.Instance.SaveWeather ();


		SaveTool.Instance.Save (index);
	}
	#endregion

	GameData currentData;

	public GameData CurrentData {
		get {
			return currentData;
		}
	}

}


//[System.Serializable]
public class GameData
{
	// crew
	public Crew 			playerCrew;

	// islands
	public MapData 			mapData;

	public Chunk[][] 		chunkArray;

	public PlayerBoatInfo 	playerBoatInfo;
	public OtherBoatInfo[] 	otherBoatInfos;

	public StoryHandler storyHandler;

	public Loot 			playerLoot;

	public int 				playerWeight = 0;
	public int 				playerGold = 0;

	public bool 			raining = false;
	public int 				currentRain = 0;

	public bool 			night = false;
	public int 				currentNight = 0;

	public GameData()
	{
		// islands ids
	}
}
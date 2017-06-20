﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class StoryLoader : MonoBehaviour {

	public static StoryLoader Instance;

	private List<Story> islandStories 	= new List<Story> ();
	private List<Story> clueStories 	= new List<Story> ();
	private List<Story> treasureStories = new List<Story> ();
	private List<Story> homeStories 	= new List<Story> ();
	private List<Story> boatStories 	= new List<Story> ();

	public bool checkNodes = false;

	private TextAsset[] storyFiles;
	[SerializeField]
	private TextAsset functionFile;

	private float minFreq = 0f;

	[SerializeField]
	private StoryFunctions storyFunctions;

	int kek = 0;

	void Awake () {
		
		Instance = this;

		LoadFunctions ();
		LoadStories ();

	}

	void Update () {
		if ( Input.GetKeyDown(KeyCode.Insert) ) {
			StartCoroutine (CheckAllNodes ());
		}
	}

	public void LoadStories ()
	{
		LoadSheets (	islandStories	, 		"Stories/CSVs/IslandStories"	);
		LoadSheets (	boatStories		, 		"Stories/CSVs/BoatStories"		);
		LoadSheets (	homeStories		, 		"Stories/CSVs/HomeStories"		);
		LoadSheets (	clueStories		, 		"Stories/CSVs/ClueStories"		);
		LoadSheets (	treasureStories	, 		"Stories/CSVs/TreasureStories"	);
	}


	private void LoadSheets (List<Story> storyList , string path)
	{
		minFreq = 0f;

		GetFiles (path);
		for (int i = 0; i < storyFiles.Length; ++i )
			storyList.Add(LoadSheet (i));

		++kek;
	}

	private void GetFiles (string path)
	{
		storyFiles = new TextAsset[Resources.LoadAll (path, typeof(TextAsset)).Length];

		int index = 0;
		foreach ( TextAsset textAsset in Resources.LoadAll (path, typeof(TextAsset) )) {
			storyFiles[index] = textAsset;
			++index;
		}
	}


	
	private Story LoadSheet (int index)
	{
		string[] rows = storyFiles[index].text.Split ('\n');

		int collumnIndex 	= 0;

		Story newStory = new Story ("name");

		for (int rowIndex = 1; rowIndex < rows.Length; ++rowIndex ) {

			string[] rowContent = rows[rowIndex].Split (';');

			// create story
			if (rowIndex == 1) 
			{
				newStory.name = rowContent [0];

				float frequence = 0f;

				bool canParse = float.TryParse (rowContent [1] ,out frequence);

				if ( canParse== false){ 
					print ("ne peut pas parse la freq dans : " + newStory.name + " TRY PARSE : " + rowContent[1]);
				}

				frequence = (frequence/100);

					// set story frequence
				newStory.freq = frequence;
				newStory.rangeMin = minFreq;
				newStory.rangeMax = minFreq + newStory.freq;

				// story visials
//				VisualStory (newStory);

				minFreq += newStory.freq;

				newStory.spriteID = int.Parse (rowContent [2]);

				foreach (string cellContent in rowContent) {
					newStory.content.Add (new List<string> ());
				}
			}
			else
			{
				foreach (string cellContent in rowContent) {

					if ( cellContent.Length > 0 && cellContent[0] == '[' ) {
						string markName = cellContent.Remove (0, 1).Remove (cellContent.IndexOf (']')-1);
						newStory.nodes.Add (new Node (markName, collumnIndex, (rowIndex-2)));
					}

					newStory.content [collumnIndex].Add (cellContent);

					++collumnIndex;

				}
			}

			collumnIndex = 0;

		}

		return newStory;
	}

	private void LoadFunctions () {
		
		string[] rows = functionFile.text.Split ( '\n' );

		storyFunctions.FunctionNames = new string[rows.Length-1];

		for (int row = 0; row < storyFunctions.FunctionNames.Length; ++row ) {
			storyFunctions.FunctionNames [row] = rows [row].Split (';') [0];

		}
	}

	#region random story from position
	public Story RandomStory (int x , int y) {
		return IslandStories[RandomStoryIndex (x, y)];
	}
	public StoryType GetTypeFromPos (int x, int y)
	{
		if (x == MapData.Instance.treasureIslandXPos &&
			y == MapData.Instance.treasureIslandYPos ) {
			return StoryType.Treasure;
		}

		// check for home island
		if (x == MapData.Instance.homeIslandXPos &&
			y == MapData.Instance.homeIslandYPos ) {
			return StoryType.Home;
		}

		// check if clue island
		for( int i = 0; i < ClueManager.Instance.ClueAmount ; ++i ) {
			if (x == MapData.Instance.clueIslandsXPos[i] &&
				y == MapData.Instance.clueIslandsYPos[i] ) {
				return StoryType.Clue;
			}
		}

		return StoryType.Island;
	}
	public int RandomStoryIndex (int x, int y)
	{
		if (x == MapData.Instance.treasureIslandXPos &&
			y == MapData.Instance.treasureIslandYPos ) {

			if (treasureStories.Count == 0)
				Debug.LogError ("no treasure stories");

			return getStoryIndexFromPercentage(treasureStories);

		}

		// check for home island
		if (x == MapData.Instance.homeIslandXPos &&
			y == MapData.Instance.homeIslandYPos ) {

			if (homeStories.Count == 0)
				Debug.LogError ("no home stories");

			return getStoryIndexFromPercentage (homeStories);

		}

		// check if clue island
		for( int i = 0; i < ClueManager.Instance.ClueAmount ; ++i ) {

			if (x == MapData.Instance.clueIslandsXPos[i] &&
				y == MapData.Instance.clueIslandsYPos[i] ) {

				if (clueStories.Count == 0)
					Debug.LogError ("no clue stories");

				return getStoryIndexFromPercentage (clueStories);

			}
		}

		return getStoryIndexFromPercentage (islandStories);
	}
	#endregion

	#region percentage
	public int getStoryIndexFromPercentage ( StoryType type ) {

		List<Story> stories = new List<Story> ();

		switch (type) {
		case StoryType.Island:
			stories =  IslandStories;
			break;
		case StoryType.Treasure:
			stories = TreasureStories;
			break;
		case StoryType.Home:
			stories = HomeStories;
			break;
		case StoryType.Clue:
			stories = ClueStories;
			break;
		case StoryType.Boat:
			stories = BoatStories;
			break;
		default:
			stories = IslandStories;
			break;
		}

		return getStoryIndexFromPercentage (stories);
	}
	public int getStoryIndexFromPercentage ( List<Story> stories ) {

		float random = Random.value * 100f;

		int a = 0;

		foreach (Story story in stories) {
			if (random < story.rangeMax && random >= story.rangeMin) {
				return a;
			}

			++a;
		}

//		Debug.LogError ("out of percentage, returning random story index : (random : " + random + ")");

		return Random.Range (0,stories.Count);
	}
	#endregion
	void VisualStory (Story newStory)
	{
		float scale = 1f;
		//
		GameObject obj = GameObject.CreatePrimitive (PrimitiveType.Cube);
		obj.transform.localScale = new Vector3 (newStory.freq, 1f, 1f);
		obj.transform.position = new Vector3 ( minFreq + (newStory.freq/2), kek , 0f );
		obj.GetComponent<Renderer> ().material.color = Random.ColorHSV ();
		obj.name = newStory.name;
	}

	public Story FindByName (string storyName)
	{
		return IslandStories[FindIndexByName (storyName)];
	}

	public int FindIndexByName (string storyName)
	{
		int storyIndex = IslandStories.FindIndex (x => x.name == storyName);

		if (storyIndex < 0) {
			Debug.LogError ("coun't find story /" + storyName + "/, returning first");
			return 0;
		}

		return storyIndex;
	}

	#region check nodes
	IEnumerator CheckAllNodes ()
	{
		CheckNodes (islandStories);
		yield return new WaitForEndOfFrame ();
		CheckNodes (boatStories);
		yield return new WaitForEndOfFrame ();
		CheckNodes (homeStories);
		yield return new WaitForEndOfFrame ();
		CheckNodes (clueStories);
		yield return new WaitForEndOfFrame ();
		CheckNodes (treasureStories);
	}

	Story storyToCheck;

	void CheckNodes (List<Story> stories)
	{
		foreach (Story story in stories) {
			storyToCheck = story;
			CheckNodes_Story (story);
		}
	}

	void CheckNodes_Story ( Story story ) {
		
		foreach (List<string> contents in story.content) {

			CheckNodes_CheckCells (contents);

		}
	}

	void CheckNodes_CheckCells (List<string> contents)
	{
		foreach (string content in contents) {

			CheckNodes_CheckCell (content);

		}
	}

	void CheckNodes_CheckCell (string cellContent)
	{
			// check if empty
		if (cellContent.Length == 0)
			return;

			// check if node
		if ( cellContent[0] == '[' ) {
			return;
		}

			// CHECK FOR FUNCTION
		bool cellContainsFunction = false;

		string functionFound = "";

		foreach (string functionName in storyFunctions.FunctionNames) {

			if (cellContent.Contains (functionName)) {
				cellContainsFunction = true;
				functionFound = functionName;
				break;
			}

		}

		if (cellContainsFunction == false) {
			Debug.LogError ("" +
				"There's no function in the cell :\n" +
				"STORY : " + storyToCheck.name + " / CELL CONTENT : " + cellContent);
			return;
		}

		if (functionFound != "Node")
			return;

		// CHECK NODE
		bool nodeIsLinked = false;

		string nodeName = cellContent.Remove (0, 6);

		foreach (Node node in storyToCheck.nodes) {
			if (nodeName == node.name) {
				nodeIsLinked = true;
				return;
			}
		}

		if (!nodeIsLinked) {
			Debug.LogError ("" +
				"There's a node function, but the node has no link :\n" +
				"STORY : " + storyToCheck.name + " / CELL CONTENT : " + cellContent + " / TARGET NODE : " + nodeName);
		}
	}

	#endregion

	#region story getters
	public List<Story> IslandStories {
		get {
			return islandStories;
		}
		set {
			islandStories = value;
		}
	}
	public List<Story> TreasureStories {
		get {
			return treasureStories;
		}
	}

	public List<Story> BoatStories {
		get {
			return boatStories;
		}
	}
	public List<Story> ClueStories {
		get {
			return clueStories;
		}
	}

	public List<Story> HomeStories {
		get {
			return homeStories;
		}
	}
	#endregion
}

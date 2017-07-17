﻿using UnityEngine;
using System.Collections;

public class StoryTest : MonoBehaviour {

	public static StoryTest Instance;

	int lign = 0;
	int decal = 0;

	public int storyID = 0;

	public bool launchStoryOnStart;

	public string testStoryName = "Maison";
	public StoryType testStoryType;
	public string nodeName = "";

	public int X1 = 0;
	public int Y1 = 0;
	public int X2 = 0;
	public int Y2 = 0;

	void Awake () {
		Instance = this;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
		if (Input.GetKeyDown(KeyCode.Return) ) {
			StoryReader.Instance.CurrentStoryManager.storyHandlers [0].storyType = testStoryType;
			StoryReader.Instance.CurrentStoryManager.storyHandlers[0].storyID = StoryLoader.Instance.FindIndexByName (testStoryName,testStoryType);
			StoryLauncher.Instance.PlayStory (StoryReader.Instance.CurrentStoryManager,StoryLauncher.StorySource.island);
		}

//
//		if (Input.GetKeyDown(KeyCode.PageUp) ) {
//
//			Boats.Instance.PlayerBoatInfo.PosX = MapData.Instance.treasureIslandXPos;
//			Boats.Instance.PlayerBoatInfo.PosY = MapData.Instance.treasureIslandYPos;
//			NavigationManager.Instance.ChangeChunk (Directions.None);
//
//			Debug.Log (" player Y : " + Boats.Instance.PlayerBoatInfo.PosX);
//			Debug.Log (" player X : " + Boats.Instance.PlayerBoatInfo.PosY);
//			Debug.Log ("island X : " + MapData.Instance.treasureIslandXPos);
//			Debug.Log ("island Y : " + MapData.Instance.treasureIslandYPos);
//		}

	}

	void checkDirection () {
		Vector2 dir = (new Vector2 (X2, Y2) - new Vector2 (X1, Y1));

		for (int i = 0; i < 8; ++i ) {

			if ( Vector2.Angle ( dir , NavigationManager.Instance.getDir((Directions)i) ) < 45f ) {
				Debug.Log (NavigationManager.Instance.getDirName((Directions)(i)));
			}

		}
	}
}
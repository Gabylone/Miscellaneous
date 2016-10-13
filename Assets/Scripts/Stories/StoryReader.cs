﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StoryReader : MonoBehaviour {

	public static StoryReader Instance;

	private Story currentStory;

	private int index = 0;
	private int decal = 0;

	bool waitForInput = false;
	bool waitToNextCell = false;
	float timer = 0f;

	void Awake () {
		Instance = this;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		if ( waitForInput ) {
			if ( Input.GetMouseButtonDown (0) ) {

				PressInput ();

			}
		}

		if ( waitToNextCell ) {
			timer -= Time.deltaTime;

			if (timer <= 0) {

				waitToNextCell = false;
				NextCell ();
				UpdateStory ();
			}
		}

	}

	public void PickRandomStory () {

//		currentStory = StoryLoader.Instance.Stories [Random.Range (0, StoryLoader.Instance.Stories.Count)];
		currentStory = StoryLoader.Instance.Stories [1];
		SetStory (currentStory);
	}

	#region wait for input
	[SerializeField]
	private GameObject inputButton;

	public void WaitForInput () {
		waitForInput = true;

		inputButton.SetActive (true);
	}
	public void PressInput () {
		waitForInput = false;
		inputButton.SetActive (false);

		NextCell ();
		UpdateStory ();
	}
	#endregion

	public void Wait ( float duration ) {
		waitToNextCell = true;
		timer = duration;
	}

	#region navigation
	public void SetStory ( Story newStory ) {

		index = 0;
		decal = 0;

		UpdateStory ();
	}
	public void UpdateStory () {

		StoryFunctions.Instance.Read ( StoryLoader.Instance.GetContent );

	}
	public void NextCell () {
		++index;
	}
	#endregion

	#region decal
	public void SetDecal ( int steps ) {

		while (steps > 0) {

			++decal;
			string content = StoryLoader.Instance.GetContent;

			if (content.Length > 0) {
				--steps;
			}
		}

	}
	#endregion

	#region properties
	public Story CurrentStory {
		get {
			return currentStory;
		}
		set {
			currentStory = value;
		}
	}

	public int Index {
		get {
			return index;
		}
		set {
			index = value;
		}
	}

	public int Decal {
		get {
			return decal;
		}
		set {
			decal = value;
		}
	}
	public bool WaitForInput2 {
		get {
			return waitForInput;
		}
		set {
			waitForInput = value;
		}
	}
	#endregion
}

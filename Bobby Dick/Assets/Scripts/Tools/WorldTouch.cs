﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldTouch : MonoBehaviour {

    public static WorldTouch Instance;

	public delegate void OnTouchWorld ();
	public static OnTouchWorld onPointerExit;

	public delegate void OnPointerDownEvent ();
	public static OnPointerDownEvent onPointerDown;


    public bool touching = false;

	float timeToTouch = 0.25f;

    private void Awake()
    {
        Instance = this;
    }

    // Use this for initialization
    void Start () {

		Swipe.onSwipe += HandleOnSwipe;

        NavigationManager.Instance.EnterNewChunk += HandleChunkEvent;

	}

	void HandleChunkEvent ()
	{
		
	}

	void HandleOnSwipe (Directions direction)
	{
		touching = true;

		timer = 0f;

		timer = timeToTouch + 1;
	}
	
	// Update is called once per frame
	void Update () {
		if (touching) {
			timer += Time.deltaTime;
		}
	}

	float timer = 0f;

	public void OnPointerDown () {

        touching = true;

		if (onPointerDown != null) {
			onPointerDown ();
		}
	}

	public void OnPointerUp () {

		touching = false;

		if (timer > timeToTouch) {
			return;
		}

		if (onPointerExit != null) {
			onPointerExit ();
		}
	}
}

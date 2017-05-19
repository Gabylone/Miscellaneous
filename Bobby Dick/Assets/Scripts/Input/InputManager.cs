﻿using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour {

	public static InputManager Instance;

	public enum ScreenPart {
		Any,
		Left,
		Right
	}

	[SerializeField]
	private VirtualJoystick virtualJoystick;

	public bool mobileTest = false;

	void Awake () {
		Instance = this;
	}

//	void Update () {
//		if ( OnInputDown (0,ScreenPart.Left) ) {
//			print ("touch left");
//		}
//		if ( OnInputDown (0,ScreenPart.Right) ) {
//			print ("touch right");
//		}
//		if ( OnInputDown (0,ScreenPart.Any) ) {
//			print ("touch any");
//		}
//	}

	#region get touch & click
	/// <summary>
	/// Raises the input down event.
	/// </summary>
	public bool OnInputDown () {
		return OnInputDown (0,ScreenPart.Any);
	}
	public bool OnInputDown (int id, ScreenPart screenPart) {

		bool rightSideOfScreen = GetInputPosition ().x > 0;
		if (screenPart == ScreenPart.Left)
			rightSideOfScreen = GetInputPosition ().x <= Screen.width / 2;
		if (screenPart == ScreenPart.Right)
			rightSideOfScreen = GetInputPosition ().x > Screen.width / 2;

		if (OnMobile)
			return Input.GetTouch (id).phase == TouchPhase.Began && rightSideOfScreen;
		else
			return Input.GetMouseButtonDown (id) && rightSideOfScreen;
	}

	/// <summary>
	/// Raises the input stay event.
	/// </summary>
	public bool OnInputStay () {
		return OnInputStay (0,ScreenPart.Any);
	}
	public bool OnInputStay (int id, ScreenPart screenPart) {

		bool rightSideOfScreen = GetInputPosition ().x > 0;
		if (screenPart == ScreenPart.Left)
			rightSideOfScreen = GetInputPosition ().x <= Screen.width / 2;
		if (screenPart == ScreenPart.Right)
			rightSideOfScreen = GetInputPosition ().x > Screen.width / 2;

		if (OnMobile)
			return (Input.GetTouch (id).phase == TouchPhase.Stationary || Input.GetTouch (id).phase == TouchPhase.Moved) && rightSideOfScreen;
		else
			return Input.GetMouseButton (id) && rightSideOfScreen;
	}

	/// <summary>
	/// Raises the input exit event.
	/// </summary>
	public bool OnInputExit () {
		return OnInputExit (0,ScreenPart.Any);
	}
	public bool OnInputExit (int id, ScreenPart screenPart) {

		bool rightSideOfScreen = GetInputPosition ().x > 0;
		if (screenPart == ScreenPart.Left)
			rightSideOfScreen = GetInputPosition ().x <= Screen.width / 2;
		if (screenPart == ScreenPart.Right)
			rightSideOfScreen = GetInputPosition ().x > Screen.width / 2;

		if (OnMobile)
			return (Input.GetTouch (id).phase == TouchPhase.Ended) && rightSideOfScreen;
		else
			return Input.GetMouseButtonUp (id) && rightSideOfScreen;
	}

	/// <summary>
	/// Gets the input position.
	/// </summary>
	/// <returns>The input position.</returns>
	public Vector3 GetInputPosition () {
		return GetInputPosition (0);
	}
	public Vector3 GetInputPosition (int id) {
		if (OnMobile) {
			return Input.GetTouch (id).position;
		} else {
			return Input.mousePosition;
		}
	}
	#endregion


	#region get axis
	/// <summary>
	/// Gets the horizontal axis.
	/// </summary>
	/// <returns>The horizontal axis.</returns>
	public float GetHorizontalAxis () {

		if (OnMobile || mobileTest) {
			return virtualJoystick.GetHorizontalAxis ();
		} else {
			return Input.GetAxis ("Horizontal");
		}

	}

	/// <summary>
	/// Gets the vertical axis.
	/// </summary>
	/// <returns>The vertical axis.</returns>
	public float GetVerticalAxis () {

		if (OnMobile || mobileTest) {
			return virtualJoystick.GetVerticalAxis();
		} else {
			return Input.GetAxis ("Vertical");
		}

	}
	#endregion

	public bool OnMobile {
		get {
			return Application.isMobilePlatform;
		}
	}
}

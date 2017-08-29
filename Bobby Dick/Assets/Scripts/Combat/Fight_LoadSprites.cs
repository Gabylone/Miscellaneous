﻿using UnityEngine;
using System.Collections;

public class Fight_LoadSprites : MonoBehaviour {

	public enum SpriteIndex {
		body,
		rightArm,
		leftArm,
		weapon,
		head,
		eyes,
		eyebrows,
		hair,
		nose,
		mouth,
		beard,
	}

	SpriteRenderer[] allSprites;
	float fade_Duration;
	Color[] fade_InitColors;
	bool fading = false;
	float timer = 0f;

	public void Init ()
	{
		allSprites = GetComponentsInChildren<SpriteRenderer> (true);
		fade_InitColors = new Color[allSprites.Length];


		for (int i = 0; i < fade_InitColors.Length; i++) {
			fade_InitColors [i] = allSprites [i].color;
		}

	}

	void Update () {
		if (fading) {
			Fade_Update ();
		}
	}

	public void UpdateSprites ( MemberID memberID ) {

		if (memberID.HairSpriteID > -1)
			allSprites[(int)SpriteIndex.hair].sprite = memberID.Male ? CrewCreator.Instance.HairSprites_Male [memberID.HairSpriteID] : CrewCreator.Instance.HairSprites_Female [memberID.HairSpriteID];
		else
			allSprites[(int)SpriteIndex.hair].enabled = false;
		
		allSprites[(int)SpriteIndex.hair].color = CrewCreator.Instance.HairColors [memberID.HairColorID];

		if (memberID.BeardSpriteID > -1)
			allSprites[(int)SpriteIndex.beard].sprite = CrewCreator.Instance.BeardSprites [memberID.BeardSpriteID];
		else
			allSprites[(int)SpriteIndex.beard].enabled = false;
		allSprites[(int)SpriteIndex.beard].color = CrewCreator.Instance.HairColors [memberID.HairColorID];

		allSprites[(int)SpriteIndex.eyes].sprite = CrewCreator.Instance.EyesSprites [memberID.EyeSpriteID];
		allSprites[(int)SpriteIndex.eyebrows].sprite = CrewCreator.Instance.EyebrowsSprites [memberID.EyebrowsSpriteID];
		allSprites[(int)SpriteIndex.eyebrows].color = CrewCreator.Instance.HairColors [memberID.HairColorID];

		allSprites[(int)SpriteIndex.nose].sprite = CrewCreator.Instance.NoseSprites [memberID.NoseSpriteID];

		allSprites[(int)SpriteIndex.mouth].sprite = CrewCreator.Instance.MouthSprites [memberID.MouthSpriteID];

		// body
		allSprites[(int)SpriteIndex.body].sprite = CrewCreator.Instance.BodySprites[memberID.Male ? 0:1];

	}

	#region fade
	public void Fade_Reset () {
		int a = 0;
		foreach ( SpriteRenderer sprite in allSprites ) {
			sprite.color = fade_InitColors [a];
			++a;
		}

		timer = 0f;
		fading = false;
	}
	public void Fade_Start (float dur) {
		fade_Duration = dur;

		fading = true;

		timer = 0f;
	}
	void Fade_Update ()
	{
		int a = 0;
		foreach ( SpriteRenderer sprite in allSprites ) {
			sprite.color = Color.Lerp (fade_InitColors [a], Color.clear, timer / fade_Duration);
			++a;
		}

		timer += Time.deltaTime;

		if (timer >= fade_Duration)
			fading = false;
	}
	#endregion

	public void UpdateOrder (int fighterIndex)
	{
		foreach ( SpriteRenderer sprite in allSprites ) {
			sprite.sortingOrder += 11 * (fighterIndex+1);
		}

	}
}

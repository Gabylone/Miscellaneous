﻿using UnityEngine;
using System.Collections;

public class Fight_LoadSprites : MonoBehaviour {

	[SerializeField]
	private SpriteRenderer bodySprite;
	[SerializeField]
	private SpriteRenderer hairSprite;
	[SerializeField]
	private SpriteRenderer eyesSprite;
	[SerializeField]
	private SpriteRenderer eyebrowsSprite;
	[SerializeField]
	private SpriteRenderer noseSprite;
	[SerializeField]
	private SpriteRenderer mouthSprite;

	public void UpdateSprites ( MemberID memberID ) {

		int hearIndex = memberID.hairSpriteID;
		if (hearIndex > -1)
			hairSprite.sprite = CrewCreator.Instance.HairSprites [hearIndex];
		else
			hairSprite.enabled = false;
		hairSprite.color = CrewCreator.Instance.HairColors [memberID.hairColorID];

		eyesSprite.sprite = CrewCreator.Instance.EyesSprites [memberID.eyeSpriteID];
		eyebrowsSprite.sprite = CrewCreator.Instance.EyebrowsSprites [memberID.eyebrowsSpriteID];
		eyebrowsSprite.color = CrewCreator.Instance.HairColors [memberID.hairColorID];

		noseSprite.sprite = CrewCreator.Instance.NoseSprites [memberID.noseSpriteID];
		noseSprite.color = CrewCreator.Instance.HairColors [memberID.hairColorID];

		mouthSprite.sprite = CrewCreator.Instance.MouthSprites [memberID.mouthSpriteID];

		// body
		bodySprite.sprite = CrewCreator.Instance.BodySprites[memberID.male ? 0:1];

	}

}

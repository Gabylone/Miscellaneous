﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MemberCreator : MonoBehaviour {

	public static MemberCreator Instance;

	[SerializeField]
	private GameObject overall;

	[SerializeField]
	InputField captainName;

	[SerializeField]
	InputField boatName;

	public enum ButtonType {
		Gender,
		HairSprite,
		HairColor,
		BeardSprite,
		EyesSprite,
		EyebrowsSprite,
		NoseSprite,
		MouthSprite,
	}

	[SerializeField]
	private Button[] previousButtons;
	[SerializeField]
	private Button[] nextButtons;

	void Awake () {
		Instance = this;
	}

	public string[] boatNames;
	public string[] captainNames;

	public void Show ()
	{
		Crews.playerCrew.captain.Icon.MoveToPoint (Crews.PlacingType.Discussion);

		overall.SetActive (true);

		UpdateButtons ();

		int ID = Random.Range ( 0, boatNames.Length );

		Boats.Instance.PlayerBoatInfo.Name = captainNames[ID];
		boatName.text = captainNames[ID];

		Crews.playerCrew.captain.MemberID.Name = boatNames [ID];
		captainName.text = Crews.playerCrew.captain.MemberID.Name;
	}

	public void Confirm () {

		overall.SetActive (false);

		StoryLauncher.Instance.PlayStory (MapData.Instance.currentChunk.IslandData.storyManager, StoryLauncher.StorySource.island);
		SoundManager.Instance.PlaySound (SoundManager.Sound.Select_Big);
	}

	public void SwitchPreviousPart ( int i ) {
		
		switch ((ButtonType)i) {
		case ButtonType.Gender:
			Crews.playerCrew.captain.MemberID.Male = false;
			Crews.playerCrew.captain.MemberID.HairSpriteID = 0;
			Crews.playerCrew.captain.MemberID.BeardSpriteID = -1;
			break;
		case ButtonType.HairSprite:
			Crews.playerCrew.captain.MemberID.HairSpriteID--;
			break;
		case ButtonType.HairColor:
			Crews.playerCrew.captain.MemberID.HairColorID--;
			break;
		case ButtonType.BeardSprite:
			Crews.playerCrew.captain.MemberID.BeardSpriteID--;
			break;
		case ButtonType.EyesSprite:
			Crews.playerCrew.captain.MemberID.EyeSpriteID--;
			break;
		case ButtonType.EyebrowsSprite:
			Crews.playerCrew.captain.MemberID.EyebrowsSpriteID--;
			break;
		case ButtonType.NoseSprite:
			Crews.playerCrew.captain.MemberID.NoseSpriteID--;
			break;
		case ButtonType.MouthSprite:
			Crews.playerCrew.captain.MemberID.MouthSpriteID--;
			break;
		}

		Crews.playerCrew.captain.Icon.UpdateVisual (Crews.playerCrew.captain.MemberID);
		UpdateButtons ();
	}

	public void SwitchNextPart ( int i ) {
		switch ((ButtonType)i) {
		case ButtonType.Gender:
			Crews.playerCrew.captain.MemberID.Male = true;
			Crews.playerCrew.captain.MemberID.HairSpriteID = 0;
			Crews.playerCrew.captain.MemberID.BeardSpriteID = 0;
			break;
		case ButtonType.HairSprite:
			Crews.playerCrew.captain.MemberID.HairSpriteID++;
			break;
		case ButtonType.HairColor:
			Crews.playerCrew.captain.MemberID.HairColorID++;
			break;
		case ButtonType.BeardSprite:
			Crews.playerCrew.captain.MemberID.BeardSpriteID++;
			break;
		case ButtonType.EyesSprite:
			Crews.playerCrew.captain.MemberID.EyeSpriteID++;
			break;
		case ButtonType.EyebrowsSprite:
			Crews.playerCrew.captain.MemberID.EyebrowsSpriteID++;
			break;
		case ButtonType.NoseSprite:
			Crews.playerCrew.captain.MemberID.NoseSpriteID++;
			break;
		case ButtonType.MouthSprite:
			Crews.playerCrew.captain.MemberID.MouthSpriteID++;
			break;

		}

		Crews.playerCrew.captain.Icon.UpdateVisual (Crews.playerCrew.captain.MemberID);
		UpdateButtons ();
	}


	private void UpdateButtons () {

		previousButtons [(int)ButtonType.Gender].interactable = Crews.playerCrew.captain.MemberID.Male;
		nextButtons [(int)ButtonType.Gender].interactable = !Crews.playerCrew.captain.MemberID.Male;

		if ( Crews.playerCrew.captain.MemberID.Male )
			previousButtons [(int)ButtonType.HairSprite].gameObject.SetActive( Crews.playerCrew.captain.MemberID.HairSpriteID > -1);
		else
			previousButtons [(int)ButtonType.HairSprite].gameObject.SetActive( Crews.playerCrew.captain.MemberID.HairSpriteID > 0);

		nextButtons [(int)ButtonType.HairSprite].gameObject.SetActive(Crews.playerCrew.captain.MemberID.HairSpriteID < (Crews.playerCrew.captain.Male ? CrewCreator.Instance.HairSprites_Male.Length -1 : CrewCreator.Instance.HairSprites_Female.Length -1) );

		previousButtons [(int)ButtonType.HairColor].gameObject.SetActive( Crews.playerCrew.captain.MemberID.HairColorID > 0);

		nextButtons [(int)ButtonType.HairColor].gameObject.SetActive(Crews.playerCrew.captain.MemberID.HairColorID < CrewCreator.Instance.HairColors.Length-1);

		previousButtons [(int)ButtonType.BeardSprite].gameObject.SetActive( Crews.playerCrew.captain.MemberID.BeardSpriteID > -1);

		nextButtons [(int)ButtonType.BeardSprite].gameObject.SetActive(Crews.playerCrew.captain.Male && Crews.playerCrew.captain.MemberID.BeardSpriteID < CrewCreator.Instance.BeardSprites.Length-1);

		previousButtons [(int)ButtonType.EyesSprite].gameObject.SetActive( Crews.playerCrew.captain.MemberID.EyeSpriteID > 0);
		nextButtons [(int)ButtonType.EyesSprite].gameObject.SetActive(Crews.playerCrew.captain.MemberID.EyeSpriteID < CrewCreator.Instance.EyesSprites.Length-1);

		previousButtons [(int)ButtonType.EyebrowsSprite].gameObject.SetActive( Crews.playerCrew.captain.MemberID.EyebrowsSpriteID > 0);
		nextButtons [(int)ButtonType.EyebrowsSprite].gameObject.SetActive(Crews.playerCrew.captain.MemberID.EyebrowsSpriteID < CrewCreator.Instance.EyebrowsSprites.Length-1);

		previousButtons [(int)ButtonType.NoseSprite].gameObject.SetActive( Crews.playerCrew.captain.MemberID.NoseSpriteID> 0);
		nextButtons [(int)ButtonType.NoseSprite].gameObject.SetActive(Crews.playerCrew.captain.MemberID.NoseSpriteID < CrewCreator.Instance.NoseSprites.Length-1);

		previousButtons [(int)ButtonType.MouthSprite].gameObject.SetActive( Crews.playerCrew.captain.MemberID.MouthSpriteID > 0);
		nextButtons [(int)ButtonType.MouthSprite].gameObject.SetActive(Crews.playerCrew.captain.MemberID.MouthSpriteID < CrewCreator.Instance.MouthSprites.Length-1);

		SoundManager.Instance.PlaySound (SoundManager.Sound.Select_Small);

	}

	public void ChangeBoatName () {

		Boats.Instance.PlayerBoatInfo.Name = boatName.text;
		SoundManager.Instance.PlaySound (SoundManager.Sound.Select_Big);
	}

	public void ChangeCaptainName () {

		Crews.playerCrew.captain.MemberID.Name = captainName.text;
		SoundManager.Instance.PlaySound (SoundManager.Sound.Select_Big);
	}
}

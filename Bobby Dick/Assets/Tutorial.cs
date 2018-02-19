﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Holoville.HOTween;

public class Tutorial : MonoBehaviour {

	public TextAsset tutoData;

	public int tutoCompletion = 0;

	public bool displayTutorial = false;

	public GameObject group;

	public RectTransform rectTransform;

	public Text titleText;
	public Text descriptionText;

	public delegate void OnDisplayTutorial ( TutoStep tutoStep);
	public static OnDisplayTutorial onDisplayTutorial;

	public delegate void OnHideTutorial ();
	public static OnHideTutorial onHideTutorial;

	public delegate void OnWaitForConfirm ();
	public static OnWaitForConfirm onWaitForConfirm;

	private TutoStep[] tutoSteps;

	public GameObject confirmGroup;

	void Start () {

		if (displayTutorial) {

			onDisplayTutorial += HandleOnDisplayTutorial;
			onHideTutorial += HandleOnHideTutorial;
			onWaitForConfirm += HandleOnWaitForConfirm;

			InitTutorials ();
			LoadData ();
		}

		Hide ();
	}

	void HandleOnWaitForConfirm ()
	{
		confirmGroup.SetActive (true);
	}

	void HandleOnHideTutorial ()
	{
		Fade ();
	}

	void LoadData ()
	{
		string[] rows = tutoData.text.Split ('\n');

		int tutoIndex = 0;

		for (int i = 0; i < rows.Length-1; i++) {
			
			string[] cells = rows[i].Split (';');

			tutoSteps [tutoIndex].title = cells [0];
			tutoSteps [tutoIndex].description = cells [1];

			++tutoIndex;
		}
	}

	void HandleOnDisplayTutorial (TutoStep tutoStep)
	{
		Show ();

		titleText.text = tutoStep.title;
		descriptionText.text = NameGeneration.CheckForKeyWords(tutoStep.description);

		if (tutoStep.targetPosition != Vector2.zero) {

			rectTransform.localPosition = Vector3.zero;
			HOTween.To (rectTransform, 1f, "position", (Vector3)tutoStep.targetPosition);

		} else {
			HOTween.To (rectTransform, 1f, "localPosition", Vector3.zero);

		}

		LayoutRebuilder.ForceRebuildLayoutImmediate (rectTransform);

		FitInScreen ();

	}

	void Fade() {
		Tween.Bounce (group.transform);
		Tween.Fade (group.transform, Tween.defaultDuration);

		Invoke ("Hide",Tween.defaultDuration);
	}

	void FitInScreen ()
	{
		print ("height : " + rectTransform.rect.height);
		print ("size delta y : " + rectTransform.sizeDelta.y);

		if ( rectTransform.anchoredPosition.y < 0) {
			print ("sort par le haut");
		}

		if ( rectTransform.anchoredPosition.y < 0) {
			print ("sort par le bas");
		}
	}

	void InitTutorials ()
	{
		int stepCount = System.Enum.GetValues (typeof(TutorialStep)).Length;

		tutoSteps = new TutoStep[stepCount];

		for (int i = 0; i < stepCount; i++) {

			string classRef = "TutoStep_" + (TutorialStep)i;

			Type tutoClass = Type.GetType (classRef);

			TutoStep newTutoStep = System.Activator.CreateInstance (tutoClass) as TutoStep;

			newTutoStep.Init ();

			tutoSteps [i] = newTutoStep;

		}
	}

	void Show () {
		Tween.ClearFade (group.transform);
		group.SetActive (true);

		Tween.Bounce (group.transform);
	}

	void Hide () {
		group.SetActive (false);
		confirmGroup.SetActive (false);
	}

	public void Confirm () {
		Fade ();
		confirmGroup.SetActive (false);
	}

}

public enum TutorialStep {

	Islands,
	Saves,
	Movements,
	Treasure,
	Clues,
	GoodKarma,
	BadKarma,
	Crew,
	CrewMenu,
	Inventory,
	Night,
	Rain,
	Status,
	NewMember,
	Skills,
	Quests,
	BoatGestion,
	OtherBoats,
	Hunger,
	Food,
	LevelUp,
	Weapon,
	CharacterCreation

}

public class TutoStep {

	public Vector2 targetPosition = Vector2.zero;

	public string title = "";

	public string description = "";

	public void Display () {
		//
		Tutorial.onDisplayTutorial ( this );
	}

	public virtual void Init () {

	}

	public virtual void Kill () {
		Tutorial.onHideTutorial ();
	}

	public void WaitForConfirm () {
		Tutorial.onWaitForConfirm ();
	}
}

public class TutoStep_Islands : TutoStep {

	public override void Init ()
	{
		base.Init ();

		StoryLauncher.Instance.endStoryEvent += HandleEndStoryEvent;
	}

	void HandleEndStoryEvent ()
	{
		targetPosition = Island.Instance.transform.position;

		Display ();

		StoryLauncher.Instance.endStoryEvent -= HandleEndStoryEvent;

		StoryLauncher.Instance.playStoryEvent += HandlePlayStoryEvent;
	}

	void HandlePlayStoryEvent ()
	{
		Kill ();
	}

	public override void Kill ()
	{
		base.Kill ();

		StoryLauncher.Instance.playStoryEvent -= HandlePlayStoryEvent;
	}

}

public class TutoStep_Saves : TutoStep {

	int chunkCount = 0;

	public override void Init ()
	{
		base.Init ();

		NavigationManager.Instance.EnterNewChunk += HandleChunkEvent;
	}

	void HandleChunkEvent ()
	{
		++chunkCount;

		if ( chunkCount == 8 ) {

			Display ();
			NavigationManager.Instance.EnterNewChunk -= HandleChunkEvent;
			WaitForConfirm ();
		}
	}
}

public class TutoStep_Movements: TutoStep {

	int count = 0;

	public override void Init ()
	{
		base.Init ();

		StoryLauncher.Instance.endStoryEvent += HandleEndStoryEvent;
	}

	void HandleEndStoryEvent ()
	{
		count++;

		if (count > 0) {

			targetPosition = PlayerBoat.Instance.getTransform.position;

			Display ();

			StoryLauncher.Instance.endStoryEvent -= HandleEndStoryEvent;

			NavigationManager.Instance.EnterNewChunk += HandleChunkEvent;
		}
	}

	void HandleChunkEvent ()
	{
		Kill ();
	}

	public override void Kill ()
	{
		base.Kill ();

		NavigationManager.Instance.EnterNewChunk -= HandleChunkEvent;
	}

}
public class TutoStep_Treasure: TutoStep {

	int chunkCount = 0;

	public override void Init ()
	{
		base.Init ();

		NavigationManager.Instance.EnterNewChunk += HandleChunkEvent;
	}

	void HandleChunkEvent ()
	{
		++chunkCount;

		if ( chunkCount == 6 ) {

			Display ();
			NavigationManager.Instance.EnterNewChunk -= HandleChunkEvent;
			WaitForConfirm ();
		}
	}

}
public class TutoStep_Clues: TutoStep {

	public override void Init ()
	{
		base.Init ();

		NameGeneration.onDiscoverFormula += HandleOnDiscoverFormula;
	}

	void HandleOnDiscoverFormula (Formula Formula)
	{
		Display ();
		NameGeneration.onDiscoverFormula -= HandleOnDiscoverFormula;
		WaitForConfirm ();
	}
}
public class TutoStep_GoodKarma: TutoStep {

	public override void Init ()
	{
		base.Init ();

		Karma.onChangeKarma += HandleOnChangeKarma;
	}

	void HandleOnChangeKarma (int previousKarma, int newKarma)
	{
		if ( newKarma > previousKarma ) {
			Display ();
			Karma.onChangeKarma -= HandleOnChangeKarma;
			WaitForConfirm ();
		}
	}

}
public class TutoStep_BadKarma: TutoStep {

	public override void Init ()
	{
		base.Init ();

		Karma.onChangeKarma += HandleOnChangeKarma;
	}

	void HandleOnChangeKarma (int previousKarma, int newKarma)
	{
		if ( newKarma < previousKarma ) {
			Display ();
			Karma.onChangeKarma -= HandleOnChangeKarma;
			WaitForConfirm ();
		}
	}

}
public class TutoStep_Crew: TutoStep {

	int chunkCount = 0;

	public override void Init ()
	{
		base.Init ();

		NavigationManager.Instance.EnterNewChunk += HandleChunkEvent;
	}

	void HandleChunkEvent ()
	{
		++chunkCount;

		if ( chunkCount == 3 ) {

			targetPosition = Crews.playerCrew.mapAnchors [0].position;
			Display ();
			NavigationManager.Instance.EnterNewChunk -= HandleChunkEvent;
			WaitForConfirm ();
		}
	}

}
public class TutoStep_CrewMenu: TutoStep {

	public override void Init ()
	{
		base.Init ();

		CrewInventory.Instance.openInventory += HandleOpenInventory;
	}

	void HandleOpenInventory (CrewMember member)
	{
		if (OtherInventory.Instance.type == OtherInventory.Type.None) {
			Display ();
			CrewInventory.Instance.openInventory -= HandleOpenInventory;
			WaitForConfirm ();
		}
	}

}
public class TutoStep_Inventory: TutoStep {

	public override void Init ()
	{
		base.Init ();

		LootUI.onShowLoot += HandleOnShowLoot;
	}
//
	void HandleOnShowLoot ()
	{
		if (LootUI.Instance.currentSide == Crews.Side.Player) {
			Display ();
			LootUI.onShowLoot -= HandleOnShowLoot;
			WaitForConfirm ();
		}
	}
}

public class TutoStep_Night: TutoStep {

	public override void Init ()
	{
		base.Init ();

		TimeManager.onSetTimeOfDay += HandleOnSetTimeOfDay;
	}

	void HandleOnSetTimeOfDay (TimeManager.DayState dayState)
	{
		if (dayState == TimeManager.DayState.Night) {
			Display ();
			TimeManager.onSetTimeOfDay -= HandleOnSetTimeOfDay;
			WaitForConfirm ();
		}
	}

}
public class TutoStep_Rain: TutoStep {

	public override void Init ()
	{
		base.Init ();

		TimeManager.onSetRain += HandleOnSetRain;
	}

	void HandleOnSetRain ()
	{
		Display ();
		TimeManager.onSetRain -= HandleOnSetRain;
		WaitForConfirm ();
	}

}

public class TutoStep_Status: TutoStep {

	public override void Init ()
	{
		base.Init ();

		CombatManager.Instance.onFightStart += HandleFightStarting;
	}

	void HandleFightStarting ()
	{
		CombatManager.Instance.onFightStart -= HandleFightStarting;

		CombatManager.Instance.currPlayerFighters[0].onAddStatus += HandleOnAddStatus;
	}

	void HandleOnAddStatus (Fighter.Status status, int count)
	{
		targetPosition = CombatManager.Instance.currPlayerFighters [0].BodyTransform.position;
		Display ();
		CombatManager.Instance.currPlayerFighters[0].onAddStatus -= HandleOnAddStatus;
		WaitForConfirm ();
	}

}

public class TutoStep_NewMember: TutoStep {

	public override void Init ()
	{
		base.Init ();

		StoryFunctions.Instance.getFunction += HandleGetFunction;
	}

	void HandleGetFunction (FunctionType func, string cellParameters)
	{
		if ( func == FunctionType.AddMember ) {
			targetPosition = Crews.playerCrew.mapAnchors [0].position;
			StoryFunctions.Instance.getFunction -= HandleGetFunction;
			Display ();
			WaitForConfirm ();
		}
	}

}
public class TutoStep_Skills: TutoStep {

	public override void Init ()
	{
		base.Init ();

		CombatManager.Instance.onChangeState += HandleOnChangeState;
	}

	void HandleOnChangeState (CombatManager.States currState, CombatManager.States prevState)
	{
		if (currState == CombatManager.States.PlayerActionChoice) {
			targetPosition = Crews.playerCrew.mapAnchors [0].position;
			CombatManager.Instance.onChangeState -= HandleOnChangeState;
			Display ();
			WaitForConfirm ();
		}
	}

}
public class TutoStep_Quests: TutoStep {

	public override void Init ()
	{
		base.Init ();

		StoryFunctions.Instance.getFunction += HandleGetFunction;
	}

	void HandleGetFunction (FunctionType func, string cellParameters)
	{
		if ( func == FunctionType.AddCurrentQuest ) {
			StoryFunctions.Instance.getFunction -= HandleGetFunction;
			Display ();
			WaitForConfirm ();
		}
	}

}
public class TutoStep_BoatGestion: TutoStep {

	public override void Init ()
	{
		base.Init ();

		BoatUpgradeManager.onOpenBoatUpgrade += HandleOnOpenBoatUpgrade;
	}

	void HandleOnOpenBoatUpgrade ()
	{
		BoatUpgradeManager.onOpenBoatUpgrade -= HandleOnOpenBoatUpgrade;
		Display ();
		WaitForConfirm ();
	}

}
public class TutoStep_OtherBoats: TutoStep {
//
	public override void Init ()
	{
		base.Init ();

		StoryLauncher.Instance.playStoryEvent += HandlePlayStoryEvent;
	}

	void HandlePlayStoryEvent ()
	{

		if (StoryLauncher.Instance.CurrentStorySource == StoryLauncher.StorySource.boat) {

			StoryLauncher.Instance.playStoryEvent -= HandlePlayStoryEvent;
			Display ();
			WaitForConfirm ();

		}
	}

}
public class TutoStep_Hunger: TutoStep {

	public override void Init ()
	{
		base.Init ();

		NavigationManager.Instance.EnterNewChunk += HandleChunkEvent;
	}

	void HandleChunkEvent ()
	{

		CrewMember member = Crews.playerCrew.captain;

		float fillAmount = 1f - ((float)member.CurrentHunger / (float)member.maxHunger);

		if (fillAmount < 0.45f) {
			NavigationManager.Instance.EnterNewChunk -= HandleChunkEvent;
			Display ();
			WaitForConfirm ();
		}
	}

}
public class TutoStep_Food: TutoStep {

	public override void Init ()
	{
		base.Init ();

		LootUI.useInventory += HandleUseInventory;
	}

	void HandleUseInventory (InventoryActionType actionType)
	{
		if (actionType == InventoryActionType.Eat) {
			
			LootUI.useInventory -= HandleUseInventory;

			Display ();

			WaitForConfirm ();

		}
	}

}
public class TutoStep_LevelUp: TutoStep {

	public override void Init ()
	{
		base.Init ();

		Crews.playerCrew.captain.onLevelUp += HandleOnLevelUp;

	}

	void HandleOnLevelUp (CrewMember member)
	{
		Crews.playerCrew.captain.onLevelUp -= HandleOnLevelUp;

		Display ();

		WaitForConfirm ();
	}

}
public class TutoStep_Weapon: TutoStep {

	public override void Init ()
	{
		base.Init ();

		LootUI.useInventory += HandleUseInventory;

	}

	void HandleUseInventory (InventoryActionType actionType)
	{
		if ( actionType == InventoryActionType.Buy || actionType == InventoryActionType.PickUp ) {

			if (LootUI.Instance.SelectedItem.category == ItemCategory.Weapon) {

				LootUI.useInventory -= HandleUseInventory;

				Display ();

				WaitForConfirm ();
			}
		}
	}
}
public class TutoStep_CharacterCreation: TutoStep {

	public override void Init ()
	{
		base.Init ();

		Display ();

		WaitForConfirm ();

	}

}

﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Holoville.HOTween;

public class Tutorial : MonoBehaviour {

	public TextAsset tutoData;

	public bool debugTutorial = true;

	public delegate void OnDisplayTutorial ( TutoStep tutoStep);
	public static OnDisplayTutorial onDisplayTutorial;

	public delegate void OnHideTutorial ();
	public static OnHideTutorial onHideTutorial;

	public delegate void OnWaitForConfirm ();
	public static OnWaitForConfirm onWaitForConfirm;

	private TutoStep[] tutoSteps;

	void Start () {

		if (debugTutorial) {
			KeepOnLoad.displayTuto = true;
		}

		if (KeepOnLoad.displayTuto) {

			InitTutorials ();
			LoadData ();

			Invoke ("CharacterCreationDelay", 1.5f);

		}
	}

	void CharacterCreationDelay () {
		tutoSteps [(int)TutorialStep.CharacterCreation].Display ();
		tutoSteps [(int)TutorialStep.CharacterCreation].WaitForConfirm ();
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

	void InitTutorials ()
	{
		int stepCount = System.Enum.GetValues (typeof(TutorialStep)).Length;

		tutoSteps = new TutoStep[stepCount];

		for (int i = 0; i < stepCount; i++) {

			string classRef = "TutoStep_" + (TutorialStep)i;

			Type tutoClass = Type.GetType (classRef);

			TutoStep newTutoStep = System.Activator.CreateInstance (tutoClass) as TutoStep;

			newTutoStep.Init ();
			newTutoStep.step = (TutorialStep)i;

			tutoSteps [i] = newTutoStep;

		}

		LoadData ();

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
	Skills2,
	Quests,
	QuestMenu,
	BoatGestion,
	OtherBoats,
	Hunger,
	Food,
	LevelUp,
	SkillMenu,
	Weapon,
	CharacterCreation,
	ItemMenu1,
	ItemMenu2,
	Minimap,
	SkillInfo,
	StatusInfo

}

public class TutoStep {

	public TutorialStep step;

	public DisplayInfo.Corner corner = DisplayInfo.Corner.None;

	public string title = "";

	public string description = "";

	public void Display () {
		//
		Tutorial.onDisplayTutorial ( this );
	}

	public virtual void Init () {

	}

	public virtual void Kill () {
		if ( Tutorial.onHideTutorial != null )
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
		Display ();

		StoryLauncher.Instance.endStoryEvent -= HandleEndStoryEvent;

		StoryLauncher.Instance.onStartStory += HandlePlayStoryEvent;
	}

	void HandlePlayStoryEvent ()
	{
		Kill ();
	}

	public override void Kill ()
	{
		base.Kill ();

		StoryLauncher.Instance.onStartStory -= HandlePlayStoryEvent;
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

		if (count > 0) {

			Display ();

			StoryLauncher.Instance.endStoryEvent -= HandleEndStoryEvent;

			NavigationManager.Instance.EnterNewChunk += HandleChunkEvent;
		}
		count++;

	}

	void HandleChunkEvent ()
	{
		NavigationManager.Instance.EnterNewChunk -= HandleChunkEvent;

		Kill ();
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
			corner = DisplayInfo.Corner.BottomLeft;
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
			corner = DisplayInfo.Corner.BottomLeft;
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
			corner = DisplayInfo.Corner.TopLeft;

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
			corner = DisplayInfo.Corner.TopLeft;
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
		corner = DisplayInfo.Corner.TopLeft;
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

		foreach (var item in CombatManager.Instance.initPlayerFighters) {
			item.onAddStatus += HandleOnAddStatus;
		}
	}

	void HandleOnAddStatus (Fighter.Status status, int count)
	{
		Display ();

		foreach (var item in CombatManager.Instance.initPlayerFighters) {
			item.onAddStatus -= HandleOnAddStatus;
		}

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
			corner = DisplayInfo.Corner.TopLeft;
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
			corner = DisplayInfo.Corner.TopLeft;
			CombatManager.Instance.onChangeState -= HandleOnChangeState;
			Display ();
			WaitForConfirm ();
		}
	}

}

public class TutoStep_Skills2: TutoStep {

	public override void Init ()
	{
		base.Init ();

		Tutorial.onDisplayTutorial += HandleOnDisplayTutorial;
	}

	void HandleOnDisplayTutorial (TutoStep tutoStep)
	{
		if (tutoStep.step == TutorialStep.Skills) {
			Tutorial.onDisplayTutorial -= HandleOnDisplayTutorial;
			Tutorial.onHideTutorial += HandleOnHideTutorial;
		}
	}

	void HandleOnHideTutorial ()
	{
		Tutorial.onHideTutorial -= HandleOnHideTutorial;

		corner = DisplayInfo.Corner.TopLeft;
		Display ();
		WaitForConfirm ();
	}

}

public class TutoStep_Quests: TutoStep {

	bool aquieredQuest = false;

	public override void Init ()
	{
		base.Init ();

		StoryFunctions.Instance.getFunction += HandleGetFunction;

		StoryLauncher.Instance.endStoryEvent += HandleEndStoryEvent;
	}

	void HandleEndStoryEvent ()
	{
		if (aquieredQuest) {
			StoryLauncher.Instance.endStoryEvent -= HandleEndStoryEvent;
			Display ();
			WaitForConfirm ();
		}
	}

	void HandleGetFunction (FunctionType func, string cellParameters)
	{
		if ( func == FunctionType.AddCurrentQuest ) {
			StoryFunctions.Instance.getFunction -= HandleGetFunction;
			aquieredQuest = true;
		}
	}

}

public class TutoStep_QuestMenu: TutoStep {

	public override void Init ()
	{
		base.Init ();

		QuestMenu.onOpenQuestMenu += HandleOnOpenQuestMenu;
	}

	void HandleOnOpenQuestMenu ()
	{
		Display ();
		WaitForConfirm ();

		QuestMenu.onOpenQuestMenu -= HandleOnOpenQuestMenu;

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

		StoryLauncher.Instance.onStartStory += HandlePlayStoryEvent;
	}

	void HandlePlayStoryEvent ()
	{

		if (StoryLauncher.Instance.CurrentStorySource == StoryLauncher.StorySource.boat) {

			StoryLauncher.Instance.onStartStory -= HandlePlayStoryEvent;
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

		float fillAmount = 1f - ((float)member.CurrentHunger / (float)Crews.maxHunger);

		if (fillAmount < 0.45f) {
			corner = DisplayInfo.Corner.TopLeft;
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

public class TutoStep_SkillMenu : TutoStep {

	public override void Init ()
	{
		base.Init ();

		CrewInventory.onShowCharacterStats += HandleOnShowCharacterStats;

	}

	void HandleOnShowCharacterStats ()
	{
		CrewInventory.onShowCharacterStats -= HandleOnShowCharacterStats;
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

	}

}

public class TutoStep_ItemMenu1: TutoStep {

	public override void Init ()
	{
		base.Init ();

		LootUI.onSetSelectedItem += HandleOnSetSelectedItem;
	}

	void HandleOnSetSelectedItem ()
	{
		corner = DisplayInfo.Corner.TopRight;
		LootUI.onSetSelectedItem -= HandleOnSetSelectedItem;

		Display ();
		WaitForConfirm ();
	}

}

public class TutoStep_ItemMenu2: TutoStep {

	public override void Init ()
	{
		base.Init ();

		Tutorial.onDisplayTutorial += HandleOnDisplayTutorial;
	}

	void HandleOnDisplayTutorial (TutoStep tutoStep)
	{
		if (tutoStep.step == TutorialStep.ItemMenu1) {
			corner = DisplayInfo.Corner.TopRight;
			Tutorial.onDisplayTutorial -= HandleOnDisplayTutorial;
			Tutorial.onHideTutorial += HandleOnHideTutorial;
		}
	}

	void HandleOnHideTutorial ()
	{
		Tutorial.onHideTutorial -= HandleOnHideTutorial;

		Display ();
		WaitForConfirm ();
	}

}

public class TutoStep_Minimap : TutoStep {

	int chunkCount = 0;

	public override void Init ()
	{
		base.Init ();

		NavigationManager.Instance.EnterNewChunk += HandleChunkEvent;
	}

	void HandleChunkEvent ()
	{
		++chunkCount;

		if ( chunkCount == 4 ) {

			corner = DisplayInfo.Corner.BottomLeft;

			Display ();
			NavigationManager.Instance.EnterNewChunk -= HandleChunkEvent;
			WaitForConfirm ();
		}
	}
}

public class TutoStep_SkillInfo: TutoStep {

	public override void Init ()
	{
		base.Init ();

		Tutorial.onDisplayTutorial += HandleOnDisplayTutorial;
	}

	void HandleOnDisplayTutorial (TutoStep tutoStep)
	{
		if (tutoStep.step == TutorialStep.Skills2) {
			Tutorial.onDisplayTutorial -= HandleOnDisplayTutorial;
			Tutorial.onHideTutorial += HandleOnHideTutorial;
		}
	}

	void HandleOnHideTutorial ()
	{
		Tutorial.onHideTutorial -= HandleOnHideTutorial;

		corner = DisplayInfo.Corner.TopLeft;
		Display ();
		WaitForConfirm ();
	}

}

public class TutoStep_StatusInfo: TutoStep {

	public override void Init ()
	{
		base.Init ();

		Tutorial.onDisplayTutorial += HandleOnDisplayTutorial;
	}

	void HandleOnDisplayTutorial (TutoStep tutoStep)
	{
		if (tutoStep.step == TutorialStep.Status) {
			Tutorial.onDisplayTutorial -= HandleOnDisplayTutorial;
			Tutorial.onHideTutorial += HandleOnHideTutorial;
		}
	}

	void HandleOnHideTutorial ()
	{
		Tutorial.onHideTutorial -= HandleOnHideTutorial;

		Display ();
		WaitForConfirm ();
	}

}


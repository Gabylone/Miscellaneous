﻿using UnityEngine;
using System.Collections;

[System.Serializable]
public class CrewMember {

	private Crews.Side side;
	private MemberID memberID;

	public int id = 0;

	public int maxStat = 6;

	public CrewMember (MemberID _memberID, Crews.Side _side, GameObject _iconObj )
	{
		memberID = _memberID;

		iconObj = _iconObj;

		side = _side;

		Init ();
	}

		// level
	private int xpToLevelUp = 100;

	private int daysOnBoard = 0;

	private Item[] equipment = new Item[3];

	private CrewIcon icon;
	private MemberFeedback info;
	private GameObject iconObj;

	private int currentCold = 0;
	private int stepsToCold = 4;

	private int currentHunger = 0;

	private int stepsToHunger = 10;
	private int hungerDamage = 5;

	private int maxState = 100;

	private void Init () {

		// icon
		icon = iconObj.GetComponent<CrewIcon> ();
		icon.Member = this;

//		// side
//		if (side == Crews.Side.Enemy)
//			icon.Overable = false;

		// equipment
		SetEquipment (EquipmentPart.Weapon, 	ItemLoader.Instance.getItem (ItemCategory.Weapon, memberID.WeaponID));
		SetEquipment (EquipmentPart.Clothes, 	ItemLoader.Instance.getItem (ItemCategory.Clothes, memberID.ClothesID));

		// set state delegate

	}

	#region health
	public void GetHit (float damage) {
//		float damageTaken = ( ((float)damage) / ((float)Defense) );
//		damageTaken *= 10;
		float damageTaken = damage;

		damageTaken = Mathf.CeilToInt (damageTaken);
		damageTaken = Mathf.Clamp ( damageTaken , 1 , 200 );

		string smallText = damage + " / " + Defense;
		string bigText = damageTaken.ToString ();

		Health -= (int)damageTaken;

	}

	public void Kill () {
		Crews.getCrew(side).RemoveMember (this);
	}
	#endregion

	#region level
	public void AddXP ( int _xp ) {
		
		CurrentXp += _xp;

		if ( CurrentXp >= xpToLevelUp ) {
			LevelUp ();
		}
	}
	public void LevelUp () {
		++Level;
		CurrentXp = xpToLevelUp - CurrentXp;

		++StatPoints;
	}
	public bool CheckLevel ( int lvl ) {

		if (lvl > Level) {

//			PlayerLoot.Instance.inven

			DialogueManager.Instance.SetDialogueTimed ("Je sais pas porter ça moi...", this);

			return false;
		}

		return true;

	}
	#endregion

	#region states
	public void AddToStates () {

		AddXP (3);

		CurrentHunger += StepsToHunger;

		if ( CurrentHunger >= maxState ) {

			DialogueManager.Instance.SetDialogueTimed ("J'ai faim !", this);

			Health -= hungerDamage;

			if ( Health == 0 )
			{
				DialogueManager.Instance.ShowNarratorTimed (" Après " + daysOnBoard + " jours à bord, " + MemberName + " est mort d'une faim atroce");
				Kill ();
				return;
			}
		}

		++daysOnBoard;

		Icon.UpdateHungerIcon ();

	}
	#endregion

	#region parameters
	public int Health {
		get {
			return memberID.health;
		}
		set {
			memberID.health = Mathf.Clamp (value , 0 , memberID.maxHealth);

			if (memberID.health <= 0)
				Kill ();


		}
	}

	public string MemberName {
		get {
			return memberID.Name;
		}
	}


	public int Level {
		get {
			return memberID.Lvl;
		}
		set {
			memberID.Lvl = value;
		}
	}

	public bool Male {
		get { return memberID.Male; }
	}
	#endregion

	#region stats
	private int currentAttack = 0;

	public int CurrentAttack {
		get {
			return currentAttack;
		}
		set {
			currentAttack = value;
		}
	}

	public int Attack {
		get {

			int i = Strenght * 5;

			if (GetEquipment (EquipmentPart.Weapon) != null)
				return i + GetEquipment (EquipmentPart.Weapon).value;

			return i;
		}
	}

	public int Defense {
		get {

			int i = Constitution * 5;

			if (GetEquipment (EquipmentPart.Clothes) != null)
				return i + GetEquipment (EquipmentPart.Clothes).value;

			return i;
		}
	}

	public int Strenght {
		get {
			return memberID.Str;
		}
		set {
			memberID.Str = value;
		}
	}

	public int Dexterity {
		get {
			return memberID.Dex;
		}
		set {
			memberID.Dex = value;
		}
	}

	public int Charisma {
		get {
			return memberID.Cha;
		}
		set {
			memberID.Cha = value;
		}
	}

	public int Constitution {
		get {
			return memberID.Con;
		}
		set {
			memberID.Con = value;
		}
	}
	#endregion

	#region icon
	public GameObject IconObj {
		get {
			return iconObj;
		}
		set {
			iconObj = value;
		}
	}

	public Vector3 IconPos {
		get {
			return iconObj.transform.position;
		}
	}

	public CrewIcon Icon {
		get {
			return icon;
		}
		set {
			icon = value;
		}
	}
	public int GetIndex {
		get {
			return Crews.getCrew (side).CrewMembers.FindIndex (x => x == this);
		}
	}

	public Crews.Side Side {
		get {
			return side;
		}
		set {
			side = value;
		}
	}
	#endregion

	#region properties
	public MemberID MemberID {
		get {
			return memberID;
		}
	}

	public MemberFeedback Info {
		get {
			return info;
		}
	}
	#endregion

	#region equipment
	public enum EquipmentPart {
		Weapon,
		Clothes,
	}
	public void SetRandomEquipment () {

		const int l = 2;

		ItemCategory[] equipmentCategories = new ItemCategory[l] {
			ItemCategory.Weapon,
			ItemCategory.Clothes
		};

		for (int i = 0; i < l; ++i) {

			Item equipmentItem = ItemLoader.Instance.getRandomItem (equipmentCategories [i]);
			SetEquipment ((EquipmentPart)i, equipmentItem);
		}

	}
	public void SetEquipment ( EquipmentPart part , Item item ) {
		equipment [(int)part] = item;
	}
	public Item GetEquipment ( EquipmentPart part ) {
		return equipment [(int)part];
	}

	public Item[] Equipment {
		get {
			return equipment;
		}
		set {
			equipment = value;
		}
	}
	#endregion

	#region states properties
	public int StepsToHunger {
		get {
			return stepsToHunger;
		}
		set {
			stepsToHunger = value;
		}
	}

	public int CurrentHunger {
		get {
			return currentHunger;
		}
		set {
			currentHunger = Mathf.Clamp (value, 0, maxState);
		}
	}

	public int StepsToCold {
		get {
			return stepsToCold;
		}
		set {
			stepsToCold = value;
		}
	}

	public int CurrentCold {
		get {
			return currentCold;
		}
		set {
			currentCold = Mathf.Clamp (value, 0, maxState);
		}
	}

	public int MaxState {
		get {
			return maxState;
		}
		set {
			maxState = value;
		}
	}
	#endregion

	#region level
	public int CurrentXp {
		get {
			return memberID.xp;
		}
		set {
			memberID.xp = value;
		}
	}

	public int XpToLevelUp {
		get {
			return xpToLevelUp;
		}
	}

	public int StatPoints {
		get {
			return memberID.statPoints;
		}
		set {
			memberID.statPoints = value;
		}
	}
	#endregion
}
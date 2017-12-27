﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Holoville.HOTween;

public class Card : MonoBehaviour {

	public static Card previouslySelectedCard;

		// components
	private Transform _transform;

	[Header("UI Elements")]
	[SerializeField]
	private GameObject cardObject;

	[SerializeField]
	private Image targetFeedbackImage;

	[SerializeField]
	private Text nameText;

	[SerializeField]
	private RectTransform heartBackground;
	[SerializeField]
	private Image heartImage;

	[SerializeField]
	private GameObject energyGroup;
	[SerializeField]
	private GameObject[] energyPoints;

	[SerializeField]
	private GameObject heartGroup;

	[SerializeField]
	private Text defenceText;

	[SerializeField]
	private Text attackText;

	[SerializeField]
	private Text levelText;

	[SerializeField]
	private GameObject statGroup;

	public Fighter linkedFighter;

	public Image jobImage;

	float maxWidth = 0f;

	bool playingTurn = false;

//	void Awake () {
	public void Init() {

//		print ("oui?");

		linkedFighter.onInit += HandleOnInit;
		linkedFighter.onSelect += HandleOnSelect;
		linkedFighter.onSetTurn += HandleOnSetTurn;
		linkedFighter.onEndTurn += HandleOnEndTurn;

		linkedFighter.onShowInfo += HandleOnShowInfo;
		linkedFighter.onGetHit += HandleOnGetHit;

		linkedFighter.onChangeState += HandleOnChangeState;

		linkedFighter.onSetPickable += HandleOnSetPickable;;

//		if (linkedFighter.crewMember != null)
//			UpdateMember ();

		LootUI.useInventory+= HandleUseInventory;

		maxWidth = heartImage.rectTransform.rect.width;

		HideTargetFeedback ();

		energyGroup.SetActive (false);


	}

	void HandleOnSetPickable (bool pickable)
	{
		if (pickable) {
			ShowTargetFeedback (Color.yellow);
		} else {

			if ( playingTurn ) {
				ShowTargetFeedback (Color.magenta);
				//
			} else {
				HideTargetFeedback ();
				//
			}

		}
	}

	void ShowTargetFeedback(Color color) {
		targetFeedbackImage.color = color;

		targetFeedbackImage.gameObject.SetActive (true);
		Tween.Bounce (targetFeedbackImage.transform);
	}

	void HideTargetFeedback () {
		targetFeedbackImage.gameObject.SetActive (false);
//		print ("dOUDOUDOUDOUODU?====");
		//
	}

	void HandleOnChangeState (Fighter.states currState, Fighter.states prevState)
	{
		if (currState == Fighter.states.triggerSkill) {
			UpdateEnergyBar (linkedFighter.crewMember);

			Tween.Bounce (energyGroup.transform);
		}
	}


	void HandleOnEndTurn ()
	{
		Tween.Scale (transform, 0.2f, 1f);

		HideTargetFeedback ();

		energyGroup.SetActive (false);

		playingTurn = false;
	}

	void HandleOnInit () {

		UpdateMember (linkedFighter.crewMember);
	}

	void HandleOnGetHit ()
	{
		UpdateMember ();
	}

	void HandleOnShowInfo ()
	{
		if (previouslySelectedCard == this) {
			previouslySelectedCard = null;
			HideInfo ();
			return;
		}

		if (previouslySelectedCard != null)
			previouslySelectedCard.HideInfo ();

		previouslySelectedCard = this;

		statGroup.SetActive (true);
		energyGroup.SetActive (true);

		Tween.Bounce (transform);
	}

	public delegate void OnHideInfo ();
	public OnHideInfo onHideInfo;
	public void HideInfo ()
	{
		statGroup.SetActive (false);
		energyGroup.SetActive (false);


		if (onHideInfo != null)
			onHideInfo ();
	}

	void HandleOnSelect ()
	{
		UpdateMember ();
	}

	void HandleOnSetTurn ()
	{

		playingTurn = true;

		UpdateMember ();

		ShowTargetFeedback (Color.magenta);

		energyGroup.SetActive (true);

		Tween.Scale (transform, 0.2f, 1.15f);
	}

	void HandleUseInventory (InventoryActionType actionType)
	{
		UpdateMember (CrewMember.selectedMember);
	}

	void UpdateMember() {
		UpdateMember (linkedFighter.crewMember);
	}

	public virtual void UpdateMember ( CrewMember member ) {

		nameText.text = member.MemberName;

		levelText.text = member.Level.ToString ();
		if (member.Level == member.maxLevel)
			levelText.text = "MAX";

		maxWidth = heartBackground.sizeDelta.x;

		float health_Width = maxWidth * (float)member.Health / (float)member.MemberID.maxHealth;
		heartImage.rectTransform.sizeDelta = new Vector2 ( health_Width , heartImage.rectTransform.sizeDelta.y);

		attackText.text = member.Attack.ToString ();
		defenceText.text = member.Defense.ToString ();

		if (SkillManager.jobSprites.Length <= (int)member.job)
			print ("skill l : " + SkillManager.jobSprites.Length + " / member job " + (int)member.job);
		jobImage.sprite = SkillManager.jobSprites[(int)member.job];

		UpdateEnergyBar (member);

		Tween.Bounce (transform);

	}

	public Color energyColor_Full;
	public Color energyColor_Empty;
	int currentEnergy = 0;
	public Text energyText;
	void UpdateEnergyBar(CrewMember member) {

		float scaleAmount = 0.8f;

		float dur = 0.5f;

		int a = 0;

		energyText.text = "" + member.energy;

//		foreach (var item in energyPoints) {
//			
//			if (a < member.energy) {
//				
//				item.transform.localScale = Vector3.one * scaleAmount;
//
//				HOTween.To ( item.transform , dur , "localScale" , Vector3.one );
//				HOTween.To ( item.GetComponent<Image>() , dur , "color" , energyColor_Full);
//
////				item.SetActive (true);
//			} else {
//
//				HOTween.To ( item.transform , dur , "localScale" , Vector3.one * scaleAmount);
//				HOTween.To ( item.GetComponent<Image>() , dur , "color" , energyColor_Empty);
//
////				item.SetActive (false);
//			}
//			++a;
//		}

//		currentEnergy = member.energy;
	}

	public void ShowStats () {
		statGroup.SetActive (true);
	}

	public void HideStats () {
		statGroup.SetActive (false);
	}

	public void ShowCard () {
		cardObject.SetActive (true);

	}
	public void HideCard () {
		cardObject.SetActive (false);
	}


}

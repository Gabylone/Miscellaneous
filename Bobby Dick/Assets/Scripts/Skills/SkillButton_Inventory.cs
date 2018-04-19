﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillButton_Inventory : SkillButton {

	public GameObject chargeCostObj;

	public GameObject padlockObj;
	public Text padlockText;

	public override void Start ()
	{
		base.Start ();

		onUnlockSkill += HandleOnUnlockSkill;
	}

	void HandleOnUnlockSkill ()
	{
		if (skill != null)
			SetSkill (skill);
	}

	public delegate void OnUnlockSkill ();
	public static OnUnlockSkill onUnlockSkill;
	public void OnPointerDown () {

		Tween.Bounce (skillImage.transform);

		if ( CrewMember.GetSelectedMember.SkillPoints >= GetSkillCost() ) {

			CrewMember.GetSelectedMember.SkillPoints -= GetSkillCost ();

			CrewMember.GetSelectedMember.AddSkill (skill);

			if (onUnlockSkill != null)
				onUnlockSkill ();

			Unlock ();

		} else {
			padlockObj.GetComponent<Animator> ().SetTrigger ("giggle");
			Tween.Bounce (padlockObj.transform);
		}

	}

	void Unlock () {

		button.interactable = false;

		chargeCostObj.SetActive (true);

		Tween.Bounce ( padlockObj.transform );
		Invoke ("HidePadlock", Tween.defaultDuration);

	}

	void HidePadlock () {
		padlockObj.SetActive (false);
	}

	void Lock ()
	{
		button.interactable = true;

		padlockObj.SetActive (true);
		chargeCostObj.SetActive (false);

//		image.color = Color.gray;

		padlockText.text = "" + GetSkillCost ();
	}

	public override void SetSkill (Skill _skill)
	{
		base.SetSkill (_skill);

		if (CrewMember.GetSelectedMember.SpecialSkills.Find (x => x.type == _skill.type) == null) {

			Lock ();

		} else {
			
			Unlock ();

		}
	}

	int GetSkillCost ()
	{
		return (int)(CrewMember.GetSelectedMember.SpecialSkills.Count*1.5f);
	}
}
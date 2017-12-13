﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Wallop : Skill {

	public override void Start ()
	{
		base.Start ();
	}

	public override void TriggerSkill ()
	{

		base.TriggerSkill ();

		foreach (var targetFighter in CombatManager.Instance.getCurrentFighters (Crews.otherSide (fighter.crewMember.side))) {

			targetFighter.GetHit (fighter, fighter.crewMember.Attack / 2f);

		}

		EndSkill ();

	}

	public override bool MeetsConditions (CrewMember member)
	{

		bool moreThanOneMember = CombatManager.Instance.getCurrentFighters (Crews.otherSide (member.side)).Count > 1;

		return moreThanOneMember && base.MeetsConditions (member);
	}
}
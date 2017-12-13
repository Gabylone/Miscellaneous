﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Skill_Cosh : Skill {

	public override void Start ()
	{
		base.Start ();
	}

	public override void TriggerSkill ()
	{
		base.TriggerSkill ();

		fighter.TargetFighter.GetHit (fighter, fighter.crewMember.Attack);

		fighter.TargetFighter.AddStatus (Fighter.Status.KnockedOut);

		EndSkill ();

	}

	public override bool MeetsConditions (CrewMember member)
	{
		bool hasTarget = false;
		
		foreach (var item in CombatManager.Instance.getCurrentFighters(Crews.otherSide(member.side)) ) {
			if (item.HasStatus(Fighter.Status.KnockedOut) == false ) {
				hasTarget = true;
				preferedTarget = item;
			}
		}

		return hasTarget && base.MeetsConditions (member);
	}
}
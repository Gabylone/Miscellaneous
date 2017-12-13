﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_RhumRound : Skill {

	public int healthToHeal = 60;

	public override void Start ()
	{
		base.Start ();
	}

	public override void TriggerSkill ()
	{

		base.TriggerSkill ();

		foreach (var targetFighter in CombatManager.Instance.getCurrentFighters (fighter.crewMember.side) ) {

			targetFighter.Heal (25);

		}

		EndSkill ();

	}

	public override bool MeetsConditions (CrewMember member)
	{

		bool allyInHelp = false;

		int count = 0;

		foreach (var item in Crews.getCrew(member.side).CrewMembers) {
			if (item.Health < healthToHeal) {
				++count;
				if ( count > 1 )
					allyInHelp = true;
			}
		}

		return allyInHelp && base.MeetsConditions (member);
	}
}
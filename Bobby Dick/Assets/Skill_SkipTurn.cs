﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_SkipTurn: Skill {

	public override void ApplyEffect ()
	{
		base.ApplyEffect ();

		CombatManager.Instance.NextTurn ();
	}

}

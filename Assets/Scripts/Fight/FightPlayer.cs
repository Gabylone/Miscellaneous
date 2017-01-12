﻿using UnityEngine;
using System.Collections;

public class FightPlayer : Humanoid {

	FightIA lastIAFight;

	void Start () {
		Init ();
	}

	void Update () {
		UpdateStateMachine ();
	}

	#region hit
	public override void hit_Start ()
	{
		base.hit_Start ();

		if ( lastIAFight != null ) {
			if ( Random.value < 0.5f )
				lastIAFight.ChangeState (states.guard);
		}
	}
	#endregion

	#region move
	public override void move_Update ()
	{
		base.move_Update ();

		Animator.SetFloat ("move" , Input.GetAxis ("Horizontal") != 0 ? 1 : 0);

		transform.Translate ( Direction * Input.GetAxis ("Horizontal") * Speed * Time.deltaTime);

		if (Input.GetKeyDown (KeyCode.D))
			ChangeState (states.hit);

//		if (Input.GetKeyDown (KeyCode.F))
//			ChangeState (states.blocked);
//
//		if (Input.GetKeyDown (KeyCode.G))
//			ChangeState (states.getHit);

		if (Input.GetKeyDown (KeyCode.DownArrow))
			ChangeState (states.guard);
		
	}
	#endregion

	#region guard
	public override void guard_Update ()
	{
		base.guard_Update ();

		if (Input.GetKeyUp (KeyCode.DownArrow))
			ChangeState (states.move);

	}
	#endregion

	#region collision
	public override void OnTriggerEnter2D (Collider2D other)
	{
		base.OnTriggerEnter2D (other);

		lastIAFight = other.GetComponentInParent<FightIA> ();
	}
	#endregion


}
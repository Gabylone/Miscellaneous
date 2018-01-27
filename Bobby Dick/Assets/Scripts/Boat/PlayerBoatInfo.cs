﻿using UnityEngine;
using System.Collections;

[System.Serializable]
public class PlayerBoatInfo : BoatInfo {
	
	public int shipRange = 1;

	public bool isInNoMansSea = false;
	public bool hasBeenWarned = false;

	// seulement lors d'une novelle partie
	public override void Randomize ()
	{
		base.Randomize ();

		coords = MapData.Instance.homeIslandCoords;
	}

	public override Coords coords {
		get {
			return base.coords;
		}
		set {
			base.coords = value;

			if (value.x < 0 || value.x > MapGenerator.Instance.MapScale - 1 || value.y < 0 || value.y > MapGenerator.Instance.MapScale - 1) {
				Narrator.Instance.ShowNarratorTimed("CAPITAINE entre dans un abîme d'océan, mieux vaut faire demi-tour");
			}
		}
	}
}

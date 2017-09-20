﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayHex : MonoBehaviour {

	Island island;

	void Start () {
		
	}

	public void UdpateHex ( Coords coords ) {
	
		island = GetComponentInChildren<Island> ();

		Chunk chunk = Chunk.GetChunk (Boats.PlayerBoatInfo.CurrentCoords);

		island.Init ();
		island.UpdatePositionOnScreen (coords);
	}
}

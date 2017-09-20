﻿
using System.Collections;
using System.Collections.Generic;

public enum ChunkState {
	//
	UndiscoveredSea,
	DiscoveredSea,
	UndiscoveredIsland,
	DiscoveredIsland,
	VisitedIsland

}

public class Chunk
{
	public static Dictionary<Coords,Chunk> chunks = new Dictionary<Coords, Chunk>();

	public int stateID;
	private IslandData islandData;

	public Chunk () {
	}

	public IslandData IslandData {
		get {
			return islandData;
		}
		set {
			islandData = value;

			State = ChunkState.UndiscoveredIsland;
		}
	}

	public ChunkState State {
		get {
			return (ChunkState)stateID;
		}
		set {
			stateID = (int)value;
		}
	}

	public static Chunk currentChunk {
		get {
			return chunks[Boats.PlayerBoatInfo.CurrentCoords];
		}
	}

	public static Chunk GetChunk (Coords c) {
		if (chunks.ContainsKey (c) == false) {
			return chunks [new Coords ()];
		}

		return chunks [c];
	}
}


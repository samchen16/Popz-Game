﻿using UnityEngine;
using System.Collections;

public class GroundGenerator : MonoBehaviour {

	public Transform groundPiece;
	public float pitSpawnChance = 0.15f;
	public float hillSpawnChance = 0.15f;

	private bool previousHasPit = false;


	// Use this for initialization
	void Start () {
		Grid grid = GameObject.FindGameObjectWithTag ("Grid").GetComponent<Grid> ();
		if (grid != null) {
			groundPiece.localScale = new Vector3(grid.cellSizeX, grid.cellSizeY, 1f);
		}
	}

	// Update is called once per frame
	public void GenerateGrounds (Grid grid, TerrainChunk tc) {
		for (int x = 0; x < grid.numCellsX; x++) {
			if (Random.value < pitSpawnChance && !previousHasPit) {
				previousHasPit = !grid.containsObject(x,0);
				continue;
			}
			GenerateGround (x, 0, grid, tc);
			if (Random.value < hillSpawnChance && !previousHasPit) {
				Transform h = GenerateGround (x, 1, grid, tc);
				h.tag = "Hill";
			}
			previousHasPit = false;
		}
	}

	public Transform GenerateGround (int x, int y, Grid grid, TerrainChunk tc) {
		if (grid.containsObject(x, y)) {
			return null;
		}
		Vector3 spawnPos = grid.GridToWorld (x,y) + tc.transform.position; 
		Transform t = GenerateGround (spawnPos.x, spawnPos.y);
		t.parent = tc.gameObject.transform;
		grid.MarkGrid (x, y);
		return t;
	}

	public Transform GenerateGround (float x, float y) {
		Vector3 spawnPos = new Vector3 (x,y,0); 
		Transform t = GameObject.Instantiate (groundPiece, spawnPos, Quaternion.identity) as Transform;
		return t;
	}


}

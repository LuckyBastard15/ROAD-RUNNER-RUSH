using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameState
{
    public Vector3 playerPosition;
    public List<TerrainState> terrains = new List<TerrainState>();
}

[Serializable]
public class TerrainState
{
    public Vector3 position;
    public string terrainType;
}

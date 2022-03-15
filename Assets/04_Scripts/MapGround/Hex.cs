using System;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class Hex : MonoBehaviour
{
	[SerializeField]
	private GlowHighlight highlight;
    private HexCoordinates hexCoordinates;

	private HexGrid myGrid;

	[SerializeField]
	private HexType tileType;

#region For AStar pathfinding - variable // will move to AstarNode
	public Hex Connection { get; private set; }
	public float G { get; private set; }
	public float H { get; private set; }
	public float F => G + H;
	public List<Hex> Neighbors = new List<Hex>();
#endregion

	public Vector3Int HexCoords => hexCoordinates.GetHexCoords();

#region Unity Functions
	private void Awake()
	{
		hexCoordinates = GetComponent<HexCoordinates>();
		highlight = GetComponent<GlowHighlight>();
		myGrid = GetComponentInParent<HexGrid>();
	}

	private void Start()
	{
		// Neighbors = myGrid.GetNeighboursHex(this.HexCoords);
	}
#endregion

#region Public Functions
	// Astar Functions
	public void SetConnection(Hex hex)
	{
		Connection = hex;
	}
	// Astar Functions
	public float GetDistance(Vector3Int target)
	{
		return AxialLength(this.HexCoords - target);
	}

	public int GetTileValue()
		=> tileType switch
		{
			HexType.Walkable => 1,
			HexType.NonWalkable => 2,
			HexType.Activetile => 1,
			_ => throw new Exception($"Hex type {tileType} not supported")
		};
	public int GetTileCost()
		=> tileType switch
		{
			HexType.Walkable => 1,
			HexType.NonWalkable => 2,
			HexType.Activetile => 1,
			_ => throw new Exception($"Hex type {tileType} not supported")
		};
	public bool IsNonMoveable()
	{
		return this.tileType == HexType.NonFlyable;
	}
	public void EnableHighlight()
	{
		highlight.ToggleGlow(true);
	}

	public void DisableHighlight()
	{
		highlight.ToggleGlow(false);
	}

	// Astar Functions
	public void SetG(float g)
	{
		G = g;
	}

	public void SetH(float h)
	{
		H = h;
	}
#endregion

#region Private Functions
	// Astar Functions
	private int AxialLength(Vector3Int Coords)
	{
		if (Coords.x == 0 && Coords.z == 0)
			return 0;
		if (Coords.x > 0 && Coords.z >= 0)
			return Coords.x + Coords.z;
		if (Coords.x <= 0 && Coords.z > 0)
			return -Coords.x < Coords.z ? Coords.z : -Coords.x;
		if (Coords.z < 0)
			return -Coords.x - Coords.z;
		return -Coords.z > Coords.x ? -Coords.z : Coords.x;
	}
#endregion
}

public enum HexType
{
	None,
	Walkable,
	NonWalkable,
	Activetile,
	NonFlyable
}
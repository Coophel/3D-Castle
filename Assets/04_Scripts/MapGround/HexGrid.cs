using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexGrid : MonoBehaviour
{
	Dictionary<Vector3Int, Hex> hexTileDict = new Dictionary<Vector3Int, Hex>();
	Dictionary<Vector3Int, List<Vector3Int>> hexTileNeighborsDict = new Dictionary<Vector3Int, List<Vector3Int>>();
	Dictionary<Vector3Int, List<Hex>> hexNeighborsDict = new Dictionary<Vector3Int, List<Hex>>();

#region Unity Functions
	private void Start()
	{
		foreach (Hex hex in FindObjectsOfType<Hex>())
		{
			hexTileDict[hex.HexCoords] = hex;
		}

		// List<Hex> nbors = GetNeighboursHex(new Vector3Int(0, 0, 0));
		// Debug.Log("Neighbors for (0, 0, 0) are : " + nbors.Count);
		// foreach (Hex nbor in nbors)
		// {
		// 	Debug.Log(nbor.HexCoords);
		// }
	}
#endregion

#region Public Functions
	public Hex GetTileAt(Vector3Int hexCoordinates)
	{
		Hex result = null;
		hexTileDict.TryGetValue(hexCoordinates, out result);
		return result;
	}

	public List<Hex> GetNeighboursHex(Vector3Int hexCoords)
	{
		Debug.Log("no tile from : " + hexCoords);
		if (hexTileDict.ContainsKey(hexCoords) == false)
			return new List<Hex>();

		Debug.Log("get neighbors from : " + hexCoords);
		if (hexNeighborsDict.ContainsKey(hexCoords))
			return hexNeighborsDict[hexCoords];

		hexNeighborsDict.Add(hexCoords, new List<Hex>());

		Debug.Log("Finding new neighbors");
		foreach (var direction in Direction.GetDirectionList(hexCoords.z))
		{
			if (hexTileDict.ContainsKey(hexCoords + direction))
				hexNeighborsDict[hexCoords].Add(hexTileDict[hexCoords + direction]);
		}
		Debug.Log("hexNeighboersDic Count : " + hexNeighborsDict.Count);
		return hexNeighborsDict[hexCoords];
	}

	public List<Vector3Int> GetNeighboursFor(Vector3Int hexCoordinates)
	{
		if (hexTileDict.ContainsKey(hexCoordinates) == false)
		{
			Debug.Log("didn`t ContainsKey");
			return new List<Vector3Int>();
		}

		if (hexTileNeighborsDict.ContainsKey(hexCoordinates))
		{
			Debug.Log("find mach Key");
			return hexTileNeighborsDict[hexCoordinates];
		}

		hexTileNeighborsDict.Add(hexCoordinates, new List<Vector3Int>());

		foreach (var direction in Direction.GetDirectionList(hexCoordinates.z))
		{
			if (hexTileDict.ContainsKey(hexCoordinates + direction))
				hexTileNeighborsDict[hexCoordinates].Add(hexCoordinates + direction);
		}

		return hexTileNeighborsDict[hexCoordinates];
	}

	public Vector3Int GetClosestHex(Vector3 worldposition)
	{
		worldposition.y = 0;
		return HexCoordinates.ConvertPositionToOffset(worldposition);
	}
#endregion
}

// solution for hex tile direction / it changes even or odd;
public static class Direction
{
	public static List<Vector3Int> directionsOffsetOdd = new List<Vector3Int>
	{
		new Vector3Int(-1, 0, 1), // North 1
		new Vector3Int(0, 0, 1), // North 2
		new Vector3Int(1, 0, 0), // East
		new Vector3Int(0, 0, -1), // South 2
		new Vector3Int(-1, 0, -1), // South 1
		new Vector3Int(-1, 0, 0), // West
	};

	public static List<Vector3Int> directionsOffsetEven = new List<Vector3Int>
	{
		new Vector3Int(0, 0, 1), // North 1
		new Vector3Int(1, 0, 1), // North 2
		new Vector3Int(1, 0, 0), //East
		new Vector3Int(1, 0, -1), // South 2
		new Vector3Int(0, 0, -1), // South 1
		new Vector3Int(-1, 0, 0), // West
	};

	// to check even or odd;
	public static List<Vector3Int> GetDirectionList(int z)
		=> z % 2 == 0 ? directionsOffsetEven : directionsOffsetOdd;
}
using System.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

public static class Astar
{
	public static List<Hex> FindPathHexAstar(Hex startHex, Hex targetHex)
	{
		List<Hex> toSearch = new List<Hex>() { startHex };
		List<Hex> processed = new List<Hex>();
		Debug.Log("FindPathAstar Start : " + toSearch[0].HexCoords);

		while (toSearch.Any())
		{
			Hex current = toSearch[0];
			foreach (Hex point in toSearch)
			{
				Debug.Log("Check Search point");
				if (point.F < current.F || point.F == current.F && point.H < current.H)
				{
					current = point;
				}
			}

			processed.Add(current);
			toSearch.Remove(current);
			foreach (var h in processed)
			{
				Debug.Log("In processed : " + h.HexCoords);
			}

			if (current == targetHex)
			{
				Debug.Log("Finish & return PathFinding");
				var currentPathTile = targetHex;
				var path = new List<Hex>();

				while (currentPathTile != targetHex)
				{
					path.Add(currentPathTile);
					currentPathTile = currentPathTile.Connection;
				}

				return path;
			}


			foreach (var n in current.Neighbors)
			{
				Debug.Log("path neighbors" + n.HexCoords);
			}
			Debug.Log("Checking UnitType & distance");
			// Check UnitType about is this Unit can move neighbors Tile Type.
			foreach (Hex neighbor in current.Neighbors.Where(t => !t.IsNonMoveable() && !processed.Contains(t)))
			{
				bool inSearch = toSearch.Contains(neighbor);

				var costToNeighbor = current.G + current.GetDistance(neighbor.HexCoords);
				Debug.Log("cost G : " + costToNeighbor);

				if (!inSearch || costToNeighbor < neighbor.G)
				{
					neighbor.SetG(costToNeighbor);
					neighbor.SetConnection(current);

					if (!inSearch)
					{
						neighbor.SetH(neighbor.GetDistance(targetHex.HexCoords));
						toSearch.Add(neighbor);
						Debug.Log("Search by Hex : " + toSearch[0].HexCoords);
					}
				}
			}
		}

		return null;
	}

	public static List<AstarNode> FindPathAstar(AstarNode startNode, AstarNode targetNode)
	{
		var toSearch = new List<AstarNode>() { startNode };
        var processed = new List<AstarNode>();

		while (toSearch.Any())
		{
			AstarNode current = toSearch[0];
			foreach (var t in toSearch)
			{
				if (t.F < current.F || t.F == current.F && t.H < current.H)
					current = t;
			}

			processed.Add(current);
			toSearch.Remove(current);

			if (targetNode.Myhex.IsNonWalkable() || targetNode.Myhex.IsNonMoveable())
			{
				foreach (var neighbor in targetNode.Neighbors)
				{
					if (current == neighbor)
					{
						Debug.Log("Finish & return at neighbor PathFinding");
						var currentPathTile = neighbor;
						var path = new List<AstarNode>();
						var count = 100;
						while (currentPathTile != startNode)
						{
							path.Add(currentPathTile);
							currentPathTile = currentPathTile.Connection;
							count--;
							if (count < 0) throw new Exception();
						}
						path.Reverse();
						return path;
					}
				}
			}
			else
			{
				if (current == targetNode)
				{
					Debug.Log("Finish & return PathFinding");
					var currentPathTile = targetNode;
					var path = new List<AstarNode>();
					var count = 100;
					while (currentPathTile != startNode)
					{
						path.Add(currentPathTile);
						currentPathTile = currentPathTile.Connection;
						count--;
						if (count < 0) throw new Exception();
					}
					path.Reverse();
					Debug.Log("path ----------------------");
					foreach (var h in path)
						Debug.Log(h.Myhex.HexCoords);
					return path;
				}
			}

			foreach (var neighbor in current.Neighbors.Where(t => !processed.Contains(t)))
			{
				var inSearch = toSearch.Contains(neighbor);
				var costToNeighbor = current.G + current.GetDistance(neighbor.Myhex.HexCoords);

				if (!inSearch || costToNeighbor < neighbor.G)
				{
					neighbor.SetG(costToNeighbor);
					neighbor.SetConnection(current);

					if (!inSearch)
					{
						neighbor.SetH(neighbor.GetDistance(targetNode.Myhex.HexCoords));
						toSearch.Add(neighbor);
					}
				}
			}
		}
		return null;
	}
}

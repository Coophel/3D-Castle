using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AstarNode : MonoBehaviour
{
	[SerializeField]
	private TextMeshPro _F, _G, _H;
	private HexGrid MyGrid;
	public Hex Myhex;
	public AstarNode Connection { get; private set; }
	public float G { get; private set; }
	public float H { get; private set; }
	public float F => G + H;
	public List<Vector3Int> NeighborsVect3 = new List<Vector3Int>();
	public List<Hex> NeighborsHex = new List<Hex>();
	public List<AstarNode> Neighbors = new List<AstarNode>();
    
#region Unity Functions
	private void Awake()
	{
		MyGrid = GetComponentInParent<HexGrid>();
		Myhex = GetComponent<Hex>();
	}
	private void Start()
	{
		NeighborsVect3 = MyGrid.GetNeighboursFor(Myhex.HexCoords);
		// Debug.Log(NeighborsVect3.Count);
		foreach (var n in NeighborsVect3)
		{
			NeighborsHex.Add(MyGrid.GetTileAt(n));
		}
		foreach (var n in NeighborsHex)
		{
			Neighbors.Add(n.GetComponent<AstarNode>());
		}
	}
	private void Update()
	{
		_F.text = F.ToString();
	}
#endregion

#region Public Functions
	public void SetConnection(AstarNode hex)
	{
		Connection = hex;
	}
	public float GetDistance(Vector3Int target)
	{
		return AxialLength(Vector3IntDistance(Myhex.HexCoords, target));
	}
	public void SetG(float g)
	{
		G = g;
		_G.text = g.ToString();
	}

	public void SetH(float h)
	{
		H = h;
		_H.text = h.ToString();
	}
#endregion

#region Private Functions
	private Vector3Int Vector3IntDistance(Vector3Int start, Vector3Int end)
	{
		Vector3Int result = new Vector3Int(0, 0, 0);
		bool sign = (start.x ^ end.x) < 0;

		if (sign)
			result.x = Mathf.Abs(start.x) + Mathf.Abs(end.x);
		else
			result.x = Mathf.Abs(start.x - end.x);

		sign = (start.z ^ end.z) < 0;

		if (sign)
			result.z = Mathf.Abs(start.z) + Mathf.Abs(end.z);
		else
			result.z = Mathf.Abs(start.z - end.z);

		return result;
	}
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexCoordinates : MonoBehaviour
{
    public static float xOffset = 2, yOffset = 1, zOffset = 1.73f;

	internal Vector3Int GetHexCoords() => offestCoordinates;

	[Header("Offset coordinates")]
	[SerializeField]
	private Vector3Int offestCoordinates;

#region Unity Functions
	private void Awake()
	{
		offestCoordinates = ConvertPositionToOffset(transform.position);
	}
#endregion

#region Private Functions
	private Vector3Int ConvertPositionToOffset(Vector3 position)
	{
		// RoundToInt can`t care of even of x so we have to use other solutions;
		// int x = Mathf.RoundToInt(position.x / xOffset);
		// int y = Mathf.RoundToInt(position.y / yOffset);
		// int z = Mathf.RoundToInt(position.z / zOffset);

		int x = Mathf.CeilToInt(position.x / xOffset);
		int y = Mathf.RoundToInt(position.y / yOffset);
		int z = Mathf.RoundToInt(position.z / zOffset);

		return new Vector3Int(x, y, z);
	}
#endregion
}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{

	private Camera cam = null;

	// for testing only
	public LayerMask selectionMask;
	public HexGrid hexGrid;

	private List<AstarNode> paths = new List<AstarNode>();
	private Hex leftselected;
	private AstarNode leftAstar;
	// end testing;

#region Unity Functions
	private void Start()
	{
		cam = Camera.main;
	}
	private void Update()
	{
		CheckMousepoint();
	}
#endregion

#region Private Functions
	private void CheckMousepoint()
	{
		if (Mouse.current.leftButton.wasPressedThisFrame)
		{
			Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
			RaycastHit hit;

			if (Physics.Raycast(ray, out hit, selectionMask))
			{
				AstarNode selectedHex = hit.collider.gameObject.GetComponent<AstarNode>();
				if (leftselected != selectedHex && leftselected != null)
				{
					leftselected.DisableHighlight();
				}
				leftselected = selectedHex.Myhex;
				leftAstar = selectedHex;

				selectedHex.Myhex.EnableHighlight();

				Debug.Log("leftclick Coord Neighbors : " + selectedHex.NeighborsHex.Count);

				// foreach(Vector3Int neighbor in neighbors)
				// {
				// 	hexGrid.GetTileAt(neighbor).DisableHighlight();
				// }
				
				// neighbors = hexGrid.GetNeighboursFor(selectedHex.HexCoords);

				// foreach (Vector3Int neighbor in neighbors)
				// {
				// 	hexGrid.GetTileAt(neighbor).EnableHighlight();
				// }

				// Debug.Log($"Neighbors for {selectedHex.HexCoords} are :");
				// foreach (Vector3Int neighbor in neighbors)
				// {
				// 	Debug.Log(neighbor);
				// }
			}
		}

		if (Mouse.current.rightButton.wasPressedThisFrame)
		{
			Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
			RaycastHit hit;

			if (Physics.Raycast(ray, out hit, selectionMask))
			{
				AstarNode selectedHex = hit.collider.gameObject.GetComponent<AstarNode>();
				Debug.Log("right clicked : " + selectedHex.Myhex.HexCoords);


				if (paths != null)
				{
					foreach (AstarNode path in paths)
					{
						hexGrid.GetTileAt(path.Myhex.HexCoords).DisableHighlight();
					}
				}

				paths = Astar.FindPathAstar(leftAstar, selectedHex);
				Debug.Log(paths.Count);

				if (paths == null)
				{
					Debug.Log("not get Astar path");
					return ;
				}

				foreach (AstarNode path in paths)
				{
					Debug.Log(path.Myhex.HexCoords);
				}

				foreach (AstarNode path in paths)
				{
					hexGrid.GetTileAt(path.Myhex.HexCoords).EnableHighlight();
				}
			}
		}
	}
#endregion
}

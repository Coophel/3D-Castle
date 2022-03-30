using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class PlayerInput : MonoBehaviour
{
	public static PlayerInput Instance;

	private Camera cam = null;

	// for testing only
	public LayerMask selectionMask;
	public HexGrid hexGrid;
	// public Unit testunit;

	public UnityEvent<GameObject> OnUnitSelect;
	public UnityEvent<GameObject> OnTerrainSelect;
	public UnityEvent<GameObject> OnBuildingSelect;


	private List<AstarNode> paths = new List<AstarNode>();
	private Hex leftselected;
	private AstarNode leftAstar;
	// end testing;

#region Unity Functions
	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			Destroy(this.gameObject);
		}
	}
	private void Start()
	{
		cam = Camera.main;
	}
	private void Update()
	{
		CheckMousepoint();
	}
#endregion

#region Public Functions
	public void MouseHandle(Vector3 mousePosition)
	{
		Debug.Log("MouseHandle!" + mousePosition);
		GameObject result;
		
		if (FindTarget(mousePosition, out result))
		{
			Debug.Log(result.name);
			if (UnitSelect(result))
				OnUnitSelect?.Invoke(result);
			else if (BuildingSelect(result))
				OnBuildingSelect?.Invoke(result);
			else
				OnTerrainSelect?.Invoke(result);
		}
	}

	public void UnitSeleeet()
	{
		Debug.Log("UnitSeleeet!");
	}
#endregion

#region Private Functions
	private bool BuildingSelect(GameObject result)
	{
		return result.GetComponent<Building>() != null;
	}
	private bool UnitSelect(GameObject result)
	{
		return result.GetComponent<UnitStateMachine>() != null;
	}
	private bool FindTarget(Vector3 mousePosition, out GameObject result)
	{
		RaycastHit hit;
		Ray ray = cam.ScreenPointToRay(mousePosition);

		if (Physics.Raycast(ray, out hit, selectionMask))
		{
			result = hit.collider.gameObject;
			return true;
		}
		result = null;
		return false;
	}
	private void CheckMousepoint()
	{
		if (Mouse.current.leftButton.wasPressedThisFrame)
		{
			MouseHandle(Mouse.current.position.ReadValue());
		}
	}
#endregion
}
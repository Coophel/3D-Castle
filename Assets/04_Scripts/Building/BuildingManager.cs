using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    public static BuildingManager Instance;

	public List<GameObject> BuildingPrefab;
	public GameObject SelectedBuilding;
	public bool IsBuildMode;

	public Hex SelectedHex;

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
		SelectedBuilding = BuildingPrefab[0];
		PlayerInput.Instance.OnTerrainSelect.AddListener(CheckAndBulid);
	}

	private void OnDisable()
	{
		PlayerInput.Instance.OnTerrainSelect.RemoveListener(CheckAndBulid);
	}
#endregion

#region Public Functions
	public void On_Click_Build()
	{
		IsBuildMode = !IsBuildMode;
	}

	public void On_Click_SelectBulid(GameObject selected)
	{
		SelectedBuilding = selected;
	}
#endregion

#region Private Functions
	private void CheckAndBulid(GameObject target)
	{
		Debug.Log(target.name + "call by event in OnTerrainSelect");
		if (IsBuildMode)
		{
			if (target.GetComponentInChildren<Building>())
			{
				// Can`t bulid here messege;
				Debug.Log("Building is already exists");
				return;
			}
			else
			{
				var now = target.GetComponent<Hex>();
				Debug.Log("Building build");
				if (SelectedBuilding != null)
				{
					var build = Instantiate(SelectedBuilding, target.transform.position + new Vector3(0, 1, 0), Quaternion.identity);
					build.transform.parent = target.gameObject.transform.Find("props");
				}
			}
		}
		else
		{
			if (target.GetComponentInChildren<Building>())
			{
				Debug.Log("Select Building");
				var now = target.GetComponent<Hex>();
				if (SelectedHex != null && now != SelectedHex)
					SelectedHex.DisableHighlight();
				now.EnableHighlight();
				SelectedHex = now;
			}
			else if (target.GetComponent<Hex>())
			{
				Debug.Log("Select terrain");
				var now = target.GetComponent<Hex>();
				if (SelectedHex != null && now != SelectedHex)
					SelectedHex.DisableHighlight();
				now.EnableHighlight();
				SelectedHex = now;
			}
		}
	}
#endregion
}

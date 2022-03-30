using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSpawn : MonoBehaviour
{
	// tile list to spawn unit
    public List<GameObject> _listOfAvailableTile = new List<GameObject>();
	// unit list to selected when spawning
	public List<GameObject> _unitSelections = new List<GameObject>();
	// WeightRandom total number
	public int _totalRandom = 0;


	// test variables
	// float _testtime = 0f;

#region Unity Functions
	// private void Update()
	// {
	// 	// Test code
	// 	// _testtime += Time.deltaTime;
	// 	// if (_testtime >= 5.0f)
	// 	// {
	// 	// 	if (SpawnUnit())
	// 	// 		Debug.Log("unit spawn");
	// 	// 	else
	// 	// 		Debug.Log("no more spawn");
			
	// 	// 	_testtime = 0f;
	// 	// }
	// }
#endregion

#region Public Functions
	public void UnitSpawn_Button()
	{
		SpawnUnit();
	}
	public bool SpawnUnit()
	{
		CheckAvailableTile();
		GetAddUnit();
		GameObject selectedUnit = RandomUnitSelection();
		
		if (_listOfAvailableTile.Count == 0 || selectedUnit == null)
			return false;

		GameObject tileSpawn = _listOfAvailableTile[Random.Range(0, _listOfAvailableTile.Count - 1)];
		// Instantiate Unit!
		GameObject _currentUnit = Instantiate(selectedUnit, tileSpawn.transform.position, tileSpawn.transform.rotation);

		_listOfAvailableTile.Remove(tileSpawn);
		return true;
	}
#endregion

#region Private Functions
	private GameObject RandomUnitSelection()
	{
		int weight = 0;
		int selectNum = 0;

		selectNum = Mathf.RoundToInt(_totalRandom * Random.Range(0f, 1.0f));

		for (int i = 0; i < _unitSelections.Count; i++)
		{
			weight += _unitSelections[i].GetComponent<UnitStateMachine>().UnitObject.UnitDrawWeight;
			if (selectNum <= _unitSelections[i].GetComponent<UnitStateMachine>().UnitObject.UnitDrawWeight)
			{
				return _unitSelections[i];
			}
		}

		return null;
	}
	private void GetAddUnit()
	{
		_totalRandom = 0;
		foreach (GameObject unit in _unitSelections)
		{
			// Debug.Log(unit.GetComponent<UnitStateMachine>().UnitObject);
			_totalRandom += unit.GetComponent<UnitStateMachine>().UnitObject.UnitDrawWeight;
		}
	}
	private void CheckAvailableTile()
	{
		_listOfAvailableTile.Clear();
		foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Base_Ground"))
		{
			if (obj.GetComponent<Hex>().GettileType() == HexType.Walkable)
				_listOfAvailableTile.Add(obj);
		}
		Debug.Log("AvailableTile : " + _listOfAvailableTile.Count);
	}
#endregion
}
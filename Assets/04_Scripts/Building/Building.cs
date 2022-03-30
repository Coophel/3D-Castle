using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[SelectionBase]
public class Building : MonoBehaviour
{
    [SerializeField]
	Hex _posHex;
	[SerializeField]
	BuildStates _buildObject;
	BuildState _buildState;

#region Unity Functions
	private void Awake()
	{
		_posHex = GetComponentInParent<Hex>();
		_buildState = new BuildState(_buildObject);
	}

	private void Start()
	{
		SetHexType();
	}

	private void OnEnable()
	{
		Debug.Log("OnEnable by " + _buildState.buildName);
		StartCoroutine("GetposHex");
	}
#endregion

#region Public Functions
	// public void Deselect()
	// {
	// 	_glowHighlight.ToggleGlow(false);
	// }
	// public void Select()
	// {
	// 	_glowHighlight.ToggleGlow();
	// }
	public void SetHexType()
	{
		switch (_buildState.HeightLevel)
		{
			case 0:
				_posHex.ChangeType(HexType.Walkable);
				break;
			case 1:
				_posHex.ChangeType(HexType.NonWalkable);
				break;
			case 2:
				_posHex.ChangeType(HexType.NonFlyable);
				break;
		}
	}
#endregion

#region Private Functions
	private IEnumerator GetposHex()
	{
		while (_posHex == null)
		{
			_posHex = GetComponentInParent<Hex>();
			yield return null;
		}
	}
#endregion
}

public class BuildState
{
	public GameObject buildPrefab;
	public string buildName;
	public int MaxHp;
	public int CurrentHp;
	public int HeightLevel;
	public int NeedWorker;
	public BuildType buildType;
	public float upValuepercent;
	public int upIncome;
	public int upWorker;
	// for DefBuild
	public int AttackRange;
	public int AttackSpeed;
	public int AttackDamage;

	public BuildState(BuildStates value)
	{
		this.buildName = value.buildName;
		this.buildPrefab = value.buildPrefab;
		this.MaxHp = value.MaxHp;
		this.CurrentHp = value.CurrentHp;
		this.HeightLevel = value.HeightLevel;
		this.NeedWorker = value.NeedWorker;
		this.buildType = value.buildType;
		this.upValuepercent = value.upValuepercent;
		this.upIncome = value.upIncome;
		this.upWorker = value.upWorker;
		this.AttackRange = value.AttackRange;
		this.AttackSpeed = value.AttackSpeed;
		this.AttackDamage = value.AttackDamage;
	}
}

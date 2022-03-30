using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class UnitStateMachine : MonoBehaviour
{
	[SerializeField]
	UnitStates _unitObject;	// get unit type by scriptableObject;
	UnitState _unitState;	// Unit`s state;

	// About Hex Tile variables
	public HexGrid _hexGrid;
	public Hex TestTarget;
	[SerializeField]
	Hex _unitHex;
	private Hex _lastHex;
	[SerializeField]
	AstarNode _unitAstarNode;

	// Move variables
	[SerializeField]
	float _movementDuration = 1, _rotationDuration = 0.3f;

	// Check selection about Unit variavle
	GlowHighlight _glowHighlight;

	// Astar path variables
	// test moveing make it more systematic
	[SerializeField]
	public Unit target;
	bool _moveFinish;
	public Queue<Vector3> _pathPositions = new Queue<Vector3>();
	public AstarNode _targetPosition = null;
	public List<AstarNode> _astarPaths = new List<AstarNode>();
	public List<Vector3> _paths = new List<Vector3>();

	// FSM variavle
	CharBaseState _currentState;
	CharStateFactory _states;

	// Getter & Setter
	public CharBaseState CurrentState { get { return _currentState; } set { _currentState = value; }}
	public Hex UnitHex { get { return _unitHex; } set { _unitHex = value; }}
	public AstarNode UnitAstarNode { get { return _unitAstarNode; } set { _unitAstarNode = value; }}
	public UnitStates UnitObject {get { return _unitObject; }}
	public UnitState UnitState { get { return _unitState; }}
	public bool MoveFinish { get { return _moveFinish; } set { _moveFinish = value; }}

#region Unity Functions
	private void Awake()
	{
		// set reference variables
		_glowHighlight = this.GetComponent<GlowHighlight>();
		_unitState = new UnitState(_unitObject);

		// setup state
		_states = new CharStateFactory(this);
		_currentState = _states.Idle();
		_hexGrid = GameObject.Find("BaseGround").GetComponent<HexGrid>();
	}

	private void Start()
	{
		PlayerInput.Instance.OnUnitSelect.AddListener(UnitSelect);
	}

	private void Update()
	{
		if (_lastHex != null)
			_lastHex.ChangeType(HexType.Walkable);

		_unitHex = _hexGrid.GetTileAt(_hexGrid.GetClosestHex(transform.position));
		_lastHex = _unitHex;
		_unitHex.ChangeType(HexType.NonWalkable);

		_currentState.UpdateState();
	}

	private void OnDisable()
	{
		PlayerInput.Instance.OnUnitSelect.RemoveListener(UnitSelect);
	}
#endregion

#region Public Functions
	public void Deselect()
	{
		_glowHighlight.ToggleGlow(false);
	}
	public void Select()
	{
		_glowHighlight.ToggleGlow();
	}
	public void GetNextPath(List<AstarNode> aStarpath)
	{
		_paths = aStarpath.Select(pos => _hexGrid.GetTileAt(pos.Myhex.HexCoords).transform.position).ToList();
	}
#endregion

#region Private Functions
	private void UnitSelect(GameObject target)
	{
		Debug.Log("Call by Events in OnUnitSelect");
		var now = target.GetComponent<UnitStateMachine>();
		Debug.Log("Select Unit" + now._unitState.UnitName);
	}
#endregion
}

public class UnitState
{
	public string UnitName;
	public int UnitDrawWeight;
	public int MaxHp;	public int CurrentHp;
	public int MaxMp;	public int CurrentMp;
	public int AttackRange;	public float AttackSpeed;
	public float MoveSpeed;
	// Advanced properties
	public DefType UnitDefType;
	public AtkType UnitAtkType;
	public MoveType UnitMoveType;
	public UnitType UnitType;
	public UnitSkillState Unityskillstate;

	// 생성자
	public UnitState(UnitStates unit)
	{
		this.UnitName = unit.UnitName;
		this.UnitDrawWeight = unit.UnitDrawWeight;
		this.MaxHp = unit.MaxHp;	this.CurrentHp = unit.CurrentHp;
		this.MaxMp = unit.MaxMp;	this.CurrentMp = unit.CurrentMp;
		this.AttackRange = unit.AttackRange;	this.AttackSpeed = unit.AttackSpeed;	this.MoveSpeed = unit.MoveSpeed;

		this.UnitDefType = unit.UnitDefType;
		this.UnitAtkType = unit.UnitAtkType;
		this.UnitMoveType = unit.UnitMoveType;
		this.Unityskillstate = unit.Unityskillstate;
	}
}
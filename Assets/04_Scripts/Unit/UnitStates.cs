using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class UnitStates : ScriptableObject
{
	public string UnitName;
	public int UnitDrawWeight;
	public int MaxHp;
	public int CurrentHp;
	public int MaxMp;
	public int CurrentMp;
	public int AttackRange;
	public float AttackSpeed;
	public float MoveSpeed;
	
	// Advanced properties
	public DefType UnitDefType;
	public AtkType UnitAtkType;
	public MoveType UnitMoveType;
	public UnitType UnitType;
	public UnitSkillState Unityskillstate;
}

public enum DefType
{
	lightArmor,
	HeavyArmor
};

public enum AtkType
{
	normal,
	Magical
};

public enum MoveType
{
	Walk,
	Fly,
	Magic
};

public enum UnitType
{
	Human,
	Superman,
	Monster,
	Hextech
};

public enum UnitSkillState
{
	None,
	Stune,
	Fire,
	Ice
};
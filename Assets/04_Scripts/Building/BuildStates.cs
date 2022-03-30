using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
public class BuildStates : ScriptableObject
{
	public GameObject buildPrefab;
	public string buildName;
	public int MaxHp;
	public int CurrentHp;
	public int HeightLevel;
	public int NeedWorker;

	// Advanced properties
	public BuildType buildType;
	public float upValuepercent;
	public int upIncome;
	public int upWorker;

	// for DefBuild
	public int AttackRange;
	public int AttackSpeed;
	public int AttackDamage;
}

public enum BuildType
{
	None = 0,
	Main,
	Soilder,
	Knight,
	Magition,
	Monster,
	Money,
	DefBuild,
};